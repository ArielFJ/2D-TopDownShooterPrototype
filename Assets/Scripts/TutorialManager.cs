using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [SerializeField] private GameObject tutorialScreen;
    public Toggle toggleTuto;
    //public GameObject checkImage;

    public bool dontShowTutorial;
    public bool isInTuto;

    private void Awake()
    {
        instance = this;        
        toggleTuto.isOn = dontShowTutorial;
    }

    private void Start()
    {
        dontShowTutorial = false;
        isInTuto = false;
    }

    public void ShowTutorial()
    {
        tutorialScreen.SetActive(true);
        isInTuto = true;
    }

    public void HideTutorial()
    {
        tutorialScreen.SetActive(false);
        isInTuto = false;
    }

    public void CheckShowTutorial()
    {
        dontShowTutorial = !dontShowTutorial;
        //checkImage.SetActive(dontShowTutorial);
    }
}
