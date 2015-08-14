namespace AcensiPhotoShop
{
    public interface IPicture
    {
        Color this[int x, int y] { get; set; }

        int Height { get; }
        int Width { get; }

        void SetBuffer(Color[,] pixels);
    }
}