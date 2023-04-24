using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController _instance;
    private void Awake() => _instance = this;

    [SerializeField] private Sprite[] ButtonSprites;
    [SerializeField] private Sprite PenisSprite;

    [SerializeField] private GameObject[] ButtonObjects;
    [SerializeField] private Image[] ButtonImages;

    [SerializeField] private GameObject OverwriteWarningScreen;
    [SerializeField] private GameObject SettingsErrorScreen;

    [SerializeField] private RectTransform GuideTransform;

    [SerializeField] private Toggle musicToggle;

    [SerializeField] private AudioClip audioClip;

    private bool saveExists;

    private const float ANIMATION_TIME = 0.2f;

    private readonly Vector3 ONE_AND_HALF_VECTOR = new Vector3(1.5f, 1.5f);

    private readonly Vector2 ButtonMaxPos = new Vector2(-225, 1005);
    private readonly Vector2 ButtonMinPos = new Vector2(-1695, 75);

    private int exitPressCount;

    private void Start()
    {
        GameMusicController.Play(audioClip);

        if (true == GameManager.penisMode)
        {
            PenisOn();
        }
        else
        {
            PenisOff();
        }

        musicToggle.isOn = !AudioListener.pause;

        saveExists = SaveSystem.LoadSaveFile();

        if (saveExists && SaveSystem.GetLevel() != -1)
        {
            ButtonObjects[0].SetActive(true);
        }
    }

    public void Continue()
    {
        SceneLoader.LoadGame(SaveSystem.GetLevel());
    }

    public void NewGame()
    {
        if (saveExists && SaveSystem.GetLevel() != -1)
        {
            LeanTween.scale(OverwriteWarningScreen, ONE_AND_HALF_VECTOR, ANIMATION_TIME);

            foreach (GameObject gameObject in ButtonObjects)
            {
                LeanTween.scale(gameObject, Vector3.zero, ANIMATION_TIME);
            }
        }
        else
        {
            AcceptNewGame();
        }
    }

    public void CancelNewGame()
    {
        LeanTween.scale(OverwriteWarningScreen, Vector3.zero, ANIMATION_TIME);

        foreach (GameObject gameObject in ButtonObjects)
        {
            LeanTween.scale(gameObject, ONE_AND_HALF_VECTOR, ANIMATION_TIME);
        }
    }

    public void AcceptNewGame()
    {
        const int FIRST_LEVEL_INDEX = 3;

        ToolbarUIController._instance.ClearAll();
        InventoryUIController._instance.ClearAll();

        SaveSystem.NewSaveFile();
        SceneLoader.LoadGame(FIRST_LEVEL_INDEX);
    }

    public void Settings()
    {
        LeanTween.scale(SettingsErrorScreen, ONE_AND_HALF_VECTOR, ANIMATION_TIME);

        foreach (GameObject gameObject in ButtonObjects)
        {
            LeanTween.scale(gameObject, Vector3.zero, ANIMATION_TIME);
        }
    }

    public void SettingsOkayButton()
    {
        LeanTween.scale(SettingsErrorScreen, Vector3.zero, ANIMATION_TIME);

        foreach (GameObject gameObject in ButtonObjects)
        {
            LeanTween.scale(gameObject, ONE_AND_HALF_VECTOR, ANIMATION_TIME);
        }
    }

    public void MoveExitButton()
    {
        exitPressCount++;

        if (exitPressCount <= 3)
        {
            ((RectTransform)ButtonObjects[4].transform).anchoredPosition = RandomNumbers.RandomVector(ButtonMinPos, ButtonMaxPos);
        }
    }

    public void Exit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Guide()
    {
        LeanTween.moveY(GuideTransform, 540f, 0.5f).setIgnoreTimeScale(true);
    }

    public void ExitGuide()
    {
        LeanTween.moveY(GuideTransform, -540f, 0.5f).setIgnoreTimeScale(true);
    }

    public void TogglePenis()
    {
        if (true == GameManager.penisMode)
        {
            PenisOff();
        }
        else
        {
            PenisOn();
        }
    }

    private void PenisOff()
    {
        for (int i = 0; i < ButtonImages.Length; i++)
        {
            ButtonImages[i].sprite = ButtonSprites[i];
            
        }

        GameManager.penisMode = false;
        PauseScreenUIController._instance.PenisOff();
    }

    private void PenisOn()
    {
        for (int i = 0; i < ButtonImages.Length; i++)
        {
            ButtonImages[i].sprite = PenisSprite;            
        }

        GameManager.penisMode = true;
        PauseScreenUIController._instance.PenisOn();
    }

    public void ToggleMusic(bool state)
    {
        AudioListener.pause = !state;
    }
}
