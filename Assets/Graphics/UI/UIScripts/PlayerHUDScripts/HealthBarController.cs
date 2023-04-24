using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField]
    private new Transform transform;

    [SerializeField]
    private GameObject healthBarObject;
    [SerializeField]
    private GameObject armorBarObject;

    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Image armorBar;

    private bool armorBarActive = false;
    private bool healthBarActive = false;    

    public void Flip()
    {
        CustomMath.FlipTransform(transform);
    }

    public void SetHealth(float healthPercent)
    {
        healthBar.fillAmount = healthPercent;
    }

    public void SetArmor(float armorPercent)
    {
        armorBar.fillAmount = armorPercent;
    }

    public void SetHealthBarActive(bool active)
    {
        if (healthBarActive != active)
        {
            healthBarObject.SetActive(active);
        }

        healthBarActive = active;
    }

    public void SetArmorBarActive(bool active)
    {        
        if(armorBarActive != active)
        {
            armorBarObject.SetActive(active);
        }

        armorBarActive = active;
    }    
}
