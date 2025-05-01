namespace GASudokuSolver.Core.Solver.Genes;

/// <summary>
/// Picks one of the precomputed valid candidates for a single cell.
/// </summary>
public sealed class CandidateChoiceGene : Gene
{
	public byte Number => Candidates[Index];
	public readonly byte[] Candidates;
	public int Index;

	public CandidateChoiceGene(byte[] candidates)
	{
		if (candidates == null || candidates.Length == 0)
			throw new ArgumentException("Candidates must have at least one value", nameof(candidates));
	
		Candidates = candidates;

		Randomize();
	}

	public override Gene Clone()
	{
		return new CandidateChoiceGene(Candidates)
		{
			Index = this.Index
		};
	}

	public override void Copy(Gene gene)
	{
		if (gene is not CandidateChoiceGene other)
			throw new ArgumentException("Gene type mismatch", nameof(gene));

		Index = other.Index;
	}

	public override void Mutate()
	{
		Randomize();
	}

	public override void Randomize()
	{
		Index = Random.Shared.Next(0, Candidates.Length);
	}
}