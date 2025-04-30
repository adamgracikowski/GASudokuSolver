namespace GASudokuSolver.Core.Solver.Genes;

public abstract class Gene
{
    public abstract void RandomGene();
    public abstract void Mutate();
    public abstract Gene Clone();
}
