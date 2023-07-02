using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace ASCII_Graphics
{
    
    class Program
    {
        const int SWP_NOZORDER = 0x4;
        const int SWP_NOACTIVATE = 0x10;
        [DllImport("kernel32")] static extern IntPtr GetConsoleWindow();
        [DllImport("user32")] static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int flags);
        public static void SetWindowPosition(int x, int y, int width, int height)
        {
            SetWindowPos(Handle, IntPtr.Zero, x, y, width, height, SWP_NOZORDER | SWP_NOACTIVATE);
        }
        public static IntPtr Handle => GetConsoleWindow();
        //
        private const double WIDTH_OFFSET_VERTICAL = 1.7;
        private const double WIDTH_OFFSET_SQUARE = 2;
        private const double WIDTH_OFFSET_NORMAL = 1.9;
        //
        private const int MAX_WIDTH_SQUARE = 350;
        private const int MAX_WIDTH_VERTICAL = 290;
        private const int MAX_WIDTH_NORMAL = 625;
        //
        [STAThread]
        static void Main(string[] args)
        {
			Console.WriteLine("Maximize the window, select font Consolas bold size 5 and than press 'Enter' to open the file dialog");

			Console.WindowWidth = Console.LargestWindowWidth;
            Console.WindowHeight = Console.LargestWindowHeight;
            Console.BufferWidth =  Console.LargestWindowWidth;
            Console.BufferHeight = Console.LargestWindowHeight;

            var width = Console.LargestWindowWidth;
            var height = Console.LargestWindowHeight;

            SetWindowPosition(0, 0, 1920, 1020);
            Console.Title = "ASCII ART";            //
            //
            //
            var OpenFileDialog = new OpenFileDialog
            {
                Filter = "Images | *.jpg; *.jpeg; *.png; *.bmp"
            };
            
            while (true)
            {
                Console.ReadLine();
                
                if(OpenFileDialog.ShowDialog() != DialogResult.OK)
                    continue;

                Console.Clear();

                var bitmap = new Bitmap(OpenFileDialog.FileName);
                bitmap = ResizeBitmap(bitmap);
                bitmap.ToGrayScale();

                var converter = new ConverterBitmapToASCII(bitmap);
                var rows = converter.Convert();
                
                foreach (var row in rows)
                {
                    Console.WriteLine(row);
                }

                File.WriteAllLines("image.txt", rows.Select(r => new string(r)));

                Console.SetCursorPosition(0, 0);
            }
        }

        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            if (bitmap.Width == bitmap.Height)
            {
                var maxHeight = (bitmap.Height / WIDTH_OFFSET_SQUARE * MAX_WIDTH_SQUARE / bitmap.Width);
                if (bitmap.Width > MAX_WIDTH_SQUARE || bitmap.Height > maxHeight) //if selected picture bigger than value in our variable, we resize it
                    bitmap = new Bitmap(bitmap, new Size(MAX_WIDTH_SQUARE, (int)maxHeight));
            }
            //
            //
            else if (bitmap.Width < bitmap.Height)
            {
                var maxHeight = bitmap.Height / WIDTH_OFFSET_VERTICAL * MAX_WIDTH_VERTICAL / bitmap.Width;
                if (bitmap.Width > MAX_WIDTH_VERTICAL || bitmap.Height > maxHeight) //if selected picture bigger than value in our variable, we resize it
                    bitmap = new Bitmap(bitmap, new Size(MAX_WIDTH_VERTICAL, (int)maxHeight));
            }
            //
            //
            else if (bitmap.Width > bitmap.Height)
            {
                var maxHeight = bitmap.Height / (WIDTH_OFFSET_NORMAL + 0.5) * MAX_WIDTH_NORMAL / bitmap.Width;
                if (bitmap.Width > MAX_WIDTH_NORMAL || bitmap.Height > maxHeight) //if selected picture bigger than value in our variable, we resize it
                    bitmap = new Bitmap(bitmap, new Size(MAX_WIDTH_NORMAL, (int)maxHeight));
            }
            return bitmap;
        }
    }
}
