﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace RecreateMeUtils
{
    public static class BitmapConverter
    {
        public static unsafe byte[] ByteTableFrom(Bitmap bitmap)
        {
            //Prepare variables
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            //Get data pointer
            IntPtr ptr = bmpData.Scan0;
            int bytesNumber = Math.Abs(bmpData.Stride) * bitmap.Height;
            byte[] rgbValues = new byte[bytesNumber];

            //Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytesNumber);
            bitmap.UnlockBits(bmpData);
            return rgbValues;
        }
    }
}

