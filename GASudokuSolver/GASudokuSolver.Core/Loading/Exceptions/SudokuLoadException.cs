namespace GASudokuSolver.Core.Loading.Exceptions;

public sealed class SudokuLoadException : Exception
{
	public SudokuLoadException()
		: base() { }

	public SudokuLoadException(string? message)
		: base(message) { }

	public SudokuLoadException(string? message, Exception? innerException)
		: base(message, innerException) { }
}