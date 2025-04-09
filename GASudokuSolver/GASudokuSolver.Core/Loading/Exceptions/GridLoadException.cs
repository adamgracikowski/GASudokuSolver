namespace GASudokuSolver.Core.Loading.Exceptions;

public sealed class GridLoadException : Exception
{
	public GridLoadException() 
		: base() { }
	
	public GridLoadException(string? message) 
		: base(message) { }
	
	public GridLoadException(string? message, Exception? innerException) 
		: base(message, innerException) { }
}