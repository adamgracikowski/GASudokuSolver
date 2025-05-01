using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;
using GASudokuSolver.Core.Configurations;
using System.Windows.Automation;

namespace GASudokuSolver.Core.Solver.Representations;

public class SingleCellRowCollumnRepresentation : IRepresentation
{
	public void Decode(Grid board, List<Gene> genes)
	{
		int geneIndex = 0;
		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for(var col = 0;  col < Constants.Grid.Columns; col++)
			{
				if (board.Mutable[row, col])
				{
					SingleCellGene? gene = genes[geneIndex] as SingleCellGene;
					if (gene == null)
					{
						// should never happen
						throw new Exception("Invalid gene type");
					}
					board.Data[row, col] = gene.Number;
					geneIndex++;
				}
			}
		}
	}

	public List<Gene> Encode(Grid board)
	{
		List<Gene> genes = new List<Gene>();
		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for (var col = 0; col < Constants.Grid.Columns; col++)
			{
				if (board.Mutable[row, col])
				{
					genes.Add(new SingleCellGene());
				}
			}
		}
		return genes;
	}
}
