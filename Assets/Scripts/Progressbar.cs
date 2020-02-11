using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progressbar : MonoBehaviour
{
    public Color startColor;
    public Color midColor;
    public Color endColor;

    private float curValNormalized = 0.0f;
    private Scrollbar bar;

    private void Start()
    {
        if (!TryGetComponent<Scrollbar>(out bar))
        {
            Debug.LogError(string.Format("Progressbar {0} does not have a scrollbar!", this.name));
        }
    }

    public void setPercentage(float newPercentage)
    {
        curValNormalized = newPercentage;

        if (curValNormalized < 0.5f)
        {
            SetColor(Color.Lerp(endColor, midColor, curValNormalized));
        }
        else
        {
            SetColor(Color.Lerp(midColor, startColor, curValNormalized));
        }

        bar.size = newPercentage;
    }

    private void SetColor(Color color)
    {
        ColorBlock colors = bar.colors;
        colors.disabledColor = color;
        bar.colors = colors;
    }
}
