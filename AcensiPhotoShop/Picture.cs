namespace AcensiPhotoShop
{
    /// <summary>
    /// A picture is a collection of "color".
    /// </summary>
    public class Picture : IPicture
    {
        private Color[,] _pixels;

        public Picture(Color defaultColor, int width, int height)
        {
            _pixels = new Color[width, height];
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    _pixels[x, y] = defaultColor;
                }
            }
        }

        public Picture(Color defaultColor)
        : this(defaultColor, 1, 1)
        {

        }

        public Picture()
        : this(Color.White, 1, 1)
        { }

        public int Width => _pixels.GetLength(0);

        public int Height => _pixels.GetLength(1);

        public Color this[int x, int y]
        {
            get
            {
                return _pixels[x, y];
            }
            set
            {
                _pixels[x, y] = value;
            }
        }

        public void SetBuffer(Color[,] pixels)
        {
            _pixels = pixels;
        }
    }
}