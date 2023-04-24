using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class SceneTransitionUIHandler : MonoBehaviour
{
    public static SceneTransitionUIHandler _instance;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            Debug.LogError("Second instance of singleton class \"" + this + "\" created in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }
    }

    [SerializeField] private Sprite penisLoadingBarImage;
    [SerializeField] private Sprite loadingBarImage;

    public Image TransitionProgressBar;

    [SerializeField] private RectTransform leftImage;
    [SerializeField] private RectTransform rightImage;
    [SerializeField] private RectTransform topImage;
    [SerializeField] private RectTransform bottomImage;

    private const float X_IMAGE_OFFSET = 480f;
    private const float Y_IMAGE_OFFSET = 270f;
    private const float ANIMATION_TIME = 0.5f;

    public void SetPenisMode(bool value)
    {
        TransitionProgressBar.sprite = value ? penisLoadingBarImage : loadingBarImage;
    }

    public static void AnimateTransitionIn()
    {
        _instance.leftImage.gameObject.SetActive(true);
        _instance.rightImage.gameObject.SetActive(true);

        LeanTween.moveX(_instance.leftImage, X_IMAGE_OFFSET, ANIMATION_TIME).setIgnoreTimeScale(true);
        LeanTween.moveX(_instance.rightImage, -X_IMAGE_OFFSET, ANIMATION_TIME).setIgnoreTimeScale(true).setOnComplete(() => SceneLoader._instance.InAnimationComplete());
    }

    public static void AnimateTransitionOut()
    {
        LeanTween.moveX(_instance.leftImage, -X_IMAGE_OFFSET, ANIMATION_TIME).setIgnoreTimeScale(true);
        LeanTween.moveX(_instance.rightImage, X_IMAGE_OFFSET, ANIMATION_TIME).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            SceneLoader._instance.OutAnimationComplete();
            _instance.leftImage.gameObject.SetActive(false);
            _instance.rightImage.gameObject.SetActive(false);
        });
    }

    public static void AnimateDeathScreen()
    {
        LevelController._instance.DeactivateScoreBar();

        _instance.topImage.gameObject.SetActive(true);
        _instance.bottomImage.gameObject.SetActive(true);

        LeanTween.moveY(_instance.topImage, -Y_IMAGE_OFFSET, ANIMATION_TIME).setIgnoreTimeScale(true);
        LeanTween.moveY(_instance.bottomImage, Y_IMAGE_OFFSET, ANIMATION_TIME).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            PlayerInputHandler._instance.EnableGameControl();
        });
    }

    public static void RestoreDeathScreen()
    {
        _instance.topImage.gameObject.SetActive(false);
        _instance.bottomImage.gameObject.SetActive(false);

        ((RectTransform)_instance.topImage.transform).anchoredPosition = new Vector3(0, Y_IMAGE_OFFSET);
        ((RectTransform)_instance.bottomImage.transform).anchoredPosition = new Vector3(0, -Y_IMAGE_OFFSET);
    }
}
