using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroController : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SaveSystem.SetLevelAsCompleted();
        SceneLoader.LoadMainMenu();
    }
}
