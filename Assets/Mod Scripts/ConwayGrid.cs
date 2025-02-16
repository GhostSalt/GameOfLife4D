using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class ConwayGrid
{
	/*
	 * 
	 * I'll be representing the grids in the format (x, y).
	 * Below are the representations of the cells in an example grid.
	 * 
	 * (0, 0) (1, 0) (2, 0) (3, 0) ...
	 * (0, 1) (1, 1) (2, 1) (3, 1) ...
	 * (0, 2) (1, 2) (2, 2) (3, 2) ...
	 * (0, 3) (1, 3) (2, 3) (3, 3) ...
	 *  ...    ...    ...    ...   ...
	 * 
	 * */

	private List<List<bool>> Grid;

	public ConwayGrid(List<List<bool>> grid)
	{
		Grid = grid;
	}

	public ConwayGrid(int size, bool isClear = false)
	{
		Grid = GenerateGrid(size, isClear);
	}

	private List<List<bool>> GenerateGrid(int size, bool isClear)
	{
		var generatedGrid = new List<List<bool>>();
		for (int i = 0; i < size; i++)
		{
            generatedGrid.Add(new List<bool>());
            for (int j = 0; j < size; j++)
				generatedGrid[i].Add(!isClear && Rnd.Range(0, 2) == 0);
		}

		return generatedGrid;
	}

	public List<List<bool>> GetGrid()
	{
		return Grid;
	}

	public bool GetCell(int x, int y)
	{
		return Grid[x][y];
    }

	public void SetCell(int x, int y, bool state)
	{
		Grid[x][y] = state;
    }

    public ConwayGrid Copy()
    {
		return new ConwayGrid(Grid.Select(x => x.ToList()).ToList());
    }

	public bool Equals(ConwayGrid secondGrid)
	{
		var grid = secondGrid.GetGrid();
		if (grid.Count() < Grid.Count())
			return false;   // If this passes, we know the columns are fine too, since ConwayGrids can only be square.

		for (int i = 0; i < Grid.Count(); i++)
			for (int j = 0; j < Grid[i].Count(); j++)
				if (Grid[i][j] != grid[i][j])
					return false;

        return true;
    }
}
