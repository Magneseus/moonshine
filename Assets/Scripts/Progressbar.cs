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
    private float maxWidth;
    private RawImage image;
    private RectTransform rect;

    private void Start()
    {
        if (!TryGetComponent<RawImage>(out image))
        {
            Debug.LogError(string.Format("Progressbar {0} does not have an image!", this.name));
        }
        if (!TryGetComponent<RectTransform>(out rect))
        {
            Debug.LogError(string.Format("Progressbar {0} does not have a RectTransform!", this.name));
        }

        maxWidth = rect.sizeDelta.x;
    }

    public void setPercentage(float newPercentage)
    {
        curValNormalized = newPercentage / 100.0f;

        if (curValNormalized < 0.5f)
        {
            SetColor(Color.Lerp(endColor, midColor, curValNormalized));
        }
        else
        {
            SetColor(Color.Lerp(midColor, startColor, curValNormalized));
        }

        rect.sizeDelta = new Vector2(Mathf.Lerp(0.0f, maxWidth, curValNormalized), rect.sizeDelta.y);
    }

    private void SetColor(Color color)
    {
        image.color = color;
    }
}
