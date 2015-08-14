using System;
using AcensiPhotoShop;
using Patterns.UndoRedo;

namespace PhotoShop
{
    public enum Tools
    {
        None,
        Fill,
        Draw,
        Pan
    }

    public class MainWindowViewModel : NotifyPropertyChanged
    {
        public IAccessPolicy AccessPolicy
        {
            get;
            set;
        }

        private Tools _activeTool = Tools.None;
        public Tools ActiveTool
        {
            get
            {
                return _activeTool;
            }
            set
            {
                _activeTool = value;
                OnPropertyChanged();
                OnPropertyChanged("PanEnabled");
            }
        }

        private ReversibleDrawCommand _drawCmd;

        private ReversibleDocument<IPicture> _document;
        public ReversibleDocument<IPicture> Document
        {
            get
            {
                return _document;
            }
            set
            {
                if (_document != null)
                {
                    Undo.StopMonitoring(_document);
                    Redo.StopMonitoring(_document);
                }
                _document = value;
                OnPropertyChanged();
                Undo.For(Document).Monitor(() => Document.CanUndo);
                Redo.For(Document).Monitor(() => Document.CanRedo);
            }
        }

        private void Execute(Action action)
        {
            AccessPolicy?.BeforeAccess();
            action();
            AccessPolicy?.AfterAccess();
        }

        public bool PanEnabled => ActiveTool == Tools.Pan;

        private RelayCommand _selectPanTool;

        public RelayCommand SelectPanTool
        {
            get
            {
                if (_selectPanTool != null)
                {
                    return _selectPanTool;
                }
                _selectPanTool = new RelayCommand(_ => ActiveTool != Tools.Pan, _ => ActiveTool = Tools.Pan);
                _selectPanTool.For(this).Monitor(() => ActiveTool);
                return _selectPanTool;
            }
        }

        private RelayCommand _selectFillTool;

        public RelayCommand SelectFillTool
        {
            get
            {
                if (_selectFillTool != null)
                {
                    return _selectFillTool;
                }
                _selectFillTool = new RelayCommand(_ => ActiveTool != Tools.Fill, _ => ActiveTool = Tools.Fill);
                _selectFillTool.For(this).Monitor(() => ActiveTool);
                return _selectFillTool;
            }
        }

        private RelayCommand _selectDrawTool;

        public RelayCommand SelectDrawTool
        {
            get
            {
                if (_selectDrawTool != null)
                {
                    return _selectDrawTool;
                }
                _selectDrawTool = new RelayCommand(_ => ActiveTool != Tools.Draw, _ => ActiveTool = Tools.Draw);
                _selectDrawTool.For(this).Monitor(() => ActiveTool);
                return _selectDrawTool;
            }
        }

        private RelayCommand _undo;

        public RelayCommand Undo
        {
            get
            {
                if (_undo != null)
                {
                    return _undo;
                }
                _undo = new RelayCommand(_ => Document != null && Document.CanUndo, _ => Execute(() => Document.Undo()));
                _undo.For(this).Monitor(() => Document);
                return _undo;
            }
        }

        private RelayCommand _redo;

        public RelayCommand Redo
        {
            get
            {
                if (_redo != null)
                {
                    return _redo;
                }
                _redo = new RelayCommand(_ => Document != null && Document.CanRedo, _ => Execute(() => Document.Redo()));
                _redo.For(this).Monitor(() => Document);
                return _redo;
            }
        }

        private RelayCommand _invert;

        public RelayCommand Invert
        {
            get
            {
                if (_invert != null)
                {
                    return _invert;
                }
                _invert = new RelayCommand(_ => Document != null, _ => Execute(() => Document.Invert().Apply()));
                _invert.For(this).Monitor(() => Document);
                return _invert;
            }
        }

        private RelayCommand _mouseLeftButtonDown;

        public RelayCommand MouseLeftButtonDown
        {
            get
            {
                if (_mouseLeftButtonDown != null)
                {
                    return _mouseLeftButtonDown;
                }
                _mouseLeftButtonDown = new RelayCommand(_ => ActiveTool == Tools.Fill || ActiveTool == Tools.Draw, t =>
                {
                    var pos = t as Tuple<int, int>;
                    var x = pos.Item1;
                    var y = pos.Item2;

                    if (ActiveTool == Tools.Fill)
                    {
                        Execute(() => Document.Fill(x, y, Document.Document[x, y], PrimaryColor).Apply());
                    }
                    else if (ActiveTool == Tools.Draw)
                    {
                        _drawCmd = new ReversibleDrawCommand(PrimaryColor);
                    }
                });
                _mouseLeftButtonDown.For(this).Monitor(() => ActiveTool);
                return _mouseLeftButtonDown;
            }
        }

        private RelayCommand _mouseLeftButtonUp;

        public RelayCommand MouseLeftButtonUp
        {
            get
            {
                if (_mouseLeftButtonUp != null)
                {
                    return _mouseLeftButtonUp;
                }
                _mouseLeftButtonUp = new RelayCommand(_ => ActiveTool == Tools.Draw, t =>
                {
                    Document.Do(_drawCmd).Apply();
                    _drawCmd = null;
                });
                _mouseLeftButtonUp.For(this).Monitor(() => ActiveTool);
                return _mouseLeftButtonUp;
            }
        }

        private RelayCommand _mouseMove;

        public RelayCommand MouseMove
        {
            get
            {
                if (_mouseMove != null)
                {
                    return _mouseMove;
                }
                _mouseMove = new RelayCommand(_ => ActiveTool == Tools.Draw && _drawCmd != null, t =>
                {
                    var pos = t as Tuple<int, int>;
                    Execute(() => _drawCmd.AddPoint(Document.Document, pos.Item1, pos.Item2));
                });
                _mouseMove.For(this).Monitor(() => ActiveTool);
                return _mouseMove;
            }
        }

        private Color _primaryColor;

        public Color PrimaryColor
        {
            get
            {
                return _primaryColor;
            }
            set
            {
                _primaryColor = value;
                OnPropertyChanged();
            }
        }
    }
}