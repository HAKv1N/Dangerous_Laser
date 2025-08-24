using UnityEngine;
using UnityEngine.UI;

public class RainbowButton : MonoBehaviour
{
    private Image image;
    private float timer = 0f;
    public float speed = 1f;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        timer += speed * Time.deltaTime;

        if (timer > 1f) timer -= 1f;
        
        image.color = Color.HSVToRGB(timer, 1f, 1f);
    }
}