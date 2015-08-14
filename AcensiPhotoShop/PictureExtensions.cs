using System;

namespace AcensiPhotoShop
{
    public static class PictureExtensions
    {
        public static void ForEachPixel(this IPicture picture, Action<int, int> action)
        {
            for (int x = 0; x < picture.Width; ++x)
            {
                for (int y = 0; y < picture.Height; ++y)
                {
                    action(x, y);
                }
            }
        }
    }
}