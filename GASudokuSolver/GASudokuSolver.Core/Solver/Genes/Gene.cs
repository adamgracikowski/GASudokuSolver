namespace GASudokuSolver.Core.Solver.Genes;

abstract class Gene
{
    public abstract Gene Random();
    public abstract void Mutate();
    public abstract Gene Clone();
}
