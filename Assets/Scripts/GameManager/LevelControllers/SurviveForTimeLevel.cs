using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SurviveForTimeLevel : LevelController
{
    [Tooltip("The survival goal in seconds")]
    [SerializeField]
    private int TimeGoal;

    private float timer = 0;

    public void Update()
    {
        timer += Time.deltaTime;

        scoreBar.fillAmount = (float)timer / TimeGoal;

        if (timer >= TimeGoal && !GoalReached)
        {
            GoalReached = true;

            LevelCompleted();
        }
    }
}
