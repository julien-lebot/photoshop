namespace Patterns.UndoRedo
{
    /// <summary>
    /// Defines some methods that every commands must implement
    /// to support the Undo/Redo pattern.
    /// </summary>
    public interface IReversibleCommand<TDocument>
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        void Execute(TDocument document);

        /// <summary>
        /// Undo the command.
        /// </summary>
        void Undo(TDocument document);
    }
}