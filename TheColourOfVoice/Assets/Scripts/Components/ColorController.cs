using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorController : MonoBehaviour
{
    public Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        if (image)
        {
            image.color = ColorManager.Instance.GetColor(LevelManager.Instance.levelColor);
        }
    }
}