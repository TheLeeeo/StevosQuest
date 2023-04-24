using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class CreateSceneItem : Editor
{
    private static Transform SceneItemFolderTransform;

    private static Transform GetSceneItemFolder()
    {
        if (SceneItemFolderTransform == null)
        {
            SceneItemFolderTransform = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().First(go => go.name == "SceneItems").transform;            
        }

        return SceneItemFolderTransform;
    }

    [MenuItem("GameObject/SceneItem", priority = 1200)]
    private static void SpawnSceneItem()
    {
        GameObject gameObject = (GameObject)Instantiate(AssetDatabase.LoadAssetAtPath("Assets/Editor/SpawnGameObjects/SceneInteractableItem.prefab", typeof(GameObject)), GetSceneItemFolder()); ;
        gameObject.name = "SceneItem";

        Vector3 position = SceneView.lastActiveSceneView.camera.transform.position;
        position.z = 0;

        gameObject.transform.position = position;

        Undo.RegisterCreatedObjectUndo(gameObject, "Create SceneItem");
    }
}
