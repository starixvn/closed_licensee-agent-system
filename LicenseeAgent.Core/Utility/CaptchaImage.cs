// --------------------------------------------------------------------------
// <copyright file="CaptchaImage.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace LicenseeAgent.Core.Utility
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Text;
    using System.Web;
    using LicenseeAgent.Core.Configurations;

    /// <summary>
    /// Amount of background noise to add to rendered image
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "*",
        Justification = "Reviewed. Suppression is OK here.")]
    public enum BackgroundNoiseLevel
    {
        /// <summary>
        ///
        /// </summary>
        None,

        /// <summary>
        ///
        /// </summary>
        Low,

        /// <summary>
        ///
        /// </summary>
        Medium,

        /// <summary>
        ///
        /// </summary>
        High,

        /// <summary>
        ///
        /// </summary>
        Extreme
    }

    /// <summary>
    /// Amount of random font warping to apply to rendered text.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "*",
        Justification = "Reviewed. Suppression is OK here.")]
    public enum FontWarpFactor
    {
        /// <summary>
        ///
        /// </summary>
        None,

        /// <summary>
        ///
        /// </summary>
        Low,

        /// <summary>
        ///
        /// </summary>
        Medium,

        /// <summary>
        ///
        /// </summary>
        High,

        /// <summary>
        ///
        /// </summary>
        Extreme
    }

    /// <summary>
    /// Amount of curved line noise to add to rendered image
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "*",
        Justification = "Reviewed. Suppression is OK here.")]
    public enum LineNoiseLevel
    {
        /// <summary>
        ///
        /// </summary>
        None,

        /// <summary>
        ///
        /// </summary>
        Low,

        /// <summary>
        ///
        /// </summary>
        Medium,

        /// <summary>
        ///
        /// </summary>
        High,

        /// <summary>
        ///
        /// </summary>
        Extreme
    }

    /// <summary>
    /// CAPTCHA Image.
    /// </summary>
    /// <seealso href="http://www.codinghorror.com">Original By Jeff Atwood.</seealso>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
               "StyleCop.CSharp.Nexcel.NexcelCustomRules",
               "*",
               Justification = "Reviewed. Suppression is OK here.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "*",
        Justification = "Reviewed. Suppression is OK here.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.OrderingRules",
        "*",
        Justification = "Reviewed. Suppression is OK here.")]
    public class CaptchaImage
    {
        #region Static

        /// <summary>
        /// Gets the cached captcha.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        public static CaptchaImage GetCachedCaptcha(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return null;
            }

            return (CaptchaImage)HttpRuntime.Cache.Get(guid);
        }

        /// <summary>
        /// List of fonts to random.
        /// </summary>
        private readonly string[] randomFontFamily =
        {
            "arial", "arial black", "comic sans ms", "courier new",
            "estrangelo edessa", "franklin gothic medium", "georgia", "lucida console", "lucida sans unicode", "mangal",
            "microsoft sans serif", "palatino linotype", "sylfaen", "tahoma", "times new roman", "trebuchet ms",
            "verdana"
        };

        /// <summary>
        /// Gets or sets a string of available text characters for the generator to use.
        /// </summary>
        /// <value>The text chars.</value>
        public static string TextChars { get; set; }

        /// <summary>
        /// Gets or sets the length of the text.
        /// </summary>
        /// <value>The length of the text.</value>
        public static int TextLength { get; set; }

        /// <summary>
        /// Gets and sets amount of random warping to apply to the <see cref="CaptchaImage"/> instance.
        /// </summary>
        /// <value>The font warp.</value>
        public static FontWarpFactor FontWarp { get; set; }

        /// <summary>
        /// Gets and sets amount of background noise to apply to the <see cref="CaptchaImage"/> instance.
        /// </summary>
        /// <value>The background noise.</value>
        public static BackgroundNoiseLevel BackgroundNoise { get; set; }

        /// <summary>
        /// Gets or sets amount of line noise to apply to the <see cref="CaptchaImage"/> instance.
        /// </summary>
        /// <value>The line noise.</value>
        public static LineNoiseLevel LineNoise { get; set; }

        /// <summary>
        /// Gets or sets the cache time out.
        /// </summary>
        /// <value>The cache time out.</value>
        public static double CacheTimeOut { get; set; }

        #endregion Static

        private int _height;
        private int _width;
        private readonly Random _rand;

        #region Public Properties

        /// <summary>
        /// Returns a GUID that uniquely identifies this Captcha.
        /// </summary>
        /// <value>The unique id.</value>
        public string UniqueId { get; private set; }

        /// <summary>
        /// Returns the date and time this image was last rendered.
        /// </summary>
        /// <value>The rendered at.</value>
        public DateTime RenderedAt { get; private set; }

        /// <summary>
        /// Gets the randomly generated Captcha text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Width of Captcha image to generate, in pixels.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                if (value <= 60)
                {
                    const string ParamName = "width";
                    throw new ArgumentOutOfRangeException(ParamName, value, "width must be greater than 60.");
                }

                _width = value;
            }
        }

        /// <summary>
        /// Height of Captcha image to generate, in pixels.
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get
            {
                return _height;
            }

            set
            {
                if (value <= 30)
                {
                    const string ParamName = "height";
                    throw new ArgumentOutOfRangeException(ParamName, value, "height must be greater than 30.");
                }

                _height = value;
            }
        }

        public Color BackColor
        {
            get;
            set;
        }

        #endregion Public Properties

        /// <summary>
        /// Initializes the <see cref="CaptchaImage"/> class.
        /// </summary>
        static CaptchaImage()
        {
            FontWarp = FontWarpFactor.None;
            BackgroundNoise = BackgroundNoiseLevel.Low;
            LineNoise = LineNoiseLevel.None;
            TextLength = 6;
            TextChars = "acdefghjknpqruvxyzACDEFGHJKLNPQRTUVXYZ2346789";
            CacheTimeOut = 180D;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaImage"/> class.
        /// </summary>
        public CaptchaImage()
        {
            _rand = new Random();
            Width = Constants.CaptchaWidth;
            Height = Constants.CaptchaHeight;
            BackColor = Constants.CaptchaBackColor;
            Text = GenerateRandomText();
            RenderedAt = DateTime.Now;
            UniqueId = Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Forces a new Captcha image to be generated using current property value settings.
        /// </summary>
        /// <returns></returns>
        public Bitmap RenderImage()
        {
            return GenerateImagePrivate();
        }

        /// <summary>
        /// Returns a random font family from the font whitelist.
        /// </summary>
        /// <returns></returns>
        private string GetRandomFontFamily()
        {
            return randomFontFamily[_rand.Next(0, randomFontFamily.Length)];
        }

        /// <summary>
        /// Generate random text for the CAPTCHA.
        /// </summary>
        /// <returns></returns>
        private string GenerateRandomText()
        {
            var sb = new StringBuilder(TextLength);
            int maxLength = TextChars.Length;
            for (int n = 0; n <= TextLength - 1; n++)
            {
                sb.Append(TextChars.Substring(_rand.Next(maxLength), 1));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a random point within the specified x and y ranges.
        /// </summary>
        /// <param name="xmin">The xmin.</param>
        /// <param name="xmax">The xmax.</param>
        /// <param name="ymin">The ymin.</param>
        /// <param name="ymax">The ymax.</param>
        /// <returns></returns>
        private PointF RandomPoint(int xmin, int xmax, int ymin, int ymax)
        {
            return new PointF(_rand.Next(xmin, xmax), _rand.Next(ymin, ymax));
        }

        /// <summary>
        /// Randoms the color.
        /// </summary>
        /// <returns></returns>
        private static Color GetRandomColor()
        {
            return Constants.CaptchaForeColor;
        }

        /// <summary>
        /// Returns a random point within the specified rectangle.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        private PointF RandomPoint(Rectangle rect)
        {
            return RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }

        /// <summary>
        /// Returns a GraphicsPath containing the specified string and font.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="f"></param>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        private static GraphicsPath TextPath(string s, Font f, Rectangle r)
        {
            var sf = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near
            };

            var gp = new GraphicsPath();
            gp.AddString(s, f.FontFamily, (int)f.Style, f.Size, r, sf);
            return gp;
        }

        /// <summary>
        /// Returns the CAPTCHA font in an appropriate size.
        /// </summary>
        /// <returns></returns>
        private Font GetFont()
        {
            float fsize;
            string fname = GetRandomFontFamily();

            switch (FontWarp)
            {
                case FontWarpFactor.None:
                    goto default;
                case FontWarpFactor.Low:
                    fsize = Convert.ToInt32(_height * 0.8);
                    break;

                case FontWarpFactor.Medium:
                    fsize = Convert.ToInt32(_height * 0.85);
                    break;

                case FontWarpFactor.High:
                    fsize = Convert.ToInt32(_height * 0.9);
                    break;

                case FontWarpFactor.Extreme:
                    fsize = Convert.ToInt32(_height * 0.95);
                    break;

                default:
                    fsize = Convert.ToInt32(_height * 0.7);
                    break;
            }

            return new Font(fname, fsize);
        }

        /// <summary>
        /// Renders the CAPTCHA image.
        /// </summary>
        /// <returns></returns>
        private Bitmap GenerateImagePrivate()
        {
            var bmp = new Bitmap(_width, _height, PixelFormat.Format24bppRgb);

            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.Clear(BackColor);

                int charOffset = 0;
                double charWidth = _width / TextLength;

                foreach (char c in Text)
                {
                    // establish font and draw area
                    using (Font fnt = GetFont())
                    {
                        using (Brush fontBrush = new SolidBrush(GetRandomColor()))
                        {
                            var rectChar = new Rectangle(Convert.ToInt32(charOffset * charWidth), 0, Convert.ToInt32(charWidth), _height);

                            // warp the character
                            var gp = TextPath(c.ToString(), fnt, rectChar);
                            WarpText(gp, rectChar);

                            // draw the character
                            gr.FillPath(fontBrush, gp);

                            charOffset += 1;
                        }
                    }
                }

                var rect = new Rectangle(new Point(0, 0), bmp.Size);
                AddNoise(gr, rect);
                AddLine(gr, rect);
            }

            return bmp;
        }

        /// <summary>
        /// Warp the provided text GraphicsPath by a variable amount.
        /// </summary>
        /// <param name="textPath">The text path.</param>
        /// <param name="rect">The rect.</param>
        private void WarpText(GraphicsPath textPath, Rectangle rect)
        {
            float warpDivisor;
            float rangeModifier;

            switch (FontWarp)
            {
                case FontWarpFactor.None:
                    goto default;
                case FontWarpFactor.Low:
                    warpDivisor = 6F;
                    rangeModifier = 1F;
                    break;

                case FontWarpFactor.Medium:
                    warpDivisor = 5F;
                    rangeModifier = 1.3F;
                    break;

                case FontWarpFactor.High:
                    warpDivisor = 4.5F;
                    rangeModifier = 1.4F;
                    break;

                case FontWarpFactor.Extreme:
                    warpDivisor = 4F;
                    rangeModifier = 1.5F;
                    break;

                default:
                    return;
            }

            var rectF = new RectangleF(Convert.ToSingle(rect.Left), 0, Convert.ToSingle(rect.Width), rect.Height);

            int hrange = Convert.ToInt32(rect.Height / warpDivisor);
            int wrange = Convert.ToInt32(rect.Width / warpDivisor);
            int left = rect.Left - Convert.ToInt32(wrange * rangeModifier);
            int top = rect.Top - Convert.ToInt32(hrange * rangeModifier);
            int width = rect.Left + rect.Width + Convert.ToInt32(wrange * rangeModifier);
            int height = rect.Top + rect.Height + Convert.ToInt32(hrange * rangeModifier);

            if (left < 0)
            {
                left = 0;
            }

            if (top < 0)
            {
                top = 0;
            }

            if (width > Width)
            {
                width = Width;
            }

            if (height > Height)
            {
                height = Height;
            }

            var leftTop = RandomPoint(left, left + wrange, top, top + hrange);
            var rightTop = RandomPoint(width - wrange, width, top, top + hrange);
            var leftBottom = RandomPoint(left, left + wrange, height - hrange, height);
            var rightBottom = RandomPoint(width - wrange, width, height - hrange, height);

            var points = new PointF[] { leftTop, rightTop, leftBottom, rightBottom };
            var m = new Matrix();
            m.Translate(0, 0);
            textPath.Warp(points, rectF, m, WarpMode.Perspective, 0);
        }

        /// <summary>
        /// Add a variable level of graphic noise to the image.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="rect">The rect.</param>
        private void AddNoise(Graphics graphics, Rectangle rect)
        {
            int density = 0;
            int size = 0;

            switch (BackgroundNoise)
            {
                case BackgroundNoiseLevel.None:
                    break;

                case BackgroundNoiseLevel.Low:
                    density = 30;
                    size = 50;
                    break;

                case BackgroundNoiseLevel.Medium:
                    density = 18;
                    size = 40;
                    break;

                case BackgroundNoiseLevel.High:
                    density = 16;
                    size = 39;
                    break;

                case BackgroundNoiseLevel.Extreme:
                    density = 12;
                    size = 38;
                    break;

                default:
                    return;
            }

            using (var br = new SolidBrush(GetRandomColor()))
            {
                int max = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / size);

                for (var i = 0; i <= Convert.ToInt32((rect.Width * rect.Height) / density); i++)
                {
                    graphics.FillEllipse(br, _rand.Next(rect.Width), _rand.Next(rect.Height), _rand.Next(max), _rand.Next(max));
                }
            }
        }

        /// <summary>
        /// Add variable level of curved lines to the image.
        /// </summary>
        /// <param name="graphics">The graphics object.</param>
        /// <param name="rect">The rectangle.</param>
        private void AddLine(Graphics graphics, Rectangle rect)
        {
            int length = 0;
            float width = 0;
            int linecount = 0;

            switch (LineNoise)
            {
                case LineNoiseLevel.None:
                    break;

                case LineNoiseLevel.Low:
                    length = 4;
                    width = Convert.ToSingle(_height / 31.25);
                    linecount = 1;
                    break;

                case LineNoiseLevel.Medium:
                    length = 5;
                    width = Convert.ToSingle(_height / 27.7777);
                    linecount = 1;
                    break;

                case LineNoiseLevel.High:
                    length = 3;
                    width = Convert.ToSingle(_height / 25);
                    linecount = 2;
                    break;

                case LineNoiseLevel.Extreme:
                    length = 3;
                    width = Convert.ToSingle(_height / 22.7272);
                    linecount = 3;
                    break;

                default:
                    return;
            }

            var pf = new PointF[length + 1];
            using (var p = new Pen(GetRandomColor(), width))
            {
                for (var l = 1; l <= linecount; l++)
                {
                    for (var i = 0; i <= length; i++)
                    {
                        pf[i] = RandomPoint(rect);
                    }

                    graphics.DrawCurve(p, pf, 1.75F);
                }
            }
        }
    }
}