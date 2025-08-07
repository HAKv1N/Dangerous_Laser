using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameUI
{
    public class Buttons : MonoBehaviour
    {
        public void ExitGame()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}