using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Collidable
{
    public string[] sceneNames;
    public bool introEnd;
    protected override void onCollide(Collider2D coll)
    {
        if (introEnd)
        {
            GameManager.instance.setIntroFinished();
        }
        if(coll.name == "Player")
        {
            GameManager.instance.saveState();
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
