using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCII_Graphics
{
    class ConverterBitmapToASCII
    {
        private readonly Bitmap _bitmap;
        private readonly char[] _asciitable = { '.', ',', ':', '+','*','?','%','S','#','@' };
        public ConverterBitmapToASCII(Bitmap bitmap) => _bitmap = bitmap; 
        public char[][] Convert()
        {
            var result = new char[_bitmap.Height][];
            for (int y= 0; y < _bitmap.Height; y++) // iteration line by line
			{
                result[y] = new char[_bitmap.Width];

                for (int x = 0; x < _bitmap.Width; x++) // iteration by pixels
				{
                    int MapIndex = (int)Map(_bitmap.GetPixel(x, y).R, 0, 255, 0, _asciitable.Length - 1);
                    result[y][x] = _asciitable[MapIndex];
                }
            }
            return result;
        }
        private float Map(float valuetomap, float start1, float stop1, float start2, float stop2)
        {
            return ((valuetomap - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }
    }
}
