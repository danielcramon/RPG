using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText 
{
    public bool active;
    public GameObject go;
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastSown;

    public void show()
    {
        active = true;
        lastSown = Time.time;
        go.SetActive(active);
    }

    public void hide()
    {
        active = false;
        go.SetActive(active); 
    }

    public void updateFloatingText()
    {
        if (!active)
        {
            return;
        }

        if(Time.time - lastSown > duration)
        {
            hide();
        }

        go.transform.position += motion * Time.deltaTime;
    }
}
