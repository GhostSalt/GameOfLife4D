using NUnit.Framework.Internal.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class MainGrid : MonoBehaviour
{
    public KMSelectable[] Buttons;
    public KMSelectable[] TopButtons;
    public AnimatableButton[] ButtonsAnimatable;
    public AnimatableButton[] TopButtonsAnimatable;
    public Material[] ButtonMats;
    public KMAudio Audio;
    public LayerSelect LayerSelect;

    private int GridSize;
    private ConwayGridCollection InitialState;
    private ConwayGridCollection CurrentState;
    private ConwayGridCollection AnswerState;
    private List<bool> ButtonStates;
    private int CurrentLayer;
    private bool CannotPress = true;
    private bool IsSolved;

    public event Action OnRequestStrike;
    public event Action OnRequestSolve;

    private void Awake()
    {
        GridSize = Mathf.FloorToInt(Mathf.Sqrt(Buttons.Length));
        InitialState = new ConwayGridCollection(GridSize);
        CurrentState = InitialState.Copy();
        AnswerState = InitialState.Copy();
    }

    private void Start()
    {
        SetColoursBlack();
        for (int i = 0; i < Buttons.Length; i++)
        {
            int x = i;
            Buttons[x].OnInteract += delegate { if (!CannotPress) ButtonPress(x); return false; };
        }
        for (int i = 0; i < TopButtons.Length; i++)
        {
            int x = i;
            TopButtons[x].OnInteract += delegate { if (!CannotPress) TopButtonPress(x); return false; };
        }

        LayerSelect.OnLayerChange += SetColours;
    }

    public void Activate()
    {
        LayerSelect.Activate();
        SetColours(0);
        StartCoroutine(Animate());
        CannotPress = false;
    }

    private void SetColours(int layer)
    {
        CurrentLayer = layer;
        ButtonStates = new List<bool>();
        for (int i = 0; i < Buttons.Length; i++)
        {
            var rend = Buttons[i].GetComponent<MeshRenderer>();
            var colour = rend.material.color;

            if (CurrentState.GetCell(i % 4, i / 4, layer))
            {
                rend.material = ButtonMats[1];
                ButtonStates.Add(true);
                rend.material.color = colour;
            }
            else
            {
                rend.material = ButtonMats[0];
                ButtonStates.Add(false);
            }
        }
    }

    private void SetColoursBlack()
    {
        for (int i = 0; i < Buttons.Length; i++)
            Buttons[i].GetComponent<MeshRenderer>().material = ButtonMats[0];
    }

    private void ButtonPress(int pos)
    {
        ButtonsAnimatable[pos].Press();
        Audio.PlaySoundAtTransform("press", Buttons[pos].transform);
        Buttons[pos].AddInteractionPunch();

        CurrentState.SetCell(pos % 4, pos / 4, CurrentLayer, !CurrentState.GetCell(pos % 4, pos / 4, CurrentLayer));
        SetColours(CurrentLayer);
    }

    private void TopButtonPress(int pos)
    {
        TopButtonsAnimatable[pos].Press();
        Audio.PlaySoundAtTransform("press", TopButtons[pos].transform);
        TopButtons[pos].AddInteractionPunch();

        if (pos == 0 && !IsSolved)
        {
            if (!CurrentState.Equals(AnswerState))
                OnRequestStrike?.Invoke();
            else
            {
                OnRequestSolve?.Invoke();
                StartCoroutine(SolveAnim());
                IsSolved = true;
            }
        }
        else if (pos == 1)
        {
            CurrentState = new ConwayGridCollection(GridSize, true);
            SetColours(CurrentLayer);
        }
        else if (pos == 2)
        {
            CurrentState = InitialState.Copy();
            SetColours(CurrentLayer);
        }
    }

    private IEnumerator Animate(float interval = 0.35f, float lightnessRange = 0.7f, float randomnessRange = 0.15f, float solveFlashInterval = 0.1f)
    {
        var sequence = new Queue<Color>();
        sequence.Enqueue(new Color(Rnd.Range(lightnessRange, 1), Rnd.Range(lightnessRange, 1), Rnd.Range(lightnessRange, 1)));
        for (int i = 1; i < (2 * GridSize) + 2; i++)
        {
            var colour = sequence.ElementAt(i - 1);
            sequence.Enqueue(new Color(Mathf.Clamp(colour.r + Rnd.Range(-randomnessRange, randomnessRange), lightnessRange, 1),
                Mathf.Clamp(colour.g + Rnd.Range(-randomnessRange, randomnessRange), lightnessRange, 1),
                Mathf.Clamp(colour.b + Rnd.Range(-randomnessRange, randomnessRange), lightnessRange, 1)));
        }

        var sequenceArray = sequence.ToArray();

        float timer = 0;

        while (!IsSolved)
        {
            timer = 0;

            while (timer < interval)
            {
                if (IsSolved)
                    goto skip;

                for (int i = 0; i < Buttons.Length; i++)
                    if (ButtonStates[i])
                        Buttons[i].GetComponent<MeshRenderer>().material.color = Color.Lerp(sequenceArray[(i / GridSize) + (i % GridSize)], sequenceArray[(i / GridSize) + (i % GridSize) + 1], timer / interval);

                yield return null;
                timer += Time.deltaTime;
            }

            if (IsSolved)
                goto skip;

            for (int i = 0; i < Buttons.Length; i++)
                if (ButtonStates[i])
                    Buttons[i].GetComponent<MeshRenderer>().material.color = sequenceArray[(i / GridSize) + (i % GridSize) + 1];

            sequence.Dequeue();

            var colour = sequence.ElementAt(sequenceArray.Length - 2);
            sequence.Enqueue(new Color(Mathf.Clamp(colour.r + Rnd.Range(-randomnessRange, randomnessRange), lightnessRange, 1),
                Mathf.Clamp(colour.g + Rnd.Range(-randomnessRange, randomnessRange), lightnessRange, 1),
                Mathf.Clamp(colour.b + Rnd.Range(-randomnessRange, randomnessRange), lightnessRange, 1)));
            sequenceArray = sequence.ToArray();
        }

        skip:
        while (true)
        {
            var value = Easing.InOutSine(Mathf.PingPong(Time.time / 2f, 1), 0.5f, 0.25f, 1);

            for (int i = 0; i < Buttons.Length; i++)
                if (ButtonStates[i])
                    Buttons[i].GetComponent<MeshRenderer>().material.color = new Color(value, 1, value);

            yield return null;
        }
    }

    private IEnumerator SolveAnim()
    {
        Audio.PlaySoundAtTransform("solve", transform);
        CurrentState = InitialState.Copy();
        SetColours(CurrentLayer);

        yield return new WaitForSeconds(0.2f);

        Audio.PlaySoundAtTransform("solve", transform);
        CurrentState = AnswerState.Copy();
        SetColours(CurrentLayer);
    }
}
