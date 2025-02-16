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

    void Awake()
    {
        _moduleID = _moduleIdCounter++;
        Module.OnActivate += delegate { MainGrid.Activate(); };

        MainGrid.OnRequestStrike += delegate { Module.HandleStrike(); };
        MainGrid.OnRequestSolve += delegate { Module.HandlePass(); };
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
