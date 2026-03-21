namespace SmartStore.Core.Interfaces;

// -------------------------------------------------------
// COMMAND PATTERN — interface
// -------------------------------------------------------
// Encapsulates a request as an object, supporting Execute and Undo.
// -------------------------------------------------------
public interface ICommand
{
    string Description { get; }
    void Execute();
    void Undo();
}
