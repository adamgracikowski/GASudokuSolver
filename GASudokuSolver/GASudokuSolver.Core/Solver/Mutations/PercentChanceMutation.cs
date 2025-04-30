using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.Mutations;

public class PercentChanceMutation : IMutation
{
	private double chance;

	public PercentChanceMutation(double chance)
	{
		chance = Math.Min(Math.Max(chance, 0), 100);
		this.chance = chance/100;
	}

	public void Mutate(Gene gene)
	{
		double random = Random.Shared.NextDouble();
		if(random<=chance)
		{
			gene.Mutate();
		}
	}
}
