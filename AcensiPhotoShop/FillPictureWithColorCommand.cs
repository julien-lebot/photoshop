using System;
using System.Collections.Generic;
using Patterns.UndoRedo;

namespace AcensiPhotoShop
{
    public class FillPictureWithColorCommand : IReversibleCommand<IPicture>
    {
        private readonly Color _oldColor;
        private readonly Color _newColor;
        private readonly int _x;
        private readonly int _y;

        public FillPictureWithColorCommand(Color oldColor, Color newColor, int x, int y)
        {
            _oldColor = oldColor;
            _newColor = newColor;
            _x = x;
            _y = y;
        }

        private void Paint(IPicture picture, Color oldColor, Color newColor)
        {
            var points = new Stack<Tuple<int, int>>();
            points.Push(new Tuple<int, int>(_x, _y));

            while (points.Count > 0)
            {
                var current = points.Peek();
                var x = current.Item1;
                var y = current.Item2;
                points.Pop();
                if (picture[x, y] == oldColor && picture[x, y] != newColor)
                {
                    picture[x, y] = newColor;
                    if (x > 0)
                    {
                        points.Push(new Tuple<int, int>(x - 1, y));
                    }
                    if (x < picture.Width - 1)
                    {
                        points.Push(new Tuple<int, int>(x + 1, y));
                    }
                    if (y > 0)
                    {
                        points.Push(new Tuple<int, int>(x, y - 1));
                    }
                    if (y < picture.Height - 1)
                    {
                        points.Push(new Tuple<int, int>(x, y + 1));
                    }
                }
            }
        }

        public void Execute(IPicture picture)
        {
            Paint(picture, _oldColor, _newColor);
        }

        public void Undo(IPicture picture)
        {
            Paint(picture, _newColor, _oldColor);
        }
    }
}
