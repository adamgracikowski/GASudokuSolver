namespace GASudokuSolver.Core.Solver.Genes;

public sealed class MultipleCellPermutationGene : MultipleCellGene
{

	public byte[] ValuesInCells;

	public MultipleCellPermutationGene(byte[] valuesInCells) : base(valuesInCells.Length)
	{
		ValuesInCells = valuesInCells;
		Array.Copy(ValuesInCells, Numbers, ValuesInCells.Length);
		Randomize();
	}

	public override Gene Clone()
	{
		var clone = new MultipleCellPermutationGene(ValuesInCells);
		Array.Copy(Numbers, clone.Numbers, Numbers.Length);
		return clone;
	}

	public override void Copy(Gene gene)
	{
		if (gene is not MultipleCellPermutationGene other)
			throw new ArgumentException("Gene type mismatch", nameof(gene));
		if (other.Numbers.Length != Numbers.Length)
			throw new ArgumentException("Gene length mismatch");
		if(other.ValuesInCells != ValuesInCells)
			throw new ArgumentException("Gene values mismatch");
		Array.Copy(other.Numbers, Numbers, Numbers.Length);
	}

	public override void Mutate()
	{
		for (var k = 0; k < Math.Ceiling((double)Numbers.Length / 2); k++)
		{
			var i = Random.Shared.Next(Numbers.Length);
			var j = Random.Shared.Next(Numbers.Length);
			(Numbers[j], Numbers[i]) = (Numbers[i], Numbers[j]);
		}
	}

	public override void Randomize()
	{
		for(var i = Numbers.Length - 1; i >= 0; i--)
		{
			var j = Random.Shared.Next(i + 1);
			(Numbers[j], Numbers[i]) = (Numbers[i], Numbers[j]);
		}
	}
}
