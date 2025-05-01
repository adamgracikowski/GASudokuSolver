using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Genes;

namespace GASudokuSolver.Core.Solver.Interfaces;

/// <summary>
/// Defines the encoding and decoding logic for translating between a Sudoku grid and its gene-based representation.
/// </summary>
public interface IRepresentation
{
	/// <summary>
	/// Encodes the given Sudoku board into a list of genes.
	/// This defines how the board is represented genetically.
	/// </summary>
	/// <param name="board">The Sudoku board to encode.</param>
	/// <returns>A list of <see cref="Gene"/> representing the board.</returns>
	List<Gene> Encode(Grid board);

	/// <summary>
	/// Decodes the given list of genes into the provided Sudoku board.
	/// This reconstructs the board's state based on its genetic representation.
	/// </summary>
	/// <param name="board">The board to update based on the genes.</param>
	/// <param name="genes">The list of <see cref="Gene"/> to decode from.</param>
	void Decode(Grid board, List<Gene> genes);
}
