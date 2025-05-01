namespace GASudokuSolver.Core.Configurations;

public static class Constants
{
	public static class Grid
	{
		public const int Rows = 9;
		public const int Columns = 9;
		public const int Cells = Rows * Columns;
		public const int Subgrids = SubgridsInRow * SubgridsInColumn;
		public const int SubgridsInRow = (Columns / Subgrid.Columns);
		public const int SubgridsInColumn= (Rows / Subgrid.Rows);
	}

	public static class Subgrid
	{
		public const int Rows = 3;
		public const int Columns = 3;
		public const int Cells = Rows * Columns;
		
	}

	public static class Cell
	{
		public const byte EmptyValue = 0;
		public const byte MinValue = 1;
		public const byte MaxValue = 9;
	}

	public static class Solver
	{
		public const int DefaultMaxGenerations = 100;
		public const double DefaultMaxTimeInMinutes = 5.0;
	}
}