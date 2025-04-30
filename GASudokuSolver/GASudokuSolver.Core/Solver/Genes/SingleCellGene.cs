using System;

namespace GASudokuSolver.Core.Solver.Genes;

public class SingleCellGene : Gene
{
	public byte Number;

	public SingleCellGene()
	{
		Number = (byte)Random.Shared.Next(1, 10);
	}

	public SingleCellGene(byte number)
	{
		Number = number;
	}

	public override Gene Clone()
	{
		return new SingleCellGene(Number);
	}

	public override void Mutate()
	{
		RandomGene();
	}

	public override void RandomGene()
	{
		Number = (byte)Random.Shared.Next(1, 10);
	}
}
