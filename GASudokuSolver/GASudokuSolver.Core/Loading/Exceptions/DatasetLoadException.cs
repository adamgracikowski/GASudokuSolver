namespace GASudokuSolver.Core.Loading.Exceptions;

public sealed class DatasetLoadException : Exception
{
	public DatasetLoadException()
		: base() { }

	public DatasetLoadException(string? message)
		: base(message) { }

	public DatasetLoadException(string? message, Exception? innerException)
		: base(message, innerException) { }
}