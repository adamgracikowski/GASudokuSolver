using GASudokuSolver.Core.Configurations;

namespace GASudokuSolver.Core.Solver.Genes;

public class MultipleCellGene : Gene
{
	public byte[] Numbers;

	public MultipleCellGene(int size)
	{
		Numbers = new byte[size];
		Randomize();
	}

	public override Gene Clone()
	{
		var clone = new MultipleCellGene(Numbers.Length);
		Array.Copy(Numbers, clone.Numbers, Numbers.Length);
		return clone;
	}

	public override void Copy(Gene gene)
	{
		if (gene is not MultipleCellGene other)
			throw new ArgumentException("Gene type mismatch", nameof(gene));
		if (other.Numbers.Length != Numbers.Length)
			throw new ArgumentException("Gene length mismatch");

		Array.Copy(other.Numbers, Numbers, other.Numbers.Length);
	}

	public override void Mutate()
	{
		for (var i = 0; i < Math.Ceiling((double)Numbers.Length / 2); i++)
		{
			int randomIndex = Random.Shared.Next(Numbers.Length);
			Numbers[randomIndex] = (byte)Random.Shared.Next(Constants.Cell.MinValue, Constants.Cell.MaxValue + 1);
		}
	}

	public override void Randomize()
	{
		for(var i = 0; i < Numbers.Length; i++)
		{
			Numbers[i] = (byte)Random.Shared.Next(Constants.Cell.MinValue, Constants.Cell.MaxValue + 1);
		}
	}
}