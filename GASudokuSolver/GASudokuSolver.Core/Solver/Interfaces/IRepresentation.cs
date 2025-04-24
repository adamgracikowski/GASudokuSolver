using GASudokuSolver.Core.Models;
using GASudokuSolver.Core.Solver.Genes;

namespace GASudokuSolver.Core.Solver.Interfaces;

interface IRepresentation
{
    List<Gene> Encode(Grid board);
	void Decode(Grid board, List<Gene> genes);
}
