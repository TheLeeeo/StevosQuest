using UnityEngine;

[ExecuteInEditMode]
public class CameraMovement : MonoBehaviour
{
    public static CameraMovement _instance;

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
    private Transform cameraTransform;

    public Transform playerTransform;

    private void Start()
    {
        if(null != PlayerController._instance)
        {
            playerTransform = PlayerController._instance.playerTransform;
        }
        else
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        cameraTransform.position = new Vector3(playerTransform.transform.position.x, playerTransform.transform.position.y, transform.position.z);
    }

    public void Stop()
    {
        this.enabled = false;
    }
}
