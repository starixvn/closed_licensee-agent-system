// ----------------------------------------------------------------------
// <copyright file="ExtentedStringWriter.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// ------------------------------------------------------------------------

namespace LicenseeAgent.Taiwan.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class ExtentedStringWriter : StringWriter
    {
        private readonly Encoding _writerEncoding;

        public ExtentedStringWriter(Encoding desiredEncoding)
            : base()
        {
            _writerEncoding = desiredEncoding;
        }

        public override Encoding Encoding
        {
            get
            {
                return _writerEncoding;
            }
        }
    }
}
