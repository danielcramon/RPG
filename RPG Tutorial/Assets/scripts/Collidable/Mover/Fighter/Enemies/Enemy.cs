using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    //Experience
    public int xpValue = 1;

    // Logic
    public float triggerLength = 0.25f;
    public float chaseLength = 1.0f;
    protected bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;
    public bool isBoss = false;

    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
        if (!isBoss)
        {

            this.hitpoint = GameManager.instance.enemyHitpoints[GameManager.instance.getCurrentLevel() - 1];
            this.maxHitpoint = GameManager.instance.enemyHitpoints[GameManager.instance.getCurrentLevel() - 1];
        }
        else
        {
            this.hitpoint = GameManager.instance.bossHitpoints[GameManager.instance.getCurrentLevel() - 1];
            this.maxHitpoint = GameManager.instance.hitpoints[GameManager.instance.getCurrentLevel() - 1];
        }
        
    }

    private void FixedUpdate()
    {
        //Is the player in range?
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
        {
            if(Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
            {
                chasing = true;
            }
            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    updateMotor((playerTransform.position - transform.position).normalized);
                }
            }
            else
            {
                updateMotor(startingPosition - transform.position);
            }
        }
        else
        {
            updateMotor(startingPosition - transform.position);
            chasing = false;
        }

        // Check for overlaps
        collidingWithPlayer = false;
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
            {
                continue;
            }

            if(hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }

            // The array is not cleaned up, so we do it ourself
            hits[i] = null;
        }

        updateMotor(Vector3.zero);
    }

    protected override void death()
    {
        Destroy(gameObject);
        GameManager.instance.grantXp(xpValue);
        GameManager.instance.showText("+" + xpValue + "XP", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f, false);
    }
}
