namespace GASudokuSolver.Core.Solver.Interfaces;

interface ICrossover
{
	IList<Individual> Crossover(IList<Individual> parents, int childrenCount);
}
