using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.Representations;

public sealed class MultiCellPermutationRepresentation : IRepresentation
{
	public MultiCellRepresenationGroupBy GroupBy;
	public byte[][]? ValuesInGroups;
	public int[]? GroupIndexByGeneIndex;

	public MultiCellPermutationRepresentation(MultiCellRepresenationGroupBy groupBy)
	{
		GroupBy = groupBy;
	}

	public void Decode(Grid board, List<Gene> genes)
	{
		if (GroupIndexByGeneIndex!.Length != genes.Count)
			throw new ArgumentException("Gene number missmatch");
		if (GroupBy == MultiCellRepresenationGroupBy.Rows)
		{
			for (var geneIndex = 0; geneIndex < genes.Count; geneIndex++)
			{
				var gene = genes[geneIndex] as MultipleCellGene
					?? throw new ArgumentException("Gene type mismatch");
				var row = GroupIndexByGeneIndex[geneIndex];
				var inGeneIndex = 0;
				for (var col = 0; col < Constants.Grid.Columns; col++)
				{
					if (!board.Mutable[row, col])
						continue;
					board.Data[row, col] = gene.Numbers[inGeneIndex];
					inGeneIndex++;
				}
			}
		}
		else if (GroupBy == MultiCellRepresenationGroupBy.Columns)
		{
			for (var geneIndex = 0; geneIndex < genes.Count; geneIndex++)
			{
				var gene = genes[geneIndex] as MultipleCellGene
					?? throw new ArgumentException("Gene type mismatch");
				var col = GroupIndexByGeneIndex[geneIndex];
				var inGeneIndex = 0;
				for (var row = 0; row < Constants.Grid.Rows; row++)
				{
					if (!board.Mutable[row, col])
						continue;
					board.Data[row, col] = gene.Numbers[inGeneIndex];
					inGeneIndex++;
				}
			}
		}
		else // GroupBy == MultiCellRepresenationGroupBy.Subgrids
		{
			for (var geneIndex = 0; geneIndex < genes.Count; geneIndex++)
			{
				var gene = genes[geneIndex] as MultipleCellGene
					?? throw new ArgumentException("Gene type mismatch");
				var startCol = GroupIndexByGeneIndex[geneIndex] % Constants.Grid.SubgridsInRow * Constants.Subgrid.Columns;
				var startRow = GroupIndexByGeneIndex[geneIndex] / Constants.Grid.SubgridsInRow * Constants.Subgrid.Rows;
				var inGeneIndex = 0;
				for (var row = startRow; row < startRow + Constants.Subgrid.Rows; row++)
				{
					for (var col = startCol; col < startCol + Constants.Subgrid.Columns; col++)
					{
						if (!board.Mutable[row, col])
							continue;
						board.Data[row, col] = gene.Numbers[inGeneIndex];
						inGeneIndex++;
					}
				}
			}
		}
	}

	public List<Gene> Encode()
	{
		var genes = new List<Gene>();
		foreach (var vig in ValuesInGroups!)
		{
			genes.Add(new MultipleCellPermutationGene(vig));
		}
		return genes;
	}

	public void SetupRepresentation(Grid board)
	{
		var numbersInRows = new bool[Constants.Grid.Rows, Constants.Cell.MaxValue];
		var numbersInColumns = new bool[Constants.Grid.Columns, Constants.Cell.MaxValue];
		var numbersInSubgrids = new bool[Constants.Grid.Subgrids, Constants.Cell.MaxValue];

		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for (var col = 0; col < Constants.Grid.Columns; col++)
			{
				if (board.Mutable[row, col])
					continue;

				var numberInCell = board.Data[row, col] - 1;

				numbersInRows[row, numberInCell] = true;

				numbersInColumns[col, numberInCell] = true;

				var subgrid = row / Constants.Subgrid.Rows * Constants.Grid.SubgridsInRow + col / Constants.Subgrid.Columns;

				numbersInSubgrids[subgrid, numberInCell] = true;
			}
		}

		bool[,] numbersInGroups;

		if (GroupBy == MultiCellRepresenationGroupBy.Rows)
			numbersInGroups = numbersInRows;
		else if (GroupBy == MultiCellRepresenationGroupBy.Columns)
			numbersInGroups = numbersInColumns;
		else // GroupBy == MultiCellRepresenationGroupBy.Subgrids
			numbersInGroups = numbersInSubgrids;

		var geneIndexByGroupIndex = new List<int>();
		var valuesInGroups = new List<List<byte>>();
		for (var i = 0; i < numbersInGroups.GetLength(0); i++)
		{
			var valuesInGroup = new List<byte>();
			for(var v = 0; v < numbersInGroups.GetLength(1); v++)
			{
				if (!numbersInGroups[i, v])
					valuesInGroup.Add((byte)(v + 1));
			}
			if (valuesInGroup.Count > 0)
			{
				geneIndexByGroupIndex.Add(i);
				valuesInGroups.Add(valuesInGroup);
			}

		}

		ValuesInGroups = new byte[valuesInGroups.Count][];
		for (var i = 0; i < valuesInGroups.Count; i++)
		{
			ValuesInGroups[i] = valuesInGroups[i].ToArray();
		}
		GroupIndexByGeneIndex = geneIndexByGroupIndex.ToArray();
	}
}
