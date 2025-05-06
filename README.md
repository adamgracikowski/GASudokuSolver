# 🧬 Genetic Algorithms Sudoku Solver

The project was developed as part of the _Introduction to Artificial Intelligence_ course during the summer semester of the 2024–2025 academic year.

## 📘 Table of Contents

1. [🔍 Overview](#-overview)
2. [🎯 Features](#-features)
3. [📦 Installation](#-installation)
4. [📊 Examples & Results](#-examples--results)
5. [🚀 Technologies Used](#-technologies-used)
6. [🤝 Authors](#-contributing)

## 🔍 Overview

This project implements a Sudoku solver based on a genetic algorithms. Unlike traditional methods that use backtracking or constraint propagation, our approach mimics the process of natural selection to evolve potential solutions across generations.

The application allows for **modular customization** of key components of the genetic algorithms, including:

- **Representation**
- **Fitness Function**
- **Selection**
- **Crossover**
- **Mutation**

## 🎯 Features

Our application offers a rich set of features that make experimentation with genetic algorithms both informative and interactive:

- ✅ **Modular algorithm components** – customize selection, crossover, mutation, representation, and fitness functions.
- 🔢 **Sudoku input from CSV files** – run the solver on any valid 9x9 puzzle loaded from a `.csv` file.
- 🎲 **Random initial population** – generate a population of a specified size for experimentation.
- 🧬 **Live best individual display** – view the most fit solution in each generation in real time.
- 📈 **Real-time fitness chart** – visualize the fitness of the best individual per generation as the algorithm evolves.
- ⏱️ **Execution time measurement** – automatically measure and display the algorithm’s runtime.
- 🛑 **Interrupt functionality** – manually stop the algorithm at any moment.
- 📤 **Output error analysis** – compare the final solution to the exact one and highlight incorrect values.

## 📦 Installation

To run the project locally:

1. **Clone the repository:**

```bash
git clone https://github.com/adamgracikowski/GASudokuSolver.git
cd GASudokuSolver
```

2. **Restore NuGet packages:**

```bash
dotnet restore
```

3. **Build and run the application:**

```bash
dotnet run --project GASudokuSolver.GUI
```

You can also open the solution in Visual Studio (recommended: 2022 or newer) and press `F5`.

## 📊 Examples & Results

## 🚀 Technologies Used

- **C#** – core programming language.
- **WPF (Windows Presentation Foundation)** – desktop UI framework.

The solution consists of the following projects:

- `GASudokuSolver.Core` - .NET class library containing:
  - Algorithm modules (selection, crossover, mutation, representation, fitness evaluation).
  - Data loading components (CSV input, puzzle representation).
  - Solver logic.
- `GASudokuSolver.GUI` – WPF application providing:
  - Graphical interface for loading puzzles and configuring algorithm parameters.
  - Live display of best individual and fitness chart.
  - Controls for start/stop and execution time measurement.

## 🤝 Authors

This project was created by:

- **Marcin Cieszyński** – [@Zumi002](https://github.com/Zumi002)
- **Adam Grącikowski** – [@adamgracikowski](https://github.com/adamgracikowski)
