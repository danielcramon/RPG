﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // Damage struct
    public int[] damagePoint = { 1, 2, 4, 7, 11, 17, 25 };
    public float[] pushForce = { 2.0f, 2.2f, 2.4f, 2.6f, 2.8f, 3.0f, 3.5f};

    //Upgrade
    public int weaponLevel = 0;
    public SpriteRenderer spriteRenderer;

    // Swing
    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing;


    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Time.time - lastSwing > cooldown && GameManager.instance.player.canMove)
            {
                lastSwing = Time.time;
                swing();
            }
        }
    }

    protected override void onCollide(Collider2D coll)
    {
        if(coll.tag == "Fighter")
        {
            if (coll.name != "Player")
            {
                float tempDamage = ((float)damagePoint[weaponLevel] / 100) * GameManager.instance.player.strength;
                // Create a new damage object, then we'll send it to the fighter we've hit
                Damage dmg = new Damage
                {
                    damageAmmount = (int) tempDamage,
                    origin = transform.position,
                    pushForce = pushForce[weaponLevel]
                };

                coll.SendMessage("receiveDamage", dmg);
            }
        }
    }

    private void swing()
    {
        anim.SetTrigger("Swing");
    }

    public void upgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];

        // Change stats
    }

    public void setWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
}
