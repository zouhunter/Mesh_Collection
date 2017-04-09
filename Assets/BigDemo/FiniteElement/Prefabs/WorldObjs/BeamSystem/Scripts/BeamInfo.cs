using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
[System.Serializable]
public class BeamInfo {
    public int length;
    public int wigth;
    public int height;
    public int gridSize = 1;
    //对于给定的gridSize
    public bool SetGridSize(int gridSize)
    {
        if (gridSize > 0)
        {
            bool can = true;
            if (can)
            {
                this.gridSize = gridSize;
                return true;
            }
        }
        return false;
    }
}
