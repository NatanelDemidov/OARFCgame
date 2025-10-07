using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CamControl : MonoBehaviour
{

    [SerializeField] GameObject flashLight;
    [SerializeField] AudioSource nightVisionSound;
    float timeSpent;
    [SerializeField] GameObject camPanel;
    [SerializeField] Image camPanelIMG;
    [SerializeField] Color nightVisionColor;
    [SerializeField] Color regularVisionColor;
    [SerializeField] GameObject recIcon;
    [SerializeField] GameObject dust;
    float timeSpentRec;
    [SerializeField] AudioSource failCam;
    [SerializeField] GameObject camNightVision;
    [SerializeField] Slider batterySlider;
    [SerializeField] GameObject tutorialTextObject;
    [SerializeField] TMP_Text tutorialText;
    float batteryValue = 100;
    bool isActiveNightVision;
    bool isActiveCam = false;
    void Update()
    {
        batterySlider.value = batteryValue;
    }
    public void CameraPlaneManagment()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isActiveCam)
            {
                dust.SetActive(false);
                camPanel.SetActive(false);
                isActiveCam = false;
            }
            else
            {
                camPanel.SetActive(true);
                isActiveCam = true;
            }

        }
         if (isActiveCam)
         {
             timeSpentRec += Time.deltaTime;
             if (timeSpentRec >= 0.5f && recIcon.activeInHierarchy)
             {
                 recIcon.SetActive(false);
                 timeSpentRec = 0;
             }
             if (timeSpentRec >= 0.5f && !recIcon.activeInHierarchy)
             {
                 recIcon.SetActive(true);
                 timeSpentRec = 0;

             }
         }
        if (Input.GetKeyDown(KeyCode.F) && isActiveCam)
        {
            if (isActiveNightVision)
            {
                camNightVision.SetActive(false);
                dust.SetActive(false);
                isActiveNightVision = false;
            }
            else
            {
                dust.SetActive(true);
                camNightVision.SetActive(true);
                isActiveNightVision = true;
                if (!isActiveNightVision && batteryValue >= 2)
                {
                    nightVisionSound.Play();
                }
                if (batteryValue < 2 && !isActiveNightVision)
                {
                    failCam.Play();
                }
            }
        }
        if (isActiveNightVision && isActiveCam)
        {
            if (batteryValue > 2)
            {
                tutorialTextObject.SetActive(false);
            }
            camPanelIMG.color = nightVisionColor;
            flashLight.SetActive(true);
            timeSpent += Time.deltaTime;
            if (timeSpent >= 3)
            {
                batteryValue -= 2;
                timeSpent = 0;
            }
        }
        else
        {
            camPanelIMG.color = regularVisionColor;
            flashLight.SetActive(false);
            camNightVision.SetActive(false);
        }
        if (batteryValue < 2)
        {
            flashLight.SetActive(false);
            isActiveNightVision = false;
            dust.SetActive(false);
            camNightVision.SetActive(false);
            camPanelIMG.color = regularVisionColor;
        }
    }
}
