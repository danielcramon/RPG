using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Fighter
{
    private int pesosAmount = 2;
    protected override void death()
    {
        Destroy(gameObject);
        GameManager.instance.pesos += pesosAmount;
        GameManager.instance.showText("+" + pesosAmount + "pesos!", 25, Color.yellow, transform.position + new Vector3(0, 0.16f, 0), Vector3.up * 25, 1.5f, false);
    }
}
