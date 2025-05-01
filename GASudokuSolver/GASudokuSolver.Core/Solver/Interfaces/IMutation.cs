using GASudokuSolver.Core.Solver.Genes;

namespace GASudokuSolver.Core.Solver.Interfaces;

/// <summary>
/// Controls the probability of mutation for a <see cref="Gene"/>. 
/// It decides whether a <see cref="Gene"/> should mutate, but does not define the mutation logic itself.
/// </summary>
public interface IMutation
{
	/// <summary>
	/// Determines whether the specified <see cref="Gene"/> should mutate based on a probability model, 
	/// and triggers its internal mutation logic if the condition is met.
	/// </summary>
	/// <param name="gene">The <see cref="Gene"/> to potentially mutate.</param>
	void Mutate(Gene gene);
}

