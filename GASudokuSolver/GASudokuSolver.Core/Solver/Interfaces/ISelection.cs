namespace GASudokuSolver.Core.Solver.Interfaces;

interface ISelection
{
	IList<IIndividual> Select(
		IList<IIndividual> population,
		int count,
		Func<double, double, bool> compare
	);
}
