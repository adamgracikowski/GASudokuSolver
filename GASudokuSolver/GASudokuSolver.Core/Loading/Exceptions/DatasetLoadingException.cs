namespace GASudokuSolver.Core.Loading.Exceptions;

public sealed class DatasetLoadingException : Exception
{
	public DatasetLoadingException()
		: base() { }

	public DatasetLoadingException(string? message)
		: base(message) { }

	public DatasetLoadingException(string? message, Exception? innerException)
		: base(message, innerException) { }
}