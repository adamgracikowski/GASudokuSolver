using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.Mutations;

public sealed class NeighbourStepMutation : IMutation
{
	public void Mutate(Gene gene)
	{
		if (gene is not CandidateChoiceGene c)
			return;

		var length = c.Candidates.Length;

		if (length <= 1) return;

		var delta = Random.Shared.NextDouble() < 0.5 ? -1 : +1;
		
		c.Index = (c.Index + delta + length) % length;
	}
}