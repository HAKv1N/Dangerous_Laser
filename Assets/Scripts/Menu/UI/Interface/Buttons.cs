using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interface
{
    public class Buttons : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private GameObject menuLevels;

        public void StartGame()
        {
            menuLevels.SetActive(!menuLevels.activeSelf);
            SceneManager.LoadScene("Game");
        }

        public void StartTest()
        {
            SceneManager.LoadScene("Test");
        }
    }    
}