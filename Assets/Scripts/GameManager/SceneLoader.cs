using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader _instance;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            Debug.LogError("Second instance of singleton class \"" + this.name + "\" created in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }
    }

    private static readonly int[] UNPLAYABLE_LEVELS = new int[]{ 2, 3, 16}; //Levels with no gameplay

    private const int MAIN_MENU_SCENE_ID = 2;
    private const int UI_SCENE_ID = 1;

    public static int sceneToLoad;
    public static int currentLevelBuildIndex;

    private AsyncOperation nextSceneLoadOperation;
    private AsyncOperation oldSceneUnloadOperation;

    private float progress;

    const int GAME_SCENE_INDEX = 2;

    public static bool levelCompleted = false;

    private static bool currentlyLoadingScene;
    private static bool loadingNonPlayableScene;

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 120, 30), "SwitchLevel"))
        {
            LevelController._instance.OnLevelComplete.Invoke();
            NextLevel();
        }
    }
#endif

    public void InAnimationComplete() //The in transition animation has completed
    {
        SceneTransitionUIHandler.RestoreDeathScreen(); //in case function call comes from death screen, yes this is a workaround
        PauseScreenUIController._instance.DisablePauseScreen(); //in case function call comes from pause screen

        GameMusicController.Stop();

        UnloadOldScene();
        LoadScene(sceneToLoad);

        StartCoroutine(ProgressBar());
    }

    public void OutAnimationComplete() //The out transition animation has completed
    {
        if(false == loadingNonPlayableScene) //if the scene loaded is a playable scene
        {
            SaveSystem.LoadGame();
            ToolbarInventoryHandler._instance.ActivateToolbar();
            LevelController._instance.LevelStart();
        }        

        currentlyLoadingScene = false;

        SceneTransitionUIHandler._instance.TransitionProgressBar.fillAmount = 0;        
    }

    private void Start()
    {
        LeanTween.init();

        SceneManager.LoadScene(UI_SCENE_ID, LoadSceneMode.Additive);

        _instance.nextSceneLoadOperation = SceneManager.LoadSceneAsync(MAIN_MENU_SCENE_ID, LoadSceneMode.Additive);
        _instance.nextSceneLoadOperation.completed += (_) => SceneManager.SetActiveScene(SceneManager.GetSceneAt(GAME_SCENE_INDEX));
    }

    public static void LoadGame(int startLevel)
    {
        if(false == currentlyLoadingScene)
        {            
            currentlyLoadingScene = true;

            currentLevelBuildIndex = startLevel;
            sceneToLoad = startLevel; //it gets incremented back after animation;

            SceneTransitionUIHandler.AnimateTransitionIn();
        }
    }

    private static void LoadScene(int sceneBuildIndex) //Load the next level inte the background during end cutscene
    {
        loadingNonPlayableScene = UNPLAYABLE_LEVELS.Contains(sceneBuildIndex);

        _instance.nextSceneLoadOperation = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);
        _instance.nextSceneLoadOperation.allowSceneActivation = false;

        _instance.nextSceneLoadOperation.completed += (_) => SceneManager.SetActiveScene(SceneManager.GetSceneAt(GAME_SCENE_INDEX));
    }

    public static void NextLevel() //Game is ready to switch to the next level
    {
        if (false == currentlyLoadingScene)
        {
            currentlyLoadingScene = true;

            currentLevelBuildIndex += 1;
            sceneToLoad += 1;

            if (false == loadingNonPlayableScene) //last scene was non playable
            {
                ToolbarUIController._instance.ClearAll();
                InventoryUIController._instance.ClearAll();
                SaveSystem.SaveGame();
            }
            
            SceneTransitionUIHandler.AnimateTransitionIn();

        }
    }

    /* public static void DisableOldScene()
    {
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            rootObject.SetActive(false);
        }
    }*/

    private void UnloadOldScene()
    {
        oldSceneUnloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator ProgressBar()
    {        
        progress = 0;
        while (progress < 1.8f)
        {
            progress = Mathf.MoveTowards(progress, nextSceneLoadOperation.progress + oldSceneUnloadOperation.progress, 0.1f);

            SceneTransitionUIHandler._instance.TransitionProgressBar.fillAmount = progress / 1.8f; //yes, 1.8

            yield return null;
        }

        Time.timeScale = 1;
        yield return new WaitForSeconds(0.25f); //yes

        SceneTransitionUIHandler.AnimateTransitionOut();
        nextSceneLoadOperation.allowSceneActivation = true;

        //scene load is complete
    }

    public static void ReloadScene()
    {        
        if(false == currentlyLoadingScene)
        {
            currentlyLoadingScene = true;

            PlayerHUDController._instance.RemoveAllEffects();

            GameManager._instance.CurrentWeapon = null;
            ToolbarUIController._instance.ClearAll();
            InventoryUIController._instance.ClearAll();

            SceneTransitionUIHandler.AnimateTransitionIn();

            PlayerInputHandler._instance.DisableGameControl();
        }        
    }

    public static void Death()
    {
        Time.timeScale = 0;
        SceneTransitionUIHandler.AnimateDeathScreen();
    }

    public static void LoadMainMenu()
    {
        if (false == currentlyLoadingScene)
        {
            sceneToLoad = MAIN_MENU_SCENE_ID;

            SceneTransitionUIHandler.AnimateTransitionIn();
        }
    }
}
