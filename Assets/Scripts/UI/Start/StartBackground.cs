using UnityEngine;
using UnityEngine.UI;

public class StartBackground : MonoBehaviour
{
    private bool start = false;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        start = true;
    }
    private void Update()
    {
        if (!start) return;

        UpdateBackgroundTranspency();
    }

    private void UpdateBackgroundTranspency()
    {
        Color newColor = image.color;
        newColor.a -= Time.deltaTime;

        image.color = newColor;

        if (image.color.a <= 0f)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}