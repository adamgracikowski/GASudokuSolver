using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.Representations;

public class SingleCellRowColumnRepresentation : IRepresentation
{
	private int mutableCells = 0;

	public void Decode(Grid board, List<Gene> genes)
	{
		var geneIndex = 0;
		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for(var col = 0;  col < Constants.Grid.Columns; col++)
			{
				if (!board.Mutable[row, col])
					continue;
				var gene = genes[geneIndex] as SingleCellGene
					?? throw new ArgumentException("Gene type mismatch");
				board.Data[row, col] = gene.Number;
				geneIndex++;
			}
		}
	}

	public List<Gene> Encode()
	{
		var genes = new List<Gene>(mutableCells);
		
		for (var cell = 0; cell < mutableCells; cell++)
		{
			genes.Add(new SingleCellGene());
		}
		return genes;
	}

	public void SetupRepresentation(Grid board)
	{
		var boardMutableCells = 0;
		
		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for (var col = 0; col < Constants.Grid.Columns; col++)
			{
				if (board.Mutable[row, col])
				{
					boardMutableCells++;
				}
			}
		}

		mutableCells = boardMutableCells;
	}
}