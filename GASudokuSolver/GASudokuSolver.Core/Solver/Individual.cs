using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Genes;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Solver;

/// <summary>
/// Represents a single individual in the population for the genetic algorithm.
/// </summary>
public class Individual
{
	/// <summary>
	/// Gets the fitness value of this individual, as evaluated by a fitness function.
	/// </summary>
	public double Fitness { get; private set; }

	/// <summary>
	/// Gets the Sudoku grid associated with this individual.
	/// </summary>
	public Grid Board { get; private set; }

	/// <summary>
	/// Gets the representation strategy used to encode and decode the Sudoku grid.
	/// </summary>
	public IRepresentation Representation { get; private set; }

	/// <summary>
	/// List of genes representing this individual's solution encoding.
	/// </summary>
	public List<Gene> Genes;

	/// <summary>
	/// Initializes a new instance of the <see cref="Individual"/> class using the specified representation and initial board.
	/// </summary>
	/// <param name="representation">The representation strategy to use for encoding and decoding the board.</param>
	/// <param name="board">The initial Sudoku board to base this individual on.</param>
	public Individual(IRepresentation representation, Grid board)
	{
		Fitness = 0.0;
		Board = new Grid(board);
		Representation = representation;
		Genes = Representation.Encode();
	}

	public Individual(Individual baseIndividual, IList<Gene> newGenes)
	{
		Fitness = 0.0;
		Representation = baseIndividual.Representation;
		Genes = newGenes.ToList();
		Board = new Grid(baseIndividual.Board);
	}

	/// <summary>
	/// Evaluates this individual using the specified fitness function.
	/// </summary>
	/// <param name="fitnessFunction">The fitness function used to calculate the fitness of this individual.</param>
	public void Evaluate(IFitnessFunction fitnessFunction)
	{
		Fitness = fitnessFunction.Eveluate(Board);
	}

	/// <summary>
	/// Updates the board using the current gene values and the representation's decoding logic.
	/// </summary>
	public void UpdateBoard()
	{
		Representation.Decode(Board, Genes);
	}

	/// <summary>
	/// Creates a deep copy of this individual.
	/// </summary>
	/// <returns>A new <see cref="Individual"/> instance with the same representation and a cloned board.</returns>
	public Individual Clone()
	{
		UpdateBoard();
		var clone = new Individual(Representation, Board);
		clone.Fitness = Fitness;
		return clone;
	}

	/// <summary>
	/// Creates a deep copy of this individual's genes.
	/// </summary>
	/// <returns>
	/// A new list containing cloned <see cref="Gene"/> instances that replicate the current genes of this individual.
	/// </returns>
	public List<Gene> CloneGenes()
	{
		return Genes.Select(g => g.Clone()).ToList();
	}
}
