using System;
using System.Collections.Generic;
using System.Text;

namespace Nonograms
{
    public class ColorRGB
    {
        public int red = -1, green = -1, blue = -1;

        public ColorRGB()
        {
            this.red = -1;
            this.green = -1;
            this.blue = -1;
        }

        
        public ColorRGB(int red, int green, int blue)
        {
            if (!IsCorrectColor(red) || !IsCorrectColor(green) || !IsCorrectColor(blue))
            {
                throw new ArgumentException("Color is wrong");
            }
            this.red = red;
            this.green = green;
            this.blue = blue;
        }
        public bool isBright()
        {
            if (IsNone())
            {
                throw new Exception("Color is None");
            }
            return 1 - (0.299 * red + 0.587 * green + 0.114 * blue) / 255 < 0.5;
        }
        public override bool Equals(object obj)
        {
            return obj is ColorRGB rGB &&
                   red == rGB.red &&
                   green == rGB.green &&
                   blue == rGB.blue;
        }

        public static bool operator ==(ColorRGB color1, ColorRGB color2)
        {
            return color1.Equals(color2);
        }
        public static bool operator !=(ColorRGB color1, ColorRGB color2)
        {
            return !(color1 == color2);
        }
        public override int GetHashCode()
        {
            int hashCode = 1602832517;
            hashCode = hashCode * -1521134295 + red.GetHashCode();
            hashCode = hashCode * -1521134295 + green.GetHashCode();
            hashCode = hashCode * -1521134295 + blue.GetHashCode();
            return hashCode;
        }

        public int Red { get => red; }
        public int Green { get => green; }
        public int Blue {  get => blue; }
        public bool IsNone() => red == -1 || green == -1 || blue == -1;

        private bool IsCorrectColor(int color) => color >= 0 && color < 256;

        public override string ToString()
        {
            return Red.ToString() + " " + Green.ToString() + " " + Blue.ToString();
        }
    }
}
