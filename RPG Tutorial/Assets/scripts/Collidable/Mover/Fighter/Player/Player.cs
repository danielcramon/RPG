using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    public bool canMove;
    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void receiveDamage(Damage dmg)
    {
        if (!canMove)
        {
            return;
        }
        base.receiveDamage(dmg);
        GameManager.instance.onHitpointChange();
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (canMove)
        {
            updateMotor(new Vector3(x, y, 0));
        }
    }

    public void swapSprite(int skinId)
    {
       spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }
    public void onLevelUp()
    {
        maxHitpoint = GameManager.instance.hitpoints[GameManager.instance.getCurrentLevel() - 1];
        hitpoint = maxHitpoint;
    }
    public void heal(int healingAmmount)
    {

        if (hitpoint + healingAmmount > maxHitpoint)
        {
            GameManager.instance.showText("+" + (maxHitpoint - hitpoint).ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f, false);
            hitpoint = maxHitpoint;
            GameManager.instance.onHitpointChange();
        }
        else
        {
            hitpoint = hitpoint + healingAmmount;
            GameManager.instance.showText("+" + healingAmmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f, false);
            GameManager.instance.onHitpointChange();
        }

    }

    protected override void death()
    {
        GameManager.instance.deathMenuAnim.SetTrigger("Show");
        canMove = false;
    }
    public void respawn()
    {
        hitpoint = maxHitpoint;
        GameManager.instance.onHitpointChange();
        canMove = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
    }
}
