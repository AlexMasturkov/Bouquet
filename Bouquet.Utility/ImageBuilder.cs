using System;
using System.Drawing;



namespace Bouquet.Utility
{
    public static class ImageBuilder
    {
        public static Image ResizeImage(Image image)
        {
            float ratio = 72 / 64f;
            float proportion = 0.0f;

            //possible test code
            int w = image.Width;
            int h = image.Height;
            proportion = (float)h / w;

            //resize block start
            Size newSize = new Size();

            if (proportion < ratio)
            {
                newSize.Height = 720;
                newSize.Width = (Int32)(720 / proportion);
            }
            if (proportion > ratio)
            {
                newSize.Width = 640;
                newSize.Height = (Int32)(640 * proportion);
            }
            Image newImage = new Bitmap(newSize.Width, newSize.Height);
            using (Graphics GFX = Graphics.FromImage((Bitmap)newImage))
            {
                GFX.DrawImage(image, new Rectangle(Point.Empty, newSize));
            }
            //resize block end

            Size tempSize = new Size();
            int shiftLeft = 0;
            int shiftTop = 0;
            h = newImage.Height;
            w = newImage.Width;

            if (proportion < ratio)//the case to trim width
            {
                tempSize.Height = h;
                tempSize.Width = (int)(h / ratio);
                shiftLeft = -(w - (int)(tempSize.Width)) / 2;

            }
            else //the case to trim height
            {
                tempSize.Width = w;
                tempSize.Height = (int)(w * ratio);
                shiftTop = -(h - tempSize.Height) / 2;
            }
            Image tempImage = new Bitmap(tempSize.Width, tempSize.Height);
            using (Graphics GFX = Graphics.FromImage((Bitmap)tempImage))
            {
                var point = new Point();
                point.X = shiftLeft;
                point.Y = shiftTop;
                GFX.DrawImage(newImage, point);

            }
            var result = (Image)tempImage.Clone();
            image.Dispose();
            newImage.Dispose();
            tempImage.Dispose();
            return result;
        }
    }
}
