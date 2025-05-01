using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver.Mutations;

public sealed class PercentChanceMutation : IMutation
{
	private readonly double percentage;

	public PercentChanceMutation(double chance)
	{
		chance = Math.Clamp(chance, 0, 100);
		this.percentage = chance / 100;
	}

	public void Mutate(Gene gene)
	{
		var random = Random.Shared.NextDouble();
		
		if(random <= percentage)
		{
			gene.Mutate();
		}
	}
}