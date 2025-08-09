using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Slider[] sliders;
    [SerializeField] private Text[] texts;
    [SerializeField] private int[] booleanSliders;

    private PlayerStats playerStats;
    private Camera playerCamera;
    [HideInInspector] public bool activePostProcessing = true;
    private SaveManager saveManager;

    private void Awake()
    {
        saveManager = GetComponentInParent<SaveManager>();

        playerCamera = FindFirstObjectByType<Camera>();
    }


    private void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();

        InitializeValues(sliders[0], playerStats._sensitivity, texts[0]);
        InitializeValues(sliders[1], Convert.ToSingle(activePostProcessing), texts[1]);
        InitializeValues(sliders[2], playerStats.GetComponent<PlayerController>().startFOV, texts[2]);
    }

    private void Update()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i] == null) continue;
            
            UpdateText(texts[i], sliders[i].value);
        }
    }

    public void AcceptChangeSensitivity()
    {
        playerStats._sensitivity = sliders[0].value;

        saveManager.Save();
    }

    public void AcceptChangePostProcessing()
    {
        activePostProcessing = Convert.ToBoolean(sliders[1].value);

        var cameraData = playerCamera.GetComponent<UniversalAdditionalCameraData>();
        cameraData.renderPostProcessing = activePostProcessing;

        saveManager.Save();
    }

    public void AcceptChangeFOV()
    {
        playerStats.GetComponent<PlayerController>().startFOV = sliders[2].value;

        saveManager.Save();
    }

    private void InitializeValues(Slider slider, float value, Text text)
    {
        slider.value = value;
        UpdateText(text, slider.value);
    }

    private void UpdateText(Text text, float value)
    {
        bool isBoolean = Array.Exists(booleanSliders, index => index == Array.IndexOf(texts, text));

        if (isBoolean)
        {
            if (value > 0.5f)
            {
                text.text = "ВКЛ.";
            }

            else
            {
                text.text = "ВЫКЛ.";
            }
        }

        else
        {
            text.text = value.ToString("F2");
        }
    }
}