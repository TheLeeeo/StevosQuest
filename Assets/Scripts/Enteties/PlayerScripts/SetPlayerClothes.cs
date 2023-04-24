using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerClothes : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerHeadSpriteRenderer;
    [SerializeField] private SpriteRenderer playerChestSpriteRenderer;
    [SerializeField] private SpriteRenderer playerShoulderSpriteRenderer_1;
    [SerializeField] private SpriteRenderer playerShoulderSpriteRenderer_2;

    public void Start()
    {
        Sprite[] sprites = SaveSystem.GetClothesSprites();

        if(sprites[0] != null)
        {
            playerHeadSpriteRenderer.sprite = sprites[0];
        }

        if(sprites[1] != null)
        {
            playerChestSpriteRenderer.sprite = sprites[1];
        }

        if (sprites[1] != null)
        {
            playerShoulderSpriteRenderer_1.sprite = sprites[2];
            playerShoulderSpriteRenderer_2.sprite = sprites[2];
        }             
    }
}
