using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoseMobileController : MonoBehaviour
{
    public static NoseMobileController _instance;

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

    private const float Speed = 5f;

    [SerializeField] private new Rigidbody2D rigidbody;

    [SerializeField] private SpriteRenderer playerHeadSpriteRenderer;
    [SerializeField] private SpriteRenderer playerChestSpriteRenderer;
    [SerializeField] private SpriteRenderer playerShoulderSpriteRenderer_1;
    [SerializeField] private SpriteRenderer playerShoulderSpriteRenderer_2;

    [SerializeField] private GameObject playerObject;

    [SerializeField] private Animator[] feetAnimators;

    [SerializeField] private GameObject SpawnPointParticleObject;
    [SerializeField] private GameObject SpawnParticleObject;

    [SerializeField] private LineRenderer[] EyeLazers;

    [SerializeField] private GameObject ExplosionPrefab;

    [SerializeField] private GameObject NoseMobileObject;
    [SerializeField] private GameObject InteractorObject;

    [SerializeField] private GameObject tunnelBlockageObject;

    [SerializeField] private float walkOffDistance = 30;

    private bool finishedWithKilling = false;

    public void StopSpawnPointParticles()
    {
        SpawnPointParticleObject.SetActive(false);
    }

    public void PlaySpawnParticles()
    {
        SpawnParticleObject.SetActive(true);
    }

    public void SetPlayerSprites(Sprite playerHead, Sprite playerChest, Sprite playerShoulder)
    {
        playerHeadSpriteRenderer.sprite = playerHead;
        playerChestSpriteRenderer.sprite = playerChest;
        playerShoulderSpriteRenderer_1.sprite = playerShoulder;
        playerShoulderSpriteRenderer_2.sprite = playerShoulder;

    }

    public void ShowPlayer()
    {
        playerObject.SetActive(true);
    }

    public void StartMoving()
    {
        foreach (Animator anim in feetAnimators)
        {
            anim.enabled = true;
        }
    }

    public void StopMoving()
    {
        foreach (Animator anim in feetAnimators)
        {
            anim.enabled = false;
        }
    }

    private const float LAZER_DISTANCE = 100f;

    public void Activate()
    {        
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, LAZER_DISTANCE, CommonLayerMasks.Entities);

        NoseMobileObject.SetActive(true);
        InteractorObject.SetActive(true);

        StartCoroutine(FireLazers(enemiesHit));
    }

    private IEnumerator FireLazers(Collider2D[] enteties)
    {
        EyeLazers[0].SetPosition(0, EyeLazers[0].transform.position);
        EyeLazers[1].SetPosition(0, EyeLazers[1].transform.position);

        int currentEye = 0;

        for (int i = 0; i < enteties.Length; i++)
        {
            if (enteties[i] == null) //if the enemy got killed
            {
                continue;
            }

            EyeLazers[currentEye].enabled = true;

            EyeLazers[currentEye].SetPosition(1, enteties[i].transform.position);

            Destroy(enteties[i].gameObject);
            Instantiate(ExplosionPrefab, enteties[i].transform.position, Quaternion.identity);

            currentEye = currentEye * -1 + 1; //flips between 0 and 1

            EyeLazers[currentEye].enabled = false;

            yield return new WaitForSeconds(0.25f);
        }

        EyeLazers[0].gameObject.SetActive(false);
        EyeLazers[1].gameObject.SetActive(false);

        finishedWithKilling = true;
    }

    public void InitiateLevelSwapSequence()
    {        
        ShowPlayer();

        PlayerInputHandler._instance.DisableAll();

        InteractTextController._instance.DisableText();

        StartCoroutine(DestroyBlockage());        
    }

    private IEnumerator DestroyBlockage()
    {
        while (finishedWithKilling == false)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        int direction = (int)Mathf.Sign(tunnelBlockageObject.transform.position.x - transform.position.x);

        if (direction > 0)
        {
            CustomMath.FlipTransform(NoseMobileObject.transform);                       
        }

        yield return new WaitForSeconds(1f);

        EyeLazers[0].SetPosition(0, EyeLazers[0].transform.position);
        EyeLazers[1].SetPosition(0, EyeLazers[1].transform.position);

        EyeLazers[0].gameObject.SetActive(true);
        EyeLazers[1].gameObject.SetActive(true);

        EyeLazers[0].enabled = true;
        EyeLazers[1].enabled = true;

        Vector3 targetPosition = tunnelBlockageObject.transform.position + new Vector3(-0.5f, 4, 0);

        EyeLazers[0].SetPosition(1, targetPosition);
        EyeLazers[1].SetPosition(1, targetPosition);

        yield return new WaitForSeconds(1f);

        EyeLazers[0].gameObject.SetActive(false);
        EyeLazers[1].gameObject.SetActive(false);

        Instantiate(ExplosionPrefab, targetPosition, Quaternion.identity);
        Destroy(tunnelBlockageObject.gameObject);

        StartMoving();

        StartCoroutine(MoveThroughTunnel(direction));
    }

    private IEnumerator MoveThroughTunnel(int direction)
    {
        float walkingDistance = walkOffDistance;

        while (true)
        {
            rigidbody.velocity = transform.right * Speed;

            walkingDistance -= Time.deltaTime * Speed;

            if(walkingDistance <= 0)
            {
                SceneLoader.NextLevel();

                yield break;
            }

            yield return null;
        }
    }
}
