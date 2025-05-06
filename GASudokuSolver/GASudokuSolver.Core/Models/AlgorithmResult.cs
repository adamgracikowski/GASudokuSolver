using GASudokuSolver.Core.Enums;
using GASudokuSolver.Core.Solver.Interfaces;

namespace GASudokuSolver.Core.Models;

public record AlgorithmResult(
	AlgorithmProgressData BestIndividual,
	TimeSpan ElapsedTime,
	TerminationReason TerminationReason
);
