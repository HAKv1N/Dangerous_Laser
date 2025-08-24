using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Interface
{
    public class Buttons : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private GameObject menuLevels;
        [SerializeField] private GameObject testButton;
        [SerializeField] private InputField[] inputFields;
        [SerializeField] private Toggle toggle;
        [SerializeField] private GameObject customizeMenu;

        private void Start()
        {
            testButton.SetActive(Debug.isDebugBuild);
        }

        private void Update()
        {
            if (customizeMenu.activeSelf && Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                customizeMenu.SetActive(false);
            }
        }

        public void MenuLevels()
        {
            menuLevels.SetActive(!menuLevels.activeSelf);
        }

        public void StartGameNormal(int difficulty)
        {
            StartGame(difficulty, false);
        }

        public void StartGameCustom(int difficulty)
        {
            StartGame(difficulty, true);
        }

        private void StartGame(int difficulty, bool isCustom)
        {
            bool canStart = true;

            for (int i = 0; i < inputFields.Length; i++)
            {
                if (string.IsNullOrEmpty(inputFields[i].text))
                {
                    canStart = false;

                    break;
                }
            }

            if (!canStart && isCustom) return;

            if (isCustom)
            {
                GameSettings.ammo = Convert.ToInt16(inputFields[0].text);
                GameSettings.damage = (float)Convert.ToDouble(inputFields[1].text.Replace('.', ','));
                GameSettings.fireRate = (float)Convert.ToDouble(inputFields[2].text.Replace('.', ','));
                GameSettings.rangeAttack = (float)Convert.ToDouble(inputFields[3].text.Replace('.', ','));
                GameSettings.reloadRate = (float)Convert.ToDouble(inputFields[4].text.Replace('.', ','));
                GameSettings.isAgressive = toggle.isOn;
            }

            SceneManager.LoadScene("Game");
            GameSettings.DifficultyLevel = difficulty;
        }

        public void StartTest()
        {
            SceneManager.LoadScene("Test");
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void OpenCustomizeMenu()
        {
            customizeMenu.SetActive(true);

            RectTransform rectTransform = customizeMenu.GetComponent<RectTransform>();

            rectTransform.pivot = new Vector2(1, 1);
            rectTransform.position = Input.mousePosition;
        }
    }
}