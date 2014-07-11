namespace LicenseeAgent.Site
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Hosting;
    using dotless.Core.Input;

    /// <summary>
    /// The imported file path resolver.
    /// </summary>
    public class ImportedFilePathResolver : IPathResolver
    {
        /// <summary>
        /// The current file directory.
        /// </summary>
        private string _currentFileDirectory;

        /// <summary>
        /// The current file path.
        /// </summary>
        private string _currentFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportedFilePathResolver"/> class.
        /// </summary>
        /// <param name="currentFilePath">
        /// The current file path.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Current file path should not be null nor empty.
        /// </exception>
        public ImportedFilePathResolver(string currentFilePath)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                throw new ArgumentNullException("currentFilePath");
            }

            this.CurrentFilePath = currentFilePath;
        }

        /// <summary>
        /// Gets or sets the path to the currently processed file.
        /// </summary>
        /// <value>
        /// The current file path.
        /// </value>
        public string CurrentFilePath
        {
            get
            {
                return this._currentFilePath;
            }

            set
            {
                this._currentFilePath = value;
                this._currentFileDirectory = Path.GetDirectoryName(value);
            }
        }

        /// <summary>
        /// Returns the absolute path for the specified imported file path.
        /// </summary>
        /// <param name="path">
        /// The imported file path.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetFullPath(string path)
        {
            if (path.StartsWith("~", StringComparison.OrdinalIgnoreCase))
            {
                path = VirtualPathUtility.ToAbsolute(path);
            }

            if (path.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                path = HostingEnvironment.MapPath(path);
            }
            else if (!Path.IsPathRooted(path))
            {
                path = Path.GetFullPath(Path.Combine(this._currentFileDirectory, path));
            }

            return path;
        }
    }
}