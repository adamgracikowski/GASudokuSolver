using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.Mutations;

public sealed class AlwaysMutation : IMutation
{
	public void Mutate(Gene gene)
	{
		gene.Mutate();
	}
}