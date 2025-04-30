using GASudokuSolver.Core.Configurations;
using GASudokuSolver.Core.Models;

namespace GASudokuSolver.GUI;

public static class GeneticAlgorithmMock
{
	public static async Task<AlgorithmProgressData> Run(byte[,] board, IProgress<AlgorithmProgressData>? progress = null) 
	{
		var bestProgress = new AlgorithmProgressData(0, 0, new byte[Constants.Grid.Rows, Constants.Grid.Columns]);

		var generations = 100;
		var empty = new List<(int r, int c)>();

		for(var r = 0; r < board.GetLength(0); r++)
		{
			for(var c = 0; c < board.GetLength(1); c++)
			{
				if (board[r,c] == 0)
					empty.Add((r, c));
			}
		}

		var copy = new byte[Constants.Grid.Rows, Constants.Grid.Columns];

		for(var i = 0; i < generations; i++)
		{
			copy = new byte[Constants.Grid.Rows, Constants.Grid.Columns];

			Array.Copy(board, copy, copy.Length);

			foreach (var (r, c) in empty)
			{
				copy[r, c] = (byte)Random.Shared.Next(1, 9);
			}

			await Task.Delay(200);

			var fitness = 10 * Random.Shared.NextDouble() + 1;

			var currentProgress = (new AlgorithmProgressData(fitness, i, copy));

			if (fitness > bestProgress.FitnessValue)
			{
				bestProgress = currentProgress;
			}

			progress?.Report(currentProgress);
		}

		return bestProgress;
	}
}