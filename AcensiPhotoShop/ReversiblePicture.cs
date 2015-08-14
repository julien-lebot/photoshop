using Patterns.UndoRedo;

namespace AcensiPhotoShop
{
    /// <summary>
    /// A picture that supports Undo/Redo.
    /// </summary>
    public class ReversiblePicture : ReversibleDocument<IPicture>
    {
        public ReversiblePicture(IPicture picture)
            : base(picture)
        {
        }
    }
}