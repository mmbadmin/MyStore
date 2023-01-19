namespace MyStore.Common.Utilities
{
    using ImageMagick;
    using System;
    using System.IO;

    public class Captcha
    {
        public int Value { get; set; }

        public byte[] Image { get; set; } = new byte[] { };

        public static Captcha New()
        {
            using (var stream = new MemoryStream())
            using (var image = new MagickImage(MagickColors.AliceBlue, 390, 92))
            {
                var captcha = new Captcha();
                var random = new Random((int)DateTime.Now.Ticks);
                var number = random.Next(1000, 9999);
                var numbers = number.ToString().ToCharArray();
                var y = 62;
                var x = 70;

                var drawables = new Drawables();
                foreach (var num in numbers)
                {
                    var fillColor = new MagickColor((byte)random.Next(0, 100), (byte)random.Next(0, 100), (byte)random.Next(0, 100));
                    var strokeColor = new MagickColor((byte)random.Next(0, 100), (byte)random.Next(0, 100), (byte)random.Next(0, 100));
                    drawables = drawables.FontPointSize(39)
                                        .Font("Arial")
                                        .FillColor(fillColor)
                                        .StrokeColor(strokeColor)
                                        .Text(x, y, num.ToString());
                    x += 80;
                }
                for (var i = 0; i < 4; i++)
                {
                    var fillColor = new MagickColor((byte)random.Next(0, 100), (byte)random.Next(0, 100), (byte)random.Next(0, 100));
                    var strokeColor = new MagickColor((byte)random.Next(0, 100), (byte)random.Next(0, 100), (byte)random.Next(0, 100));
                    drawables = drawables.FillColor(fillColor)
                                            .StrokeColor(strokeColor)
                                            .Line(0, random.Next(0, image.Height), image.Width, random.Next(0, image.Height));
                }
                for (var i = 0; i < 10; i++)
                {
                    var fillColor = MagickColors.Transparent;
                    var strokeColor = new MagickColor((byte)random.Next(0, 100), (byte)random.Next(0, 100), (byte)random.Next(0, 100));
                    var radius = random.Next(2, 10);
                    drawables = drawables.FillColor(fillColor)
                                            .StrokeColor(strokeColor)
                                            .Ellipse(random.Next(radius, image.Width - radius),
                                                     random.Next(radius, image.Height - radius),
                                                     radius,
                                                     radius,
                                                     0,
                                                     360);
                }
                drawables.Draw(image);
                image.Format = MagickFormat.Png;
                image.Write(stream, MagickFormat.Png);
                captcha.Image = stream.ToArray();
                captcha.Value = number;
                return captcha;
            }
        }
    }
}
