using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.Mutations;

class NoMutation : IMutation
{
	public void Mutate(Gene gene)
	{
		return;
	}
}
