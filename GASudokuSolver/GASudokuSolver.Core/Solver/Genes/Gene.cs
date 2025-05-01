namespace GASudokuSolver.Core.Solver.Genes;

public abstract class Gene
{
    public abstract void Randomize();
    public abstract void Mutate();
    public abstract Gene Clone();
    public abstract void Copy(Gene gene);
}
