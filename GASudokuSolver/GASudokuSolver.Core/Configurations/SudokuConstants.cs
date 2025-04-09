namespace GASudokuSolver.Core.Configurations;

public static class SudokuConstants
{
	public static class Grid
	{
		public const int NumberOfRows = 9;
		public const int NumberOfColumns = 9;
	}

	public static class Subgrid
	{
		public const int NumberOfRows = 3;
		public const int NumberOfColumns = 3;
	}

	public static class Cell
	{
		public const int EmptyValue = 0;
		public const int MinValue = 1;
		public const int MaxValue = 9;
	}
}