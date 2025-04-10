namespace GASudokuSolver.Core.Configurations;

public static class Constants
{
	public static class Grid
	{
		public const int Rows = 9;
		public const int Columns = 9;
		public const int Cells = Rows * Columns;
		public const int Subgrids = (Rows / Subgrid.Rows) * (Columns / Subgrid.Columns);
	}

	public static class Subgrid
	{
		public const int Rows = 3;
		public const int Columns = 3;
		public const int Cells = Rows * Columns;
	}

	public static class Cell
	{
		public const int EmptyValue = 0;
		public const int MinValue = 1;
		public const int MaxValue = 9;
	}
}