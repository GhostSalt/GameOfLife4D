using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

public class GameOfLife4DScript : MonoBehaviour
{
    static int _moduleIdCounter = 1;
    int _moduleID = 0;

    public KMBombModule Module;
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public LayerSelect LayerSelect;
    public MainGrid MainGrid;

    private static readonly int[][][] Orderings = new int[][][]
    {
        new int[][] { new[] { 1, 2, 0, 3 }, new[] { 0, 3, 1, 2 }, new[] { 2, 3, 1, 0 }, new[] { 2, 1, 3, 0 }, new[] { 1, 0, 3, 2 }, new[] { 2, 0, 1, 3 } },
        new int[][] { new[] { 2, 1, 0, 3 }, new[] { 3, 0, 1, 2 }, new[] { 2, 0, 3, 1 }, new[] { 1, 2, 3, 0 }, new[] { 3, 2, 0, 1 }, new[] { 0, 1, 3, 2 } },
        new int[][] { new[] { 3, 1, 2, 0 }, new[] { 2, 3, 0, 1 }, new[] { 1, 3, 0, 2 }, new[] { 3, 2, 1, 0 }, new[] { 1, 0, 2, 3 }, new[] { 0, 2, 3, 1 } },
        new int[][] { new[] { 3, 0, 2, 1 }, new[] { 3, 1, 0, 2 }, new[] { 0, 3, 2, 1 }, new[] { 0, 2, 1, 3 }, new[] { 1, 3, 2, 0 }, new[] { 0, 1, 2, 3 } }
    };

    void Awake()
    {
        _moduleID = _moduleIdCounter++;
        Module.OnActivate += delegate { MainGrid.Activate(); };

        MainGrid.OnRequestStrike += delegate { Module.HandleStrike(); };
        MainGrid.OnRequestSolve += delegate { Module.HandlePass(); };
    }

    void Start()
    {
        Calculate();
    }

    private void Calculate()
    {

    }
}
