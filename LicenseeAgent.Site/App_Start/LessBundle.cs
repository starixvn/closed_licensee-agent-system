// --------------------------------------------------------------------------
// <copyright file="LessBundle.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Site
{
    using System.Web.Optimization;

    /// <summary>
    /// The less bundle.
    /// </summary>
    public class LessBundle : Bundle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LessBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path.
        /// </param>
        public LessBundle(string virtualPath)
            : base(virtualPath, new IBundleTransform[] { new LessTransform() })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LessBundle"/> class.
        /// </summary>
        /// <param name="virtualPath">
        /// The virtual path.
        /// </param>
        /// <param name="cdnPath">
        /// The CDN path.
        /// </param>
        public LessBundle(string virtualPath, string cdnPath)
            : base(virtualPath, cdnPath, new IBundleTransform[] { new LessTransform() })
        {
        }
    }
}