using System;
using Patterns.UndoRedo;

namespace AcensiPhotoShop
{
    /// <summary>
    /// Rotates a picture counter-clockwise.
    /// To rotate clockwise, invert the angle of rotation.
    /// </summary>
    public class RotatePictureReversibleCommand : IReversibleCommand<IPicture>
    {
        private readonly double _angleDegrees;

        public RotatePictureReversibleCommand(double angleDegrees)
        {
            _angleDegrees = angleDegrees;
        }

        public void Execute(IPicture picture)
        {
            Rotate(picture, _angleDegrees);
        }

        private void Rotate(IPicture picture, double angleDegrees)
        {
            var angleRadians = angleDegrees * Math.PI / 180d;

            var cos = Math.Cos(angleRadians);
            var sin = Math.Sin(angleRadians);
            var newWidth = (int)Math.Abs(Math.Round(picture.Width * cos + picture.Height * sin));
            var newHeight = (int)Math.Abs(Math.Round(picture.Height * cos + picture.Width * sin));

            var pixels = new Color[newWidth, newHeight];

            picture.ForEachPixel((x, y) =>
            {
                var cx = x - ((picture.Width - 1) / 2f);
                var cy = y - ((picture.Height - 1) / 2f);
                var nx = (cx * cos - cy * sin);
                var ny = (cy * cos + cx * sin);

                var xp = (int)Math.Round(nx + ((newWidth - 1) / 2f));
                var yp = (int)Math.Round(ny + ((newHeight - 1) / 2f));
                pixels[xp, yp] = picture[x, y];
            });

            picture.SetBuffer(pixels);
        }

        public void Undo(IPicture picture)
        {
            Rotate(picture, - _angleDegrees);
        }
    }
}