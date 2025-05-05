using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Configurations;

namespace GASudokuSolver.Core.Solver.Representations;

public sealed class MultiCellRepresentation : IRepresentation
{
	public GroupByStrategy GroupBy;
	public int[]? MutablesInGroup;
	public int[]? GroupIndexByGeneIndex;

	public MultiCellRepresentation(GroupByStrategy groupBy)
	{
		GroupBy = groupBy;
	}

	public void Decode(Grid board, List<Gene> genes)
	{
		if (GroupIndexByGeneIndex!.Length != genes.Count)
			throw new ArgumentException("Gene number missmatch");
		if (GroupBy == GroupByStrategy.Rows)
		{
			for (var geneIndex = 0; geneIndex < genes.Count; geneIndex++)
			{
				var gene = genes[geneIndex] as MultipleCellGene
					?? throw new ArgumentException("Gene type mismatch");
				var row = GroupIndexByGeneIndex[geneIndex];
				var inGeneIndex = 0;
				for(var col = 0; col < Constants.Grid.Columns; col++)
				{
					if (!board.Mutable[row, col])
						continue;
					board.Data[row, col] = gene.Numbers[inGeneIndex];
					inGeneIndex++;
				}
			}
		}
		else if(GroupBy == GroupByStrategy.Columns)
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
		else // GroupBy == GroupByStrategy.Subgrids
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
					for(var col = startCol; col < startCol + Constants.Subgrid.Columns; col++)
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
		foreach (var mutables in MutablesInGroup!)
		{
			if (mutables == 0)
				continue;
			genes.Add(new MultipleCellGene(mutables));
		}
		return genes;
	}

	public void SetupRepresentation(Grid board)
	{
		var mutablesInRows = new int[Constants.Grid.Rows];
		var mutablesInColumns = new int[Constants.Grid.Columns];
		var mutablesInSubgrids = new int[Constants.Grid.Subgrids];

		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for (var col = 0; col < Constants.Grid.Columns; col++)
			{
				if (!board.Mutable[row, col])
					continue;

				mutablesInRows[row]++;

				mutablesInColumns[col]++;

				var subgrid = row / Constants.Subgrid.Rows * Constants.Grid.SubgridsInRow + col / Constants.Subgrid.Columns;

				mutablesInSubgrids[subgrid]++;
			}
		}

		if (GroupBy == GroupByStrategy.Rows)
			MutablesInGroup = mutablesInRows;
		else if (GroupBy == GroupByStrategy.Columns)
			MutablesInGroup = mutablesInColumns;
		else // GroupBy == GroupByStrategy.Subgrids
			MutablesInGroup = mutablesInSubgrids;

		var geneIndexByGroupIndex = new List<int>();
		for(var i = 0; i < MutablesInGroup.Length; i++)
		{
			if (MutablesInGroup[i] != 0)
				geneIndexByGroupIndex.Add(i);
			
		}

		GroupIndexByGeneIndex = [.. geneIndexByGroupIndex];
	}
}
