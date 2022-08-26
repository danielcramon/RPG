using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    public bool canMove;
    public bool isDead = false;
    public int stamina = 10;
    public int strength = 10;
    public int talentPoints = 0;
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
        updateHitpoints();
        hitpoint = maxHitpoint;
        talentPoints++;
        StartCoroutine(levelUpText());
    }

    IEnumerator levelUpText()
    {
        GameManager.instance.showText("LEVEL UP!", 25, Color.yellow, transform.position + new Vector3(0, 0.24f, 0), Vector3.up * 40, 1.5f, false);
        yield return new WaitForSeconds(2);
        GameManager.instance.showText("You gained a talent point. Click on the talent tree in the buttom left corner!", 15, Color.white, transform.position + new Vector3(0, 0.24f, 0), Vector3.zero, 4.0f, false);
    }
    public void setLevel()
    {
        float tempHitpoints = ((float)GameManager.instance.hitpoints[GameManager.instance.getCurrentLevel() - 1] / 100) * stamina;
        maxHitpoint = (int)tempHitpoints;
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
        isDead = true;
    }
    public void respawn()
    {
        hitpoint = maxHitpoint;
        GameManager.instance.onHitpointChange();
        canMove = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
        isDead = false;
    }

    private void updateHitpoints ()
    {
        if (hitpoint == maxHitpoint)
        {
            float tempHitpoints = ((float)GameManager.instance.hitpoints[GameManager.instance.getCurrentLevel() - 1] / 100) * stamina;
            maxHitpoint = (int)tempHitpoints;
            hitpoint = maxHitpoint;

        }
        else
        {
            float tempHitpoints = ((float)GameManager.instance.hitpoints[GameManager.instance.getCurrentLevel() - 1] / 100) * stamina;
            maxHitpoint = (int)tempHitpoints;
            if (hitpoint > maxHitpoint)
            {
                hitpoint = maxHitpoint;
            }
        }

    }
    public void addStamina()
    {
        stamina++;
        talentPoints--;
        updateHitpoints();
        GameManager.instance.onHitpointChange();
    }
    public void addStrength()
    {
        strength++;
        talentPoints--;
    }
    public void removeStamina()
    {
        stamina--;
        talentPoints++;
        updateHitpoints();
        GameManager.instance.onHitpointChange();
    }
    public void removeStrength()
    {
        strength--;
        talentPoints++;
    }
}
