using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    // Damage
    public int damage = 1;
    public float pushForce = 5;
    public bool isBoss = false;

    protected override void Start()
    {
        base.Start();
        if (!isBoss)
        {

            damage = GameManager.instance.enemyDamages[GameManager.instance.getCurrentLevel() - 1];
        }
        else
        {
            damage = GameManager.instance.bossDamages[GameManager.instance.getCurrentLevel() - 1];
        }
    }

    protected override void onCollide(Collider2D coll)
    {
        if ( coll.tag == "Fighter" && coll.name == "Player")
        {
            // Create a new damage object, before sending it to the player
            Damage dmg = new Damage
            {
                damageAmmount = damage,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("receiveDamage", dmg);
        }
    }
}
