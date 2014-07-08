namespace LicenseeAgent.Core.Services
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Text;
    using LicenseeAgent.Core.Configurations;

    public enum BackgroundNoiseLevel
    {
        /// <summary>
        /// No background noise.
        /// </summary>
        None,

        /// <summary>
        /// Low background noise.
        /// </summary>
        Low,

        /// <summary>
        /// Medium background noise.
        /// </summary>
        Medium,

        /// <summary>
        /// High background noise.
        /// </summary>
        High,

        /// <summary>
        /// Extreme background noise.
        /// </summary>
        Extreme
    }

    public enum FontWarpFactor
    {
        /// <summary>
        /// No font wrap.
        /// </summary>
        None,

        /// <summary>
        /// Low font wrap.
        /// </summary>
        Low,

        /// <summary>
        /// Medium font wrap.
        /// </summary>
        Medium,

        /// <summary>
        /// High font wrap.
        /// </summary>
        High,

        /// <summary>
        /// Extreme font wrap.
        /// </summary>
        Extreme
    }

    public enum LineNoiseLevel
    {
        /// <summary>
        /// No line noise.
        /// </summary>
        None,

        /// <summary>
        /// Low line noise.
        /// </summary>
        Low,

        /// <summary>
        /// Medium line noise.
        /// </summary>
        Medium,

        /// <summary>
        /// High line noise.
        /// </summary>
        High,

        /// <summary>
        /// Extreme line noise.
        /// </summary>
        Extreme
    }

    /// <summary>
    /// CAPTCHA Image.
    /// </summary>
    /// <seealso href="http://www.codinghorror.com">Original By Jeff Atwood.</seealso>
    public class CaptchaImage
    {
        public const string TextChars = "acdefghjknpqruvxyzACDEFGHJKLNPQRTUVXYZ2346789";

        public const int MinWidth = 60;

        public const int MinHeight = 30;

        public const int TextLength = 6;

        public static readonly string[] FontFamilies =
        {
            "arial", "arial black", "comic sans ms", "courier new",
            "estrangelo edessa", "franklin gothic medium", "georgia", "lucida console", "lucida sans unicode", "mangal",
            "microsoft sans serif", "palatino linotype", "sylfaen", "tahoma", "times new roman", "trebuchet ms",
            "verdana"
        };

        private static Random random = new Random();

        private int _height;
        private int _width;

        public CaptchaImage(int width, int height)
            : this()
        {
            Width = width;
            Height = height;
        }

        public CaptchaImage()
        {
            Width = Constants.CaptchaWidth;
            Height = Constants.CaptchaHeight;
            BackColor = Constants.CaptchaBackColor;
            FontWarp = FontWarpFactor.None;
            BackgroundNoise = BackgroundNoiseLevel.Low;
            LineNoise = LineNoiseLevel.None;
        }

        public Color BackColor { get; set; }

        public FontWarpFactor FontWarp { get; set; }

        public BackgroundNoiseLevel BackgroundNoise { get; set; }

        public LineNoiseLevel LineNoise { get; set; }

        public string Text { get; private set; }

        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                if (value <= MinWidth)
                {
                    throw new ArgumentOutOfRangeException("width", value, "width must be greater than 60.");
                }

                _width = value;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }

            set
            {
                if (value <= MinHeight)
                {
                    throw new ArgumentOutOfRangeException("height", value, "height must be greater than 30.");
                }

                _height = value;
            }
        }

        /// <summary>
        /// Forces a new Captcha image to be generated using current property value settings.
        /// </summary>
        /// <returns>Returns captcha image.</returns>
        public Bitmap RenderImage()
        {
            return GenerateImagePrivate();
        }

        /// <summary>
        /// Generate random text for the CAPTCHA.
        /// </summary>
        public void RandomText()
        {
            var stringBuilder = new StringBuilder(TextLength);
            int maxLength = TextChars.Length;
            for (int n = 0; n <= TextLength - 1; n++)
            {
                stringBuilder.Append(TextChars.Substring(random.Next(maxLength), 1));
            }

            Text = stringBuilder.ToString();
        }

        /// <summary>
        /// Returns a GraphicsPath containing the specified string and font.
        /// </summary>
        /// <param name="text">The captcha text.</param>
        /// <param name="font">The captcha font.</param>
        /// <param name="rectangle">The captcha rectangle.</param>
        /// <returns>
        /// Returns graphics path.
        /// </returns>
        private static GraphicsPath TextPath(string text, Font font, Rectangle rectangle)
        {
            var stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near
            };

            var graphicsPath = new GraphicsPath();
            graphicsPath.AddString(text, font.FontFamily, (int)font.Style, font.Size, rectangle, stringFormat);
            return graphicsPath;
        }

        /// <summary>
        /// Random the color.
        /// </summary>
        /// <returns>Random color.</returns>
        private static Color GetRandomColor()
        {
            return Constants.CaptchaForeColor;
        }

        /// <summary>
        /// Returns a random font family from the font whitelist.
        /// </summary>
        /// <returns>Returns random font family.</returns>
        private string GetRandomFontFamily()
        {
            return FontFamilies[random.Next(0, FontFamilies.Length)];
        }

        /// <summary>
        /// Returns a random point within the specified x and y ranges.
        /// </summary>
        /// <param name="minX">The minimum x.</param>
        /// <param name="maxX">The maximum x.</param>
        /// <param name="minY">The minimum y.</param>
        /// <param name="maxY">The maximum y.</param>
        /// <returns>
        /// Random point.
        /// </returns>
        private PointF RandomPoint(int minX, int maxX, int minY, int maxY)
        {
            return new PointF(random.Next(minX, maxX), random.Next(minY, maxY));
        }

        /// <summary>
        /// Returns a random point within the specified rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle.</param>
        /// <returns>Random point.</returns>
        private PointF RandomPoint(Rectangle rectangle)
        {
            return RandomPoint(rectangle.Left, rectangle.Width, rectangle.Top, rectangle.Bottom);
        }

        /// <summary>
        /// Returns the CAPTCHA font in an appropriate size.
        /// </summary>
        /// <returns>Returns font object.</returns>
        private Font GetFont()
        {
            float fontSize;
            string fontName = GetRandomFontFamily();

            switch (FontWarp)
            {
                case FontWarpFactor.None:
                    goto default;

                case FontWarpFactor.Low:
                    fontSize = Convert.ToInt32(_height * 0.8);
                    break;

                case FontWarpFactor.Medium:
                    fontSize = Convert.ToInt32(_height * 0.85);
                    break;

                case FontWarpFactor.High:
                    fontSize = Convert.ToInt32(_height * 0.9);
                    break;

                case FontWarpFactor.Extreme:
                    fontSize = Convert.ToInt32(_height * 0.95);
                    break;

                default:
                    fontSize = Convert.ToInt32(_height * 0.7);
                    break;
            }

            return new Font(fontName, fontSize);
        }

        /// <summary>
        /// Renders the CAPTCHA image.
        /// </summary>
        /// <returns>Returns captcha image.</returns>
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
        /// <param name="rectangle">The rectangle.</param>
        private void WarpText(GraphicsPath textPath, Rectangle rectangle)
        {
            float warpDivisor, rangeModifier;
            switch (FontWarp)
            {
                case FontWarpFactor.None:
                    return;

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

            var rectF = new RectangleF(Convert.ToSingle(rectangle.Left), 0, Convert.ToSingle(rectangle.Width), rectangle.Height);

            int hrange = Convert.ToInt32(rectangle.Height / warpDivisor);
            int wrange = Convert.ToInt32(rectangle.Width / warpDivisor);
            int left = Math.Max(rectangle.Left - Convert.ToInt32(wrange * rangeModifier), 0);
            int top = Math.Max(rectangle.Top - Convert.ToInt32(hrange * rangeModifier), 0);
            int width = Math.Min(rectangle.Left + rectangle.Width + Convert.ToInt32(wrange * rangeModifier), Width);
            int height = Math.Min(rectangle.Top + rectangle.Height + Convert.ToInt32(hrange * rangeModifier), Width);

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
        /// <param name="graphics">The graphics.</param>
        /// <param name="rectangle">The rectangle.</param>
        private void AddNoise(Graphics graphics, Rectangle rectangle)
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
                int max = Convert.ToInt32(Math.Max(rectangle.Width, rectangle.Height) / size);

                for (var i = 0; i <= Convert.ToInt32((rectangle.Width * rectangle.Height) / density); i++)
                {
                    graphics.FillEllipse(br, random.Next(rectangle.Width), random.Next(rectangle.Height), random.Next(max), random.Next(max));
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
            int length = 0, linecount = 0;
            float width = 0;
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