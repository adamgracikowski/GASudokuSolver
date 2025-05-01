using GASudokuSolver.Core.Configurations;

namespace GASudokuSolver.Core.Solver.Genes;

public class SingleCellGene : Gene
{
	public byte Number;

	public SingleCellGene()
	{
		Randomize();
	}

	public SingleCellGene(byte number)
	{
		Number = number;
	}

	public override Gene Clone()
	{
		return new SingleCellGene(Number);
	}

	public override void Copy(Gene gene)
	{
		var singleCellGene = (gene as SingleCellGene) ??
			throw new Exception("Cannot copy different gene Representation");

		Number = singleCellGene.Number;
	}

	public override void Mutate()
	{
		Randomize();
	}

	public override void Randomize()
	{
		Number = (byte)Random.Shared.Next(Constants.Cell.MinValue, Constants.Cell.MaxValue + 1);
	}

}
