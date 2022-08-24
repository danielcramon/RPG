using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Animator startMenuAnim;
    public Animator characterMenuAnim;
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (!GameManager.instance.startMenuObject.isActive)
            {
                GameManager.instance.showStartMenu();
            }
            else
            {
                GameManager.instance.playGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!GameManager.instance.characterMenuObject.isActive)
            {
                GameManager.instance.showCharacterMenu();
            }
            else
            {
                GameManager.instance.hideCharacterMenu();
            }
        }
    }
}
