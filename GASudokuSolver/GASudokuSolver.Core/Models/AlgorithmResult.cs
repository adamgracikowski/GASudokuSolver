using GASudokuSolver.Core.Enums;

namespace GASudokuSolver.Core.Models;

public record AlgorithmResult(
	AlgorithmProgressData BestIndividual,
	TimeSpan ElapsedTime,
	TerminationReason TerminationReason
);