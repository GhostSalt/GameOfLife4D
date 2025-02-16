using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatableButton : MonoBehaviour
{
    public TextMesh ColourblindText;

    private MeshRenderer Rend;
    private Coroutine ButtonAnimCoroutine;

    private void Awake()
    {
        Rend = GetComponent<MeshRenderer>();
    }

    public void Press()
    {
        if (ButtonAnimCoroutine != null)
            StopCoroutine(ButtonAnimCoroutine);
        ButtonAnimCoroutine = StartCoroutine(ButtonAnim());
    }

    private IEnumerator ButtonAnim(float duration = 0.075f, float depression = 0.0025f)
    {
        float timer = 0;
        while (timer < duration)
        {
            Rend.transform.localPosition = new Vector3(Rend.transform.localPosition.x, Mathf.Lerp(0, -depression, timer / duration), Rend.transform.localPosition.z);
            yield return null;
            timer += Time.deltaTime;
        }
        Rend.transform.localPosition = new Vector3(Rend.transform.localPosition.x, -depression, Rend.transform.localPosition.z);

        timer = 0;
        while (timer < duration)
        {
            Rend.transform.localPosition = new Vector3(Rend.transform.localPosition.x, Mathf.Lerp(-depression, 0, timer / duration), Rend.transform.localPosition.z);
            yield return null;
            timer += Time.deltaTime;
        }
        Rend.transform.localPosition = new Vector3(Rend.transform.localPosition.x, 0, Rend.transform.localPosition.z);
    }

    public void SetColourblindText(string text)
    {
        ColourblindText.text = text;
    }

}
