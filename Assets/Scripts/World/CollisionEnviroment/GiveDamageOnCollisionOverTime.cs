using UnityEngine;

public class GiveDamageOnCollisionOverTime : MonoBehaviour
{
    /*public float timeBetweenDamage = 0.5f;
    public float Damage;

    float stayTime;

    GameObject lastCollision;

    Health HealthClass;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != lastCollision)
        {
            if ((CommonLayerMasks.HasHealth & (1 << collision.gameObject.layer)) > 0)
            {
                HealthClass = collision.gameObject.GetComponent<Health>();
                lastCollision = collision.gameObject;
            }
        }

        stayTime = timeBetweenDamage;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        stayTime += Time.deltaTime;

        if(stayTime >= timeBetweenDamage)
        {
            HealthClass.Damage(Damage, 0);
            stayTime = 0;
        }
    }*/
}
 