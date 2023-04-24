using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScreenUIController : MonoBehaviour
{
    public static PauseScreenUIController _instance;
    private void Awake() => _instance = this;

    [SerializeField] private RectTransform rectTransform;

    const float SECONDS_TO_ACTIVATE = 0.5f;
    const float MOVEMENT_MULTIPLIER = 1080 / SECONDS_TO_ACTIVATE;

    private bool paused = false;

    private bool AnimationRunning;

    [SerializeField] private RectTransform OggvarTransform;
    [SerializeField] private RectTransform OggvarButtonTransform;

    [SerializeField] private Image[] ButtonImages;
    [SerializeField] private Sprite[] ButtonSprites;
    [SerializeField] private Sprite PenisSprite;

    [SerializeField] private Toggle musicToggle;

    [SerializeField] public AudioClip oggvarTrack;

    private const float MAX_X_VALUE = 700f;
    private const float MAX_Y_VALUE = 850f;
    private const float MIN_Y_VALUE = 225f;

    public static void TogglePause()
    {
        if (_instance.AnimationRunning)
        {
            _instance.StopAllCoroutines();
            _instance.AnimationRunning = false;
        }

        if (false == _instance.paused)
        {
            _instance.PauseGame();
        }
        else
        {
            _instance.UnPauseGame();
        }
    }

    public static void Pause() => _instance.PauseGame();
    public static void UnPause() => _instance.UnPauseGame();

    private void PauseGame()
    {
        if (RandomNumbers.RandomChance(0.25f))
        {
            OggvarButtonTransform.gameObject.SetActive(true);
            ((RectTransform)OggvarButtonTransform.transform).anchoredPosition = new Vector3(Random.Range(-MAX_X_VALUE, MAX_X_VALUE), Random.Range(MIN_Y_VALUE, MAX_X_VALUE));
            OggvarButtonTransform.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-90, 90));
        }

        if (false == _instance.paused)
        {
            GameMusicController.Pause();

            ToolbarInventoryHandler._instance.DeactivateToolbar();
            LevelController._instance.DeactivateScoreBar();

            musicToggle.isOn = !AudioListener.pause;

            paused = true;
            StartCoroutine(PauseGameAnim());
        }
    }

    private void UnPauseGame()
    {
        OggvarButtonTransform.gameObject.SetActive(false);

        if (true == _instance.paused)
        {
            GameMusicController.Play();

            ToolbarInventoryHandler._instance.ActivateToolbar();
            LevelController._instance.ActivateScoreBar();

            _instance.paused = false;
            _instance.StartCoroutine(_instance.UnPauseGameAnim());
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        PlayerHUDController._instance.RemoveAllEffects();
        SceneLoader.LoadMainMenu();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneLoader.ReloadScene();
    }

    public void DisablePauseScreen()
    {
        AnimationRunning = false;

        Vector3 pos = rectTransform.anchoredPosition;
        pos.y = 540;
        rectTransform.anchoredPosition = pos;
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

    public void PenisOff()
    {
        for (int i = 0; i < ButtonImages.Length; i++)
        {
            ButtonImages[i].sprite = ButtonSprites[i];
        }

        GameManager.penisMode = false;
        SceneTransitionUIHandler._instance.SetPenisMode(GameManager.penisMode);
    }

    public void PenisOn()
    {
        for (int i = 0; i < ButtonImages.Length; i++)
        {
            ButtonImages[i].sprite = PenisSprite;            
        }

        GameManager.penisMode = true;
        SceneTransitionUIHandler._instance.SetPenisMode(GameManager.penisMode);
    }

    private IEnumerator PauseGameAnim()
    {
        AnimationRunning = true;

        Time.timeScale = 0;

        Vector3 pos = rectTransform.anchoredPosition;

        while (rectTransform.anchoredPosition.y > -540)
        {
            pos.y -= Time.unscaledDeltaTime * MOVEMENT_MULTIPLIER;
            rectTransform.anchoredPosition = pos;

            yield return null;
        }

        pos.y = -540;
        rectTransform.anchoredPosition = pos;

        AnimationRunning = false;
    }

    private IEnumerator UnPauseGameAnim()
    {
        AnimationRunning = true;

        Time.timeScale = 1;

        Vector3 pos = rectTransform.anchoredPosition;

        while (rectTransform.anchoredPosition.y < 540)
        {
            pos.y += Time.unscaledDeltaTime * MOVEMENT_MULTIPLIER;
            rectTransform.anchoredPosition = pos;

            yield return null;
        }

        pos.y = 540;
        rectTransform.anchoredPosition = pos;

        AnimationRunning = false;
    }

    public void EnterOggvar()
    {
        LeanTween.scale(OggvarTransform, Vector3.one, 0.2f).setIgnoreTimeScale(true);
        GameMusicController.PlayOverlayTrack(oggvarTrack);
    }

    public void ExitOggvar()
    {
        LeanTween.scale(OggvarTransform, Vector3.zero, 0.2f).setIgnoreTimeScale(true);
        GameMusicController.StopOverlayTrack();
    }

    public void ToggleMusic(bool state)
    {
        AudioListener.pause = !state;
    }
}
