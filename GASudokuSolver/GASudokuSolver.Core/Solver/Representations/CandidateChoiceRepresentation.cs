using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.Representations;

public sealed class CandidateChoiceRepresentation : IRepresentation
{
	public void Decode(Grid board, List<Gene> genes)
	{
		var geneIndex = 0;
		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for (var col = 0; col < Constants.Grid.Columns; col++)
			{
				if (!board.Mutable[row, col])
					continue;
				var choiceGene = genes[geneIndex++] as CandidateChoiceGene
					?? throw new ArgumentException("Gene type mismatch");
				board.Data[row, col] = choiceGene.Number;
			}
		}
	}

	public List<Gene> Encode(Grid board)
	{
		var genes = new List<Gene>();
		for (var row = 0; row < Constants.Grid.Rows; row++)
		{
			for (var col = 0; col < Constants.Grid.Columns; col++)
			{
				if (!board.Mutable[row, col])
					continue;

				var domain = ComputeCandidates(board, row, col);
				
				genes.Add(new CandidateChoiceGene(domain));
			}
		}
		return genes;
	}

	private static byte[] ComputeCandidates(Grid board, int row, int col)
	{
		var used = new bool[Constants.Cell.MaxValue + 1]; 
		
		for (var c = 0; c < Constants.Grid.Columns; c++)
		{
			var value = board.Data[row, c];
			if (value != Constants.Cell.EmptyValue) used[value] = true;
		}

		for (var r = 0; r < Constants.Grid.Rows; r++)
		{
			var value = board.Data[r, col];
			if (value != 0) used[value] = true;
		}

		var br = (row / Constants.Subgrid.Rows) * Constants.Subgrid.Rows;
		var bc = (col / Constants.Subgrid.Columns) * Constants.Subgrid.Columns;

		for (var r = br; r < br + Constants.Subgrid.Rows; r++)
		{
			for (var c = bc; c < bc + Constants.Subgrid.Columns; c++)
			{
				var value = board.Data[r, c];
				if (value != 0) used[value] = true;
			}
		}

		var candidates = new List<byte>(9);
		for (var v = Constants.Cell.MinValue; v <= Constants.Cell.MaxValue; v++)
		{
			if (!used[v]) candidates.Add(v);
		}

		return [.. candidates];
	}
}