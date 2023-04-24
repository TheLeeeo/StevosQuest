using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ScorePointsLevel : LevelController
{
    [SerializeField]
    private int ScoreGoal;

    public void ChangeScore(int newScore)
    {
        scoreBar.fillAmount = (float)newScore / ScoreGoal;

        if(newScore >= ScoreGoal && !GoalReached)
        {
            GoalReached = true;

            LevelCompleted();
        }
    }
}
