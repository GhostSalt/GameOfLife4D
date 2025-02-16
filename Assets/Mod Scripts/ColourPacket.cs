using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourPacket
{
    private int ID;
    private string Name;
    private string Colourblind;
    private Color Value;

    public ColourPacket(int iD, string name, string colourblind, Color value)
    {
        ID = iD;
        Name = name;
        Colourblind = colourblind;
        Value = value;
    }

    public int GetID()
    {
        return ID;
    }

    public string GetName()
    {
        return Name;
    }

    public string GetColourblind()
    {
        return Colourblind;
    }

    public Color GetValue()
    {
        return Value;
    }
}