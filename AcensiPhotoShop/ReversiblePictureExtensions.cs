using Patterns.UndoRedo;

namespace AcensiPhotoShop
{
    /// <summary>
    /// Convenience class to add commands to a reversible picture.
    /// </summary>
    public static class ReversiblePictureExtensions
    {
        /// <summary>
        /// Rotates the picture by angleDegrees degrees.
        /// </summary>
        /// <param name="doc">The document to apply this transformation to.</param>
        /// <param name="angleDegrees">The amount to rotate, in degrees.</param>
        /// <returns></returns>
        public static ReversibleDocument<IPicture> Rotate(this ReversibleDocument<IPicture> doc, double angleDegrees)
        {
            return doc.Do(new RotatePictureReversibleCommand(angleDegrees));
        }

        /// <summary>
        /// Inverts all pixels in the picture.
        /// </summary>
        /// <param name="doc">The document to apply this transformation to.</param>
        /// <returns></returns>
        public static ReversibleDocument<IPicture> Invert(this ReversibleDocument<IPicture> doc)
        {
            return doc.Do(new InvertPictureReversibleCommand());
        }

        public static ReversibleDocument<IPicture> Fill(this ReversibleDocument<IPicture> doc, int x, int y, Color oldColor, Color newColor)
        {
            return doc.Do(new FillPictureWithColorCommand(oldColor, newColor, x, y));
        } 

    }
}