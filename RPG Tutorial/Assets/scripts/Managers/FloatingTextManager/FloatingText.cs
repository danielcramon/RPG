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
    public Vector3 origionalPosition;
    private bool locked;

    public void show()
    {
        active = true;
        lastSown = Time.time;
        go.SetActive(active);
        locked = false;

    }

    public void showLocked()
    {
        active = true;
        lastSown = Time.time;
        go.SetActive(active);
        locked = true;

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
        if (!locked)
        {
            go.transform.position += motion * Time.deltaTime;
        }
        else
        {
            go.transform.position = Camera.main.WorldToScreenPoint(origionalPosition);
        }
    }
}
