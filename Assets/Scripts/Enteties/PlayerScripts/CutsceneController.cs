using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;

    [SerializeField] private TimelineAsset[] timelineAssets;

    private int index = 0;

    public void Start()
    {
        playableDirector.playableAsset = timelineAssets[0];
        playableDirector.Play();
    }

    public void NextSection()
    {
        index++;

        if(index >= timelineAssets.Length)
        {
            FinishCutscene();
        }

        playableDirector.playableAsset = timelineAssets[index];
        playableDirector.Play();
    }

    public void FinishCutscene()
    {
        playableDirector.Stop();
        SceneLoader.NextLevel();
    }

    public void Skip(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            FinishCutscene();
        }
    }
}
