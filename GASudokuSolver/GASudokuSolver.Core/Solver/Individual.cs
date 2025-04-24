using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Genes;
using System.Security.Cryptography.X509Certificates;

namespace GASudokuSolver.Core.Solver.Interfaces;

class Individual
{
	public double Fitness { get; private set; }
	public Grid Board { get; private set; }
	public List<Gene> Genes { get; private set; }

	public IRepresentation Representation { get; private set; }
	public Individual(IRepresentation representation, Grid board)
	{
		Fitness = 0.0;
		Board = new Grid(board);
		Representation = representation;
		Genes = Representation.Encode(Board);
	}

	public void Evaluate(IFitnessFunction fitnessFunction)
	{
		Fitness = fitnessFunction.Eveluate(Board);
	}
	public void UpdateBoard()
	{
		Representation.Decode(Board, Genes);
	}
	public Individual Clone()
	{
		UpdateBoard();
		return new Individual(Representation, Board);
	}
}
