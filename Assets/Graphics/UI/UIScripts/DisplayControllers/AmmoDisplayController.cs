using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplayController : MonoBehaviour
{
    public static AmmoDisplayController _instance;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            Debug.LogError("Instance of singleton class \"" + this.name + "\" already exists in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }
    }

    [SerializeField]
    private Text[] bulletAmmountTexts;

    public void UpdateAmmoOfType(AmmoType type, int newAmmount)
    {
        bulletAmmountTexts[(int)type].text = newAmmount.ToString();
    }
}
