using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Slider[] sliders = new Slider[4];
    [SerializeField] private Text[] texts = new Text[4];

    private PlayerStats playerStats;
    private Camera playerCamera;

    private void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        playerCamera = FindFirstObjectByType<Camera>();

        InitializeValues(sliders[0], playerStats._sensitivity, texts[0]);
    }

    private void Update()
    {
        UpdateTexts();
    }

    public void AcceptChangeSensitivity()
    {
        playerStats._sensitivity = sliders[0].value;

        return;
    }

    public void AcceptChangePostProcessing()
    {
        var cameraData = playerCamera.GetComponent<UniversalAdditionalCameraData>();
        cameraData.renderPostProcessing = Convert.ToBoolean(sliders[1].value);

        return;
    }

    private void InitializeValues(Slider slider, float value, Text text)
    {
        slider.value = value;
        text.text = value.ToString();
    }

    private void UpdateTexts()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i] == null) return;

            texts[i].text = sliders[i].value.ToString("F2");
        }
    }
}