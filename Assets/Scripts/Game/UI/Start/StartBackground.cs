using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartBackground : MonoBehaviour
{
    private bool isStart = false;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.enabled = true;

        isStart = true;
    }

    private void Update()
    {
        if (!isStart) return;

        UpdateBackgroundTranspency();
    }

    private void UpdateBackgroundTranspency()
    {
        Color newColor = image.color;
        newColor.a -=  0.5f * Time.deltaTime;

        image.color = newColor;

        if (image.color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}