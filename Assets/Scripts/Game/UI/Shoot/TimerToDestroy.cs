using UnityEngine;
using UnityEngine.UI;

public class TimerToDestroy : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        UpdateTextTranspency();
    }

    private void UpdateTextTranspency()
    {
        Color newColor = text.color;
        newColor.a -= 1.5f * Time.deltaTime;

        text.color = newColor;

        if (text.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}