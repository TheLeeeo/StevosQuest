using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractTextController : MonoBehaviour
{
    public static InteractTextController _instance { get; private set; }

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

    [SerializeField]
    private Text textComponent;

    public void SetupText(Vector3 position, string textToDisplay = "interact")
    {
        gameObject.SetActive(true);

        textComponent.text = textToDisplay + " (E)";

        position.y += 1;
        transform.position = position;
    }

    public void SetupText(Vector3 position, string textToDisplay, Vector2 positionOffset, Vector2 rectScale)
    {
        gameObject.SetActive(true);

        textComponent.text = textToDisplay + " (E)";

        position += (Vector3)positionOffset;
        transform.position = position;
        (transform as RectTransform).sizeDelta = rectScale;
    }

    public void DisableText()
    {
        gameObject.SetActive(false);
        transform.localScale = Vector3.one;
    }
}
