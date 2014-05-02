// --------------------------------------------------------------------------
// <copyright file="LessTransform.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Site
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Optimization;
    using dotless.Core;
    using dotless.Core.Abstractions;
    using dotless.Core.Importers;
    using dotless.Core.Input;
    using dotless.Core.Loggers;
    using dotless.Core.Parser;

    /// <summary>
    /// The less transform.
    /// </summary>
    public class LessTransform : IBundleTransform
    {
        /// <summary>
        /// The process.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="bundle">
        /// The bundle.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Argument NULL Exception.
        /// </exception>
        public void Process(BundleContext context, BundleResponse bundle)
        {
            if (!BundleTable.EnableOptimizations)
            {
                return;
            }

            context.HttpContext.Response.Cache.SetLastModifiedFromFileDependencies();
            var content = new StringBuilder(bundle.Content.Length);
            var bundleFiles = new List<BundleFile>();

            foreach (var bundleFile in bundle.Files)
            {
                bundleFiles.Add(bundleFile);
                var transformContent = Transform(bundleFile, bundleFiles);
                content.Append(transformContent);
                content.AppendLine();
            }

            // include imports in bundle files to register cache dependencies
            bundle.Files = bundleFiles.Distinct();

            bundle.ContentType = "text/css";
            bundle.Content = content.ToString();
        }

        /// <summary>
        /// Transforms the specified bundle file.
        /// </summary>
        /// <param name="bundleFile">The bundle file.</param>
        /// <param name="bundleFiles">The bundle files.</param>
        /// <returns>Returns a transformed content.</returns>
        private string Transform(BundleFile bundleFile, List<BundleFile> bundleFiles)
        {
            var filePath = bundleFile.IncludedVirtualPath;
            filePath = filePath.Replace('\\', '/');
            if (filePath.StartsWith("~"))
            {
                filePath = VirtualPathUtility.ToAbsolute(filePath);
            }

            if (filePath.StartsWith("/"))
            {
                filePath = HostingEnvironment.MapPath(filePath);
            }

            var fileExtension = Path.GetExtension(filePath).ToLower();
            var transformContent = File.ReadAllText(filePath);
            if (fileExtension == ".less")
            {
                var lessParser = new Parser();
                var lessEngine = this.CreateLessEngine(lessParser);
                SetCurrentFilePath(lessParser, filePath);
                transformContent = lessEngine.TransformToCss(transformContent, filePath);
                bundleFiles.AddRange(GetFileDependencies(lessParser, bundleFile.VirtualFile));
            }

            foreach (var transform in bundleFile.Transforms)
            {
                transformContent = transform.Process(bundleFile.IncludedVirtualPath, transformContent);
            }

            // apply URL rewrite
            var urlTransform = new CssRewriteUrlTransform();
            transformContent = urlTransform.Process("~" + VirtualPathUtility.ToAbsolute(bundleFile.IncludedVirtualPath), transformContent);

            return transformContent;
        }

        /// <summary>
        /// Creates an instance of LESS engine.
        /// </summary>
        /// <param name="lessParser">
        /// The LESS parser.
        /// </param>
        /// <returns>
        /// The <see cref="ILessEngine"/>.
        /// </returns>
        private ILessEngine CreateLessEngine(Parser lessParser)
        {
            var logger = new AspNetTraceLogger(LogLevel.Debug, new Http());
            return new LessEngine(lessParser, logger, true, false);
        }

        /// <summary>
        /// Gets the file dependencies (@imports) of the LESS file being parsed.
        /// </summary>
        /// <param name="lessParser">The LESS parser.</param>
        /// <param name="virtualFile">The virtual file.</param>
        /// <returns>An array of file references to the dependent file references.</returns>
        private IEnumerable<BundleFile> GetFileDependencies(Parser lessParser, VirtualFile virtualFile)
        {
            var pathResolver = this.GetPathResolver(lessParser);

            foreach (var importPath in lessParser.Importer.Imports)
            {
                yield return new BundleFile(pathResolver.GetFullPath(importPath), virtualFile);
            }

            lessParser.Importer.Imports.Clear();
        }

        /// <summary>
        /// Returns an <see cref="IPathResolver"/> instance used by the specified LESS lessParser.
        /// </summary>
        /// <param name="lessParser">
        /// The LESS parser.
        /// </param>
        /// <returns>
        /// The <see cref="IPathResolver"/>.
        /// </returns>
        private IPathResolver GetPathResolver(Parser lessParser)
        {
            var importer = lessParser.Importer as Importer;
            if (importer != null)
            {
                var fileReader = importer.FileReader as FileReader;

                if (fileReader != null)
                {
                    return fileReader.PathResolver;
                }
            }

            return null;
        }

        /// <summary>
        /// Informs the LESS parser about the path to the currently processed file.
        /// This is done by using a custom <see cref="IPathResolver"/> implementation.
        /// </summary>
        /// <param name="lessParser">The LESS parser.</param>
        /// <param name="currentFilePath">The path to the currently processed file.</param>
        private void SetCurrentFilePath(Parser lessParser, string currentFilePath)
        {
            var importer = lessParser.Importer as Importer;

            if (importer == null)
            {
                throw new InvalidOperationException("Unexpected dotless importer type.");
            }

            var fileReader = importer.FileReader as FileReader;

            if (fileReader == null || !(fileReader.PathResolver is ImportedFilePathResolver))
            {
                fileReader = new FileReader(new ImportedFilePathResolver(currentFilePath));
                importer.FileReader = fileReader;
            }
        }
    }
}