using GASudokuSolver.Core.Solver.Genes;

namespace GASudokuSolver.Core.Solver.Interfaces;

interface IMutation
{
    void Mutate(Gene gene);
}

