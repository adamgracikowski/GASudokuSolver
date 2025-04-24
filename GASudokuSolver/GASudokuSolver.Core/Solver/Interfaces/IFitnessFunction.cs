using GASudokuSolver.Core.Models;

namespace GASudokuSolver.Core.Solver.Interfaces;

interface IFitnessFunction
{
    double Eveluate(Grid sudoku);
    bool Compare(double lhsFitnessValue, double rhsFitnessValue);
    bool IsSolved(double fitness);
}
