using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class CreateNewEntity : Editor
{
    private static Transform SceneItemFolderTransform;

    private static Transform GetSceneItemFolder()
    {
        if (SceneItemFolderTransform == null)
        {
            SceneItemFolderTransform = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().First(go => go.name == "Enemies").transform;
        }

        return SceneItemFolderTransform;
    }

    [MenuItem("GameObject/New Enemy-template", priority = 1100)]
    private static void CreateEnemy()
    {
        GameObject gameObject = (GameObject)Instantiate(AssetDatabase.LoadAssetAtPath("Assets/Editor/SpawnGameObjects/EntityTemplate.prefab", typeof(GameObject)), GetSceneItemFolder());

        Undo.RegisterCreatedObjectUndo(gameObject, "Create new enemy");
    }
}
