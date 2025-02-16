using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSelectColourCollection
{
    private static readonly ColourPacket[] AllColours = new[] {
        new ColourPacket(0, "Red", "R", new Color(1, 0.25f, 0.25f)),
        new ColourPacket(1, "Orange", "O", new Color(1, 0.625f, 0.25f)),
        new ColourPacket(2, "Yellow", "Y", new Color(1, 1, 0.25f)),
        new ColourPacket(3, "Green", "G", new Color(0.25f, 1, 0.25f)),
        new ColourPacket(4, "Cyan", "C", new Color(0.25f, 1, 1)),
        new ColourPacket(5, "Blue", "B", new Color(0.25f, 0.25f, 1)),
        new ColourPacket(6, "Purple", "P", new Color(0.625f, 0.25f, 1)),
        new ColourPacket(7, "Grey", "A", new Color(0.5f, 0.5f, 0.5f)),
        new ColourPacket(8, "White", "W", new Color(1, 1, 1)),
    };

    private List<ColourPacket> ChosenColours;

    public LayerSelectColourCollection(int size)
    {
        ChosenColours = GenerateColours(size);
    }

    private List<ColourPacket> GenerateColours(int size)
    {
        var chosenColours = new List<ColourPacket>();

        for (int i = 0; i < size; i++)
            chosenColours.Add(AllColours.PickRandom());

        return chosenColours;
    }

    public List<ColourPacket> GetColours()
    {
        return ChosenColours;
    }
}
