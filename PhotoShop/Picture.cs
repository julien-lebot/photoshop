using System.Windows;
using System.Windows.Media.Imaging;
using AcensiPhotoShop;

namespace PhotoShop
{
    public class Picture : IPicture
    {
        private WriteableBitmap _bmp;
        private bool _locked;

        public WriteableBitmap Bitmap => _bmp;

        public string Name
        {
            get;
        }

        public Picture(string name, WriteableBitmap bmp)
        {
            _bmp = bmp;
            Name = name;
        }

        public Color this[int x,int y]
        {
            get
            {
                unsafe
                {
                    int pBackBuffer = (int)_bmp.BackBuffer;

                    // Find the address of the pixel to draw.
                    pBackBuffer += y * _bmp.BackBufferStride;
                    pBackBuffer += x * 4;

                    return new Color(
                        (byte)(*((int*) pBackBuffer) >> 16),
                        (byte)(*((int*) pBackBuffer) >> 8 & 0xFF),
                        (byte)(*((int*) pBackBuffer) & 0xFF));
                }
            }
            set
            {
                byte[] colorData = { value.B, value.G, value.R, 1};
                _bmp.WritePixels(new Int32Rect(x, y, 1, 1), colorData, 4, 0);
            }
        }

        public int Height => _bmp.PixelHeight;

        public int Width => _bmp.PixelWidth;

        public void Lock()
        {
            if (_locked)
            {
                return;
            }
            _bmp.Lock();
            _locked = true;
        }

        public void Unlock()
        {
            if (!_locked)
            {
                return;
            }
            _bmp.Unlock();
            _locked = false;
        }

        public void SetBuffer(Color[,] pixels)
        {
        }
    }
}