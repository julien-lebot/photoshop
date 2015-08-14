using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Patterns.UndoRedo
{
    /// <summary>
    /// Proxy class that can add support for Undo/Redo to *any* class.
    /// </summary>
    /// <typeparam name="TDocument">The type to add Undo/Redo support to.</typeparam>
    public class ReversibleDocument<TDocument> : INotifyPropertyChanged
    {
        private readonly List<IReversibleCommand<TDocument>> _pending = new List<IReversibleCommand<TDocument>>();
        private readonly Stack<IReversibleCommand<TDocument>> _history = new Stack<IReversibleCommand<TDocument>>();
        private bool _canRedo;

        /// <summary>
        /// Creates a new instance of ReversibleDocument that wraps the specified document.
        /// </summary>
        /// <param name="document">The document to wrap.</param>
        public ReversibleDocument(TDocument document)
        {
            Document = document;
        }

        /// <summary>
        /// Retrieves the document.
        /// </summary>
        public TDocument Document
        {
            get;
        }

        /// <summary>
        /// Returns whether there is one or more command(s) in the Redo history.
        /// </summary>
        public bool CanRedo => _canRedo && _pending.Count > 0;

        /// <summary>
        /// Returns whether there is one or more command(s) in the Undo history.
        /// </summary>
        public bool CanUndo => _history.Count > 0;

        /// <summary>
        /// Executes a command on this document.
        /// The command will be stored and not applied immediately, use Apply()
        /// to apply all pending commands.
        /// </summary>
        /// <param name="reversibleCommand">The command to apply.</param>
        /// <returns>This document, for chaining.</returns>
        public ReversibleDocument<TDocument> Do(IReversibleCommand<TDocument> reversibleCommand)
        {
            if (_canRedo)
            {
                _canRedo = false;
                _pending.Clear();
            }
            _pending.Add(reversibleCommand);
            OnPropertyChanged("CanRedo");
            OnPropertyChanged("CanUndo");
            return this;
        }

        /// <summary>
        /// Applies all pending commands. Does not throw if no commands
        /// are pending. After execution, all commands will be in the
        /// Undo history.
        /// </summary>
        public void Apply()
        {
            foreach (var cmd in _pending)
            {
                cmd.Execute(Document);
                _history.Push(cmd);
            }
            _pending.Clear();
            OnPropertyChanged("CanRedo");
            OnPropertyChanged("CanUndo");
        }

        /// <summary>
        /// Undo the last executed command. After execution, the command
        /// will be moved to the Redo history.
        /// <exception cref="InvalidOperationException">Thrown if the Undo history stack is empty.</exception> 
        /// </summary>
        public void Undo()
        {
            if (_history.Count == 0)
            {
                throw new InvalidOperationException("Command stack is empty");
            }
            var cmd = _history.Peek();
            cmd.Undo(Document);
            _pending.Add(cmd);
            _history.Pop();
            _canRedo = true;
            OnPropertyChanged("CanRedo");
            OnPropertyChanged("CanUndo");
        }

        /// <summary>
        /// Redo the last command that was undone. After execution, the command
        /// will be moved to the Undo history.
        /// <exception cref="InvalidOperationException">Thrown if the Redo history stack is empty.</exception> 
        /// </summary>
        public void Redo()
        {
            if (!_canRedo || _pending.Count == 0)
            {
                throw new InvalidOperationException("Command stack is empty");
            }
            var cmd = _pending.Last();
            cmd.Execute(Document);
            _history.Push(cmd);
            _pending.Remove(cmd);
            OnPropertyChanged("CanRedo");
            OnPropertyChanged("CanUndo");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}