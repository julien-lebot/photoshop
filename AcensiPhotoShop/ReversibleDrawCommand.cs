using System;
using System.Collections.Generic;
using System.Linq;
using Patterns.UndoRedo;

namespace AcensiPhotoShop
{
    public class ReversibleDrawCommand : IReversibleCommand<IPicture>
    {
        private readonly Color _newColor;
        private readonly List<Tuple<int, int, Color>> _points = new List<Tuple<int, int, Color>>();

        public ReversibleDrawCommand(Color newColor)
        {
            _newColor = newColor;
        }

        public void Execute(IPicture picture)
        {
            foreach (var point in _points)
            {
                picture[point.Item1, point.Item2] = _newColor;
            }
        }

        public void AddPoint(IPicture picture, int x, int y)
        {
            if (_points.Any(t => t.Item1 == x && t.Item2 == y) ||
                x < 0 ||
                y < 0 ||
                x >= picture.Width ||
                y >= picture.Height)
            {
                return;
            }
            _points.Add(new Tuple<int, int, Color>(x, y, picture[x, y]));
            picture[x, y] = _newColor;
        }

        public void Undo(IPicture picture)
        {
            foreach (var point in _points)
            {
                picture[point.Item1, point.Item2] = point.Item3;
            }
        }
    }
}