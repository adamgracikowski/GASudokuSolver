namespace GASudokuSolver.Core.Solver.Genes;

/// <summary>
/// Base class for genes that carry their own mutation rate
/// which is adapted (by Gaussian‐style jitter) on each call to Mutate().
/// </summary>
public abstract class SelfAdaptiveGene : Gene
{
	/// <summary>
	/// Probability [0..1] that this gene will actually perform a mutation.
	/// </summary>
	public double MutationRate { get; private set; }

	/// <summary>
	/// σ for how much the rate itself can change each generation.
	/// </summary>
	private readonly double _sigma;

	protected SelfAdaptiveGene(double initialRate = 0.1, double sigma = 0.05)
	{
		MutationRate = Math.Clamp(initialRate, 0, 1);
		
		_sigma = sigma;
	}

	public override void Mutate()
	{
		AdaptRate();
		if (Random.Shared.NextDouble() <= MutationRate)
			ApplyMutation();
	}

	protected abstract void ApplyMutation();

	private void AdaptRate()
	{
		var delta = (Random.Shared.NextDouble() * 2 - 1) * _sigma;
		
		MutationRate = Math.Clamp(MutationRate + delta, 0, 1);
	}
}