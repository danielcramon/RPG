using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : Collidable
{
    private float healCooldown = 15.0f;
    private float lastHeal;
    private float textCooldown = 1.5f;
    private float lastText;
    private float checkCooldown = 0.1f;
    private float lastCheck;

    protected override void Start()
    {
        base.Start();
        lastCheck = -checkCooldown;
        lastText = -textCooldown;
        lastHeal = -healCooldown;
    }
    protected override void onCollide(Collider2D coll)
    {
        if (coll.name != "Player")
        {
            return;
        }
        if(Time.time - lastCheck > checkCooldown)
        {
            lastCheck = Time.time;
            if (Time.time - lastHeal > healCooldown)
            {
                lastHeal = Time.time;
                lastText = Time.time;
                float maxHitpoint = (float)GameManager.instance.player.maxHitpoint;
                float healthRegen = maxHitpoint / 100 * 30;
                GameManager.instance.player.heal((int)healthRegen);
            }
            else
            {
                if (Time.time - lastText > textCooldown)
                {
                    lastText = Time.time;
                    float cooldown = healCooldown - (Time.time - lastHeal);
                    int intCooldown = (int)cooldown;
                    GameManager.instance.showText("You need to wait " + intCooldown.ToString() + " Seconds!", 25, Color.grey, transform.position, Vector3.up * 50, 1.0f, false);
                }
                
            }
        }
        
    }

}
