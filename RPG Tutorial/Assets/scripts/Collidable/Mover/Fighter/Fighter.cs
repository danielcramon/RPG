using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    //public fields
    public int hitpoint = 10;
    public int maxHitpoint = 10;
    public float pushRecoverySpeed = 0.2f;

    // Immunity
    protected float immuneTime = 1.0f;
    protected float lastImmune;

    //Push
    protected Vector3 pushDirection;

    //All fighters can RecieveDamage / Die
    protected virtual void receiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            lastImmune = Time.time;
            hitpoint -= dmg.damageAmmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

            GameManager.instance.showText(dmg.damageAmmount.ToString(), 25, Color.red, transform.position, Vector3.up * 25, 0.5f, false);

            if (hitpoint <= 0)
            {
                hitpoint = 0;
                death();
            }
        }
    }

    protected virtual void death()
    {

    }
}
