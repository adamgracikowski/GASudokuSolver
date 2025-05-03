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
		public const int DefaultGenerations = 10_000;
		public const int MinimumGenerations = 10;
		public const int MaximumGenerations = 100_000;

		public const int DefaultPopulationSize = 10_000;
		public const int MinimumPopulationSize = 10;
		public const int MaximumPopulationSize = 100_000;

		public const int DefaultNumberOfParentsSize = 300;
		public const int MinimumNumberOfParentsSize = 10;
		public const int MaximumNumberOfParentsSize = 10_000;

		public const double DefaultTimeInMinutes = 5.0;
		public const double MinimumTimeInMinutes = 1.0;
		public const double MaximumTimeInMinutes = 120.0;
	}
}