using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Drawing;
using System;

namespace JA_Sepia.Extensions
{
    internal class BitmapExtension
    {
        private static BitmapData bitmapData;
        private static Bitmap bitmap;
        public static IntPtr LoadBitmap(string path)
        {
            bitmap = new Bitmap(path);
            bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr ptr = bitmapData.Scan0;
            return ptr;
        }

        public static void SaveBitmap(string path)
        {
            bitmap.UnlockBits(bitmapData);
            bitmap.Save(path);
        }

        public static Bitmap GetBitmap()
        {
            return bitmap;
        }
    }
}
