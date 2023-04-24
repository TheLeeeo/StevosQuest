using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class LevelController : MonoBehaviour
{
    public static LevelController _instance;

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

    public bool GoalReached { get; protected set; } = false;

    [SerializeField]
    private int _maxEnemyCount;
    public static int maxEnemyCount => _instance._maxEnemyCount;
    private int _activeEnemyCount;
    public static int activeEnemyCount
    {
        get { return _instance._activeEnemyCount; }
        set { _instance._activeEnemyCount = value; }
    }

    public int score { get; private set; }

    public UnityEvent onLevelStart;

    public UnityEvent<int> OnScoreChange; //For when enteties are killed

    public UnityEvent OnLevelComplete;

    [SerializeField] public AudioClip musicTrack;

    [SerializeField]
    protected Image scoreBar;

    [SerializeField] private GameObject ScoreBarParentObject;

    public void LevelStart()
    {
        GameMusicController.Play(musicTrack);

        onLevelStart.Invoke();
    }

    protected void LevelCompleted() //Level goal has been reached
    {
        DirectionArrowController._instance.gameObject.SetActive(true);

        OnLevelComplete.Invoke();
    }

    public virtual void LevelFinished() //Nose mobile is entered and level is finished
    {
        PlayerHUDController._instance.RemoveAllEffects();
        ToolbarInventoryHandler._instance.DeactivateToolbar();

        DeactivateScoreBar();
    }

    public void DeactivateScoreBar()
    {
        if(null == scoreBar)
        {
            return;
        }

        ScoreBarParentObject.SetActive(false);
    }

    public void ActivateScoreBar()
    {
        if (null == scoreBar)
        {
            return;
        }

        ScoreBarParentObject.SetActive(true);
    }

    public void SetMaxEnemyCountToZero()
    {
        _maxEnemyCount = 0;
    }

    public static void AddScore(int ammount)
    {
        _instance.score += ammount;
        _instance.OnScoreChange.Invoke(_instance.score);
    }

    public static void RemoveScore(int ammount)
    {
        _instance.score -= ammount;
        _instance.OnScoreChange.Invoke(_instance.score);
    }

    public static void SetScore(int value)
    {
        _instance.score = value;
    }

    public virtual EntityController SpawnEntity(GameObject entityPrefab, Vector3 position)
    {
        EntityController entityController = Instantiate(entityPrefab, position, Quaternion.identity).GetComponentInChildren<EntityController>();

        entityController.deathEvent.AddListener(() => EntityKilled());
        activeEnemyCount++;

        position.z = -5f;
        Instantiate(PrefabManager.GetRandomSpawnedEntityEffect(), position, Quaternion.identity);

        return entityController;
    }

    public virtual void EntityKilled()
    {
        activeEnemyCount--;
    }
}
