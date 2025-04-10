namespace GASudokuSolver.Core.Loading.Exceptions;

public sealed class SudokuLoadingException : Exception
{
	public SudokuLoadingException()
		: base() { }

	public SudokuLoadingException(string? message)
		: base(message) { }

	public SudokuLoadingException(string? message, Exception? innerException)
		: base(message, innerException) { }
}