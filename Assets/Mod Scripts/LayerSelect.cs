using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSelect : MonoBehaviour
{
    public KMSelectable[] Buttons;
    public AnimatableButton[] ButtonsAnimatable;
    public Material[] ButtonMats;
    public KMAudio Audio;
    public KMColorblindMode Colourblind;

    private LayerSelectColourCollection Colours;
    private Color OffColour;
    private int LayerIndex = 0;
    private bool CannotPress = true;
    private bool IsColourblindEnabled;

    public event Action<int> OnLayerChange;

    private void Start()
    {
        IsColourblindEnabled = Colourblind.ColorblindModeActive;

        SetColours(-1);
        OffColour = ButtonMats[0].color;
        Colours = new LayerSelectColourCollection(Buttons.Length);
        for (int i = 0; i < Buttons.Length; i++)
        {
            int x = i;
            Buttons[x].OnInteract += delegate { if (!CannotPress) ButtonPress(x); return false; };
        }
    }

    public void Activate()
    {
        SetColours(LayerIndex);
        CannotPress = false;
    }

    private void SetColours(int pos)
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            var rend = Buttons[i].GetComponent<MeshRenderer>();
            if (i == pos)
            {
                rend.material = ButtonMats[1];
                rend.material.color = Colours.GetColours()[pos].GetValue();
                if (IsColourblindEnabled)
                    ButtonsAnimatable[i].SetColourblindText(Colours.GetColours()[pos].GetColourblind());
                else
                    ButtonsAnimatable[i].SetColourblindText("");
            }
            else
            {
                rend.material = ButtonMats[0];
                rend.material.color = OffColour;
                ButtonsAnimatable[i].SetColourblindText("");
            }
        }
    }

    private void ButtonPress(int pos)
    {
        ButtonsAnimatable[pos].Press();
        Audio.PlaySoundAtTransform("press", Buttons[pos].transform);
        Audio.PlaySoundAtTransform("change layers", transform);
        Buttons[pos].AddInteractionPunch();
        LayerIndex = pos;
        OnLayerChange?.Invoke(LayerIndex);
        SetColours(pos);
    }
}
