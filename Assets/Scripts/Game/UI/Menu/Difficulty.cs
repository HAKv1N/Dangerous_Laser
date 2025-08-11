using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Text textDifficulty;

    private void Start()
    {
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

    private void UpdateDifficultyUI(Color color, string difficulty)
    {
        Image image = GetComponent<Image>();

        image.color = color;

        textDifficulty.text = difficulty;
    }
}