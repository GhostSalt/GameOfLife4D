using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConwayGridCollection
{ 
	//Here, the grids, top to bottom on the module, will be represented in order in the list.

    private List<ConwayGrid> Grids;

    public ConwayGridCollection(List<ConwayGrid> grids)
    {
        Grids = grids;
    }

    public ConwayGridCollection(int size, bool isClear = false)
    {
        Grids = GenerateGrids(size, isClear);
    }

    private List<ConwayGrid> GenerateGrids(int size, bool isClear)
    {
        var gridList = new List<ConwayGrid>();

        for (int i = 0; i < size; i++)
            gridList.Add(new ConwayGrid(size, isClear));

        return gridList;
    }

    public List<ConwayGrid> GetGrids()
    {
        return Grids;
    }

    public ConwayGrid GetGrid(int ix)
    {
        return Grids[ix];
    }

    public bool GetCell(int x, int y, int z)
    {
        return Grids[z].GetCell(x, y);
    }

    public void SetCell(int x, int y, int z, bool state)
    {
        Grids[z].SetCell(x, y, state);
    }

    public ConwayGridCollection Copy()
    {
        return new ConwayGridCollection(Grids.Select(x => x.Copy()).ToList());
    }

    public bool Equals(ConwayGridCollection secondCollection)
    {
        for (int i = 0; i < Grids.Count(); i++)
            if (!Grids[i].Equals(secondCollection.GetGrids()[i]))
                return false;

        return true;
    }
}
