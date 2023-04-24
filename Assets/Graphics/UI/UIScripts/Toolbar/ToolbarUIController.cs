using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToolbarUIController : MonoBehaviour
{
    public static ToolbarUIController _instance;

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
    private GameObject ActiveSlotIndicator;
    [SerializeField]
    private GameObject BigActiveSlotIndicator;
    [SerializeField]
    private GameObject HighlightedSlotIndicator;
    [SerializeField]
    private GameObject BigHighlightedSlotIndicator;

    private int ActiveSlotID = -1;

    [SerializeField]
    private ToolbarSlot[] ItemSlots;

    private Item[] items = new Item[10];

    [SerializeField]
    private Text currentAmmoText;
    [SerializeField]
    private Text totalAmmoText;
    [SerializeField]
    private Image ratioOfMagImage;

    private float currentMagSize = float.MaxValue; //float for divison reasons
    private int currentAmmoAmmount;

    public void SetAmmoText(int ammount)
    {
        currentAmmoAmmount = ammount;
        currentAmmoText.text = ammount.ToString();
        ratioOfMagImage.fillAmount = ammount / currentMagSize;
    }

    public void WeaponReload(int ammoAmmount)
    {
        SetAmmoText(ammoAmmount);

        totalAmmoText.text = (BulletManager.NumberOfRemainingBullets(GameManager.CurrentUsedAmmoType)).ToString();
    }

    public void UpdateMagSize(int newSize)
    {
        currentMagSize = newSize;
        ratioOfMagImage.fillAmount = currentAmmoAmmount / currentMagSize;
    }

    public void UpateTotalAmmoText(AmmoType updatedType, int newTotalAmmount)
    {
        if (GameManager.CurrentUsedAmmoType == updatedType)
        {
            totalAmmoText.text = (newTotalAmmount).ToString();
        }
    }

    public void WeaponActivated(AmmoType weaponAmmoType, int currentAmmo, int magSize)
    {
        GameManager.CurrentUsedAmmoType = weaponAmmoType;
        totalAmmoText.text = (BulletManager.NumberOfRemainingBullets(weaponAmmoType)).ToString();

        currentAmmoAmmount = currentAmmo;
        currentAmmoText.text = currentAmmo.ToString();
        currentMagSize = magSize;

        ratioOfMagImage.fillAmount = currentAmmo / currentMagSize;
    }

    public void WeaponDeactivated()
    {
        totalAmmoText.text = "0";
        currentAmmoText.text = "0";
        ratioOfMagImage.fillAmount = 0f;
        GameManager.CurrentUsedAmmoType = (AmmoType)(-1);
    }


    public void ActivateSlot(int slotIndex)
    {
        if (slotIndex >= ItemSlots.Length || slotIndex < 0) //Slot id is outside of bounds
        {
            return;
        }
        else if (ActiveSlotID != slotIndex) //New slot is activated
        {
            if(items[slotIndex] != null) //There is an item in the slot
            {
                ActiveSlotID = slotIndex;

                if (slotIndex > 4)
                {
                    ActiveSlotIndicator.SetActive(true);
                    BigActiveSlotIndicator.SetActive(false);
                    ActiveSlotIndicator.transform.position = ItemSlots[slotIndex].transform.position;
                }
                else
                {
                    BigActiveSlotIndicator.SetActive(true);
                    ActiveSlotIndicator.SetActive(false);
                    BigActiveSlotIndicator.transform.position = ItemSlots[slotIndex].transform.position;
                }

                PlayerEquipedItemsController._instance.SetActiveItem(items[slotIndex]);
            }
        } else //The active alot is pressed again, deactivate in that case
        {
            if (items[slotIndex] != null) //There is an item in the slot
            {
                DeactivateCurrentSlot();
            }
        }
    }

    public void DeactivateCurrentSlot()
    {
        ActiveSlotID = -1;

        BigActiveSlotIndicator.SetActive(false);
        ActiveSlotIndicator.SetActive(false);

        PlayerEquipedItemsController._instance.DeactivateItem();
    }

    public void HighlightSlot(int slotIndex)
    {
        if(slotIndex > 4)
        {
            HighlightedSlotIndicator.SetActive(true);
            BigHighlightedSlotIndicator.SetActive(false);
            HighlightedSlotIndicator.transform.position = ItemSlots[slotIndex].transform.position;
        } else
        {
            BigHighlightedSlotIndicator.SetActive(true);
            HighlightedSlotIndicator.SetActive(false);
            BigHighlightedSlotIndicator.transform.position = ItemSlots[slotIndex].transform.position;
        }
    }

    public void UnHighlightSlot()
    {
        HighlightedSlotIndicator.SetActive(false);
        BigHighlightedSlotIndicator.SetActive(false);
    }

    public void SetItemInSlot(Item _item, int slotIndex)
    {
        items[slotIndex] = _item;

        ItemSlots[slotIndex].SetItem(_item);

        if(slotIndex == ActiveSlotID)
        {
            PlayerEquipedItemsController._instance.SetActiveItem(items[slotIndex]);
        }
    }

    public void UpdateUtilityCount(int slotIndex, int newValue)
    {
        ItemSlots[slotIndex].UpdateCount(newValue);
    }

    public void ClearSlot(int slotIndex)
    {
        items[slotIndex] = null;

        ItemSlots[slotIndex].Clear();
    }

    public void ClearAll()
    {
        for (int i = 0; i < 10; i++)
        {
            ClearSlot(i);
        }
    }
}
