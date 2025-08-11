using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interface
{
    public class Buttons : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private GameObject menuLevels;
        [SerializeField] private GameObject testButton;

        private void Start()
        {
            testButton.SetActive(Debug.isDebugBuild);
        }

        public void MenuLevels()
        {
            menuLevels.SetActive(!menuLevels.activeSelf);
        }

        public void StartGame(int difficulty)
        {
            GameSettings.DifficultyLevel = difficulty;

            SceneManager.LoadScene("Game");
        }

        public void StartTest()
        {
            SceneManager.LoadScene("Test");
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}