using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.Mutations;

public sealed class NeverMutation : IMutation
{
	public void Mutate(Gene gene)
	{
		return;
	}
}