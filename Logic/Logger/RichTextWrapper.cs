
using System;

namespace Logic.Log
{
    public class RichTextWrapper
    {
        public byte[] Color { get; set; }

        public string Content { get; set; }

        public int FontSize { get; set; }

        public bool IsBold { get; set; }

        public RichTextWrapper(string content, byte[] color = null, int fontSize = 12, bool isBold = false)
        {
            Content = content;
            Color = color == null ? GetColor(ColorEnum.Black) : color;
            FontSize = fontSize;
            IsBold = IsBold;
        }

        public static byte[] GetColor(ColorEnum color)
        {
            switch (color)
            {
                case ColorEnum.Red:
                    return new byte[] { 234, 67, 53 };
                    break;
                case ColorEnum.Green:
                    return new byte[] { 64, 121, 8 };

                case ColorEnum.Blue:
                    return new byte[] { 8, 21, 121 };
                case ColorEnum.Black:
                    return new byte[] { 3, 3, 3 };
                case ColorEnum.White:
                    return new byte[] { 252, 252, 252 };

                case ColorEnum.Yellow:
                    return new byte[] { 245, 242, 15 };
                default:
                    return new byte[] { 3, 3, 3 };
            }

        }
    }

    public enum ColorEnum
    {
        Red,
        Green,
        Black,
        Blue,
        White,
        Yellow
    }
}
