using Patterns.UndoRedo;

namespace AcensiPhotoShop
{
    /// <summary>
    /// Inverts the color in a picture with the formula NewColor = 255 - OldColor
    /// </summary>
    public class InvertPictureReversibleCommand : IReversibleCommand<IPicture>
    {
        public void Execute(IPicture picture)
        {
            picture.ForEachPixel((x, y) =>
            {
                picture[x, y] = new Color((byte)(255 - picture[x, y].R),
                    (byte)(255 - picture[x, y].G),
                    (byte)(255 - picture[x, y].B));
            });
        }

        public void Undo(IPicture picture)
        {
            picture.ForEachPixel((x, y) =>
            {
                picture[x, y] = new Color((byte)(255 - picture[x, y].R),
                    (byte)(255 - picture[x, y].G),
                    (byte)(255 - picture[x, y].B));
            });
        }
    }
}