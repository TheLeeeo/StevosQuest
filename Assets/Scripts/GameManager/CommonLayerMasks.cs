using UnityEngine;

public class CommonLayerMasks : MonoBehaviour
{
    public static CommonLayerMasks _instance;

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
    private LayerMask _groundCheckLayers;
    public static LayerMask GroundCheckLayers => _instance._groundCheckLayers;

    [SerializeField]
    private LayerMask _hasHealthLayers;
    public static LayerMask HasHealth => _instance._hasHealthLayers;

    [SerializeField]
    private LayerMask _entityLayer;
    public static LayerMask Entities => _instance._entityLayer;

    [SerializeField]
    private LayerMask _playerLayer;
    public static LayerMask Player => _instance._playerLayer;            

    public const int PlayerLayer = 14;
}
