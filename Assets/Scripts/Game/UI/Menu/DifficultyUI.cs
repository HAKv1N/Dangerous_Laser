using UnityEngine;
using UnityEngine.UI;

public class DifficultyUI : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Text textDifficulty;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();

        switch (GameSettings.DifficultyLevel)
        {
            case -1:
                UpdateDifficultyUI(new Color(0, 1, 1, 0.5f), "Safe");
                break;
            case 0:
                UpdateDifficultyUI(new Color(0, 1, 0, 0.5f), "Easy");
                break;
            case 1:
                UpdateDifficultyUI(new Color(1, 1, 0, 0.5f), "Medium");
                break;
            case 2:
                UpdateDifficultyUI(new Color(1, 0, 0, 0.5f), "Hard");
                break;
            case 3:
                UpdateDifficultyUI(new Color(0.48f, 0, 1, 0.5f), "Insane");
                break;
            case 4:
                UpdateDifficultyUI(new Color(1, 0.5f, 0, 0.5f), "Impossible");
                break;
        }
    }

    private void Update() {
        if (GameSettings.DifficultyLevel == -2)
        {
            UpdateDifficultyUIRainbow(new Color(0.5f, 0.5f, 0.5f, 1), "Custom");
        }
    }

    private void UpdateDifficultyUI(Color color, string difficulty)
    {
        image.color = color;

        textDifficulty.text = difficulty;
    }

    private float timer = 0f;

    private void UpdateDifficultyUIRainbow(Color color, string difficulty)
    {
        timer += 0.1f * Time.deltaTime;

        if (timer > 1f) timer -= 1f;

        image.color = Color.HSVToRGB(timer, 1f, 1f);

        textDifficulty.text = difficulty;
    }
}