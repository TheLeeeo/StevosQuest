using UnityEngine;

[System.Serializable] //is this used? why is this here? do i dare to remove it?
public class GiveHealthOnActivate : MonoBehaviour
{
    [SerializeField]
    private int Ammount;

    private void OnEnable()
    {   
        GetComponent<ItemCore>().OnActivate = Activate;
    }

    private void Activate(EntityController _entityController)
    {
        _entityController.health.Heal(Ammount);
    }
}
