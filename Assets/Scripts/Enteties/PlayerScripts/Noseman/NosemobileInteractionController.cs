using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NosemobileInteractionController : MonoBehaviour, IIsInteractable
{
    [SerializeField] private GameObject redactedImageObject;

    public void PlayerInteraction()
    {
        (Sprite head, Sprite chest, Sprite shoulder) playerSprites = PlayerInventory._instance.armorController.GetPlayerSprites();
        NoseMobileController._instance.SetPlayerSprites(playerSprites.head, playerSprites.chest, playerSprites.shoulder);        

        StartCoroutine(ShowRedactedImage());
    }

    private IEnumerator ShowRedactedImage()
    {
        LevelController._instance.LevelFinished();

        redactedImageObject.SetActive(true);
        PlayerController._instance.gameObject.SetActive(false);
        DirectionArrowController._instance.gameObject.SetActive(false);

        Vector3 finalCameraPos = transform.position;
        finalCameraPos.z = -10;

        LeanTween.move(CameraMovement._instance.gameObject, finalCameraPos, 2f);
        CameraMovement._instance.Stop();

        yield return new WaitForSeconds(3f);

        redactedImageObject.SetActive(false);

        NoseMobileController._instance.InitiateLevelSwapSequence();
    }

    public void PlayerInRange()
    {
        InteractTextController._instance.SetupText(transform.position, "Enter nosemobile", new Vector2(1.5f, 3f), new Vector2(20, 3));
    }
}
