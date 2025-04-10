namespace GASudokuSolver.Core.Loading.Exceptions;

public sealed class GridLoadingException : Exception
{
	public GridLoadingException() 
		: base() { }
	
	public GridLoadingException(string? message) 
		: base(message) { }
	
	public GridLoadingException(string? message, Exception? innerException) 
		: base(message, innerException) { }
}