using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public float[] fireballSpeeds = { 2.5f, -2.5f };
    public float distance = 0.25f;
    public Transform[] fireballs;
    private void Update()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            fireballs[i].position = transform.position + new Vector3(-Mathf.Cos(Time.time * fireballSpeeds[i]) * distance, Mathf.Sin(Time.time * fireballSpeeds[i]) * distance, 0);
        }

    }
}
