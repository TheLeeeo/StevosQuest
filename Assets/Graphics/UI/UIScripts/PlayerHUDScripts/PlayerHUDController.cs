using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUDController : MonoBehaviour
{
    public static PlayerHUDController _instance;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            Debug.LogError("Instance of singleton class \"" + this + "\" already exists in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }
    }


    [SerializeField]
    private GameObject EffectDisplayPrefab;

    private EffectDisplayController[] activeDisplaysInOrder = new EffectDisplayController[Effect.MaxNumberOfActiveEffects];
    private EffectDisplayController[] displaysForType = new EffectDisplayController[Effect.NumberOfEffectTypes];

    private int indexOfFreeDisplay = 0;
    private int numberOfInstantiatedDisplays = 0;


    public void DisplayEffect(Effect effect)
    {
        if (indexOfFreeDisplay >= numberOfInstantiatedDisplays)
        {
            activeDisplaysInOrder[indexOfFreeDisplay] = Instantiate(EffectDisplayPrefab, transform).GetComponent<EffectDisplayController>();
            numberOfInstantiatedDisplays++;
        } else
        {
            activeDisplaysInOrder[indexOfFreeDisplay].gameObject.SetActive(true);
        }

        activeDisplaysInOrder[indexOfFreeDisplay].Initiate(effect);
        activeDisplaysInOrder[indexOfFreeDisplay].UpdatePosition(indexOfFreeDisplay);

        displaysForType[(int)effect.effectData.EffectType] = activeDisplaysInOrder[indexOfFreeDisplay];

        indexOfFreeDisplay++;
    }

    public void RemoveEffect(EffectEnum effectID)
    {
        if(null == displaysForType[(int)effectID])
        {
            return;
        }

        int indexInOrder = displaysForType[(int)effectID].currentIndex;
        indexOfFreeDisplay--;

        EffectDisplayController temp = activeDisplaysInOrder[indexInOrder];
        temp.gameObject.SetActive(false);

        for (int i = indexInOrder; i < indexOfFreeDisplay; i++)
        {
            activeDisplaysInOrder[i] = activeDisplaysInOrder[i + 1];
            activeDisplaysInOrder[i].UpdatePosition(i);
        }

        activeDisplaysInOrder[indexOfFreeDisplay] = temp;
        displaysForType[(int)effectID] = null;
    }

    public void UpdateEffectTime(Effect effect)
    {
        displaysForType[(int)effect.effectData.EffectType].UpdateTime(effect.Duration);
    }

    public void RemoveAllEffects()
    {
        for (int i = 0; i < activeDisplaysInOrder.Length; i++)
        {
            if (null == activeDisplaysInOrder[i])
            {
                return;
            }

            Destroy(activeDisplaysInOrder[i].gameObject);
            activeDisplaysInOrder[i] = null;

            displaysForType[i] = null;
            indexOfFreeDisplay = 0;
            numberOfInstantiatedDisplays = 0;
        }
    }
}
