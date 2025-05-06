using GASudokuSolver.Core.Models;
using System.ComponentModel;

namespace GASudokuSolver.GUI.Models;

public class ChartPointData : INotifyPropertyChanged
{
	private AlgorithmProgressData progressData;
	private bool selected;

	public ChartPointData(AlgorithmProgressData progressData)
	{
		this.progressData = progressData;
	}


	public AlgorithmProgressData ProgressData
	{
		get => this.progressData;
		set
		{
			if (this.progressData != value)
			{
				this.progressData = value;
				OnPropertyChanged(nameof(ProgressData));
			}
		}
	}

	public bool Selected
	{
		get => this.selected;
		set
		{
			if (this.selected != value)
			{
				this.selected = value;
				OnPropertyChanged(nameof(Selected));
			}
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName) =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}