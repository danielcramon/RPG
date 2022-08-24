using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud.gameObject);
            Destroy(menu.gameObject);
            return;
        }
        instance = this;
        SceneManager.sceneLoaded += onSceneLoaded;
        startMenuAnim.SetTrigger("Show");
        player.canMove = false;
        startMenuObject.isActive = true;
    }

    // Ressources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;
    public List<int> hitpoints;
    public List<int> enemyHitpoints;
    public List<int> bossHitpoints;
    private bool gameLoaded = false;
    public List<int> bossDamages;
    public List<int> enemyDamages;


    // References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public GameObject hud;
    public GameObject menu;
    public Animator deathMenuAnim;
    public Animator startMenuAnim;
    public Animator controlsMenuAnim;
    public Animator optionsMenuAnim;
    public Animator mainMenuAnim;
    public StartMenu startMenuObject;
    public OptionsMenu optionsMenuObject;
    public ControlsMenu controlsMenuObject;
    public CharacterMenu characterMenuObject;

    // Logic
    public int pesos;
    public int experience;

    //floating text
    public void showText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration, bool locked)
    {
        if (!locked)
        {
            floatingTextManager.show(msg, fontSize, color, position, motion, duration);
        }
        else
        {
            floatingTextManager.showLocked(msg, fontSize, color, position, motion, duration);
        }
    }

    // Upgrade Weapon
    public bool tryUpgradeWeapon()
    {
        // is the weapon max level?
        if (weaponPrices.Count <= weapon.weaponLevel)
        {
            return false;
        }

        if (pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.upgradeWeapon();
            return true;
        }

        return false;
    }

    // Hitpoint Bar
    public void onHitpointChange()
    {
        float ratio = (float)player.hitpoint / player.maxHitpoint;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }

    // Experience System
    public int getCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while(experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count)
            {
                return r;
            }
        }

        return r;
    }
    public int getXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while(r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }
    public void grantXp(int xp)
    {
        int currLevel = getCurrentLevel();
        experience += xp;
        if (currLevel < getCurrentLevel())
        {
            onLevelUp();
        }

    }
    public void onLevelUp()
    {
        player.onLevelUp();
        onHitpointChange();
    }

    //On Scene Loaded
    public void onSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("Spawnpoint").transform.position;
    }

    //Death Menu and Respawn
    public void respawn()
    {
        deathMenuAnim.SetTrigger("Hide");
        SceneManager.LoadScene("Main");
        player.respawn();
    }

    public void playGame()
    {
        if (!gameLoaded)
        {
            loadState();
        }
        startMenuAnim.SetTrigger("Hide");
        player.canMove = true;
        startMenuObject.isActive = false;
    }

    public void showOptions()
    {
        startMenuAnim.SetTrigger("Hide");
        optionsMenuAnim.SetTrigger("Show");
    }

    public void showControls()
    {
        startMenuAnim.SetTrigger("Hide");
        controlsMenuAnim.SetTrigger("Show");
    }

    public void returnFromOptions() 
    {
        startMenuAnim.SetTrigger("Show");
        optionsMenuAnim.SetTrigger("Hide");
    }
    public void returnFromControls()
    {
        startMenuAnim.SetTrigger("Show");
        controlsMenuAnim.SetTrigger("Hide");
    }
    public void showStartMenu()
    {
        if(!player.canMove)
        {
            deathMenuAnim.SetTrigger("Hide");
            startMenuAnim.SetTrigger("Show");
            SceneManager.LoadScene("Main");
            player.respawn();
            player.canMove = false;
            startMenuObject.isActive = true;
        }
        else
        {
            startMenuAnim.SetTrigger("Show");
            startMenuObject.isActive = true;
            if (characterMenuObject.isActive)
            {
                mainMenuAnim.SetTrigger("Hide");
                characterMenuObject.isActive = false;
            }
            if (optionsMenuObject.isActive)
            {
                optionsMenuAnim.SetTrigger("Hide");
                optionsMenuObject.isActive = false;
            }
            if (controlsMenuObject.isActive)
            {
                controlsMenuAnim.SetTrigger("Hide");
                controlsMenuObject.isActive = false;
            }
        }
    }

    public void showCharacterMenu()
    {
        mainMenuAnim.SetTrigger("Show");
        characterMenuObject.updateMenu();
        characterMenuObject.isActive = true;
        Debug.Log("Why am i here");
        if (startMenuObject.isActive){
            startMenuAnim.SetTrigger("Hide");
            startMenuObject.isActive = false;
        }
        if (optionsMenuObject.isActive)
        {
            optionsMenuAnim.SetTrigger("Hide");
            startMenuObject.isActive = false;
        }
        if (controlsMenuObject.isActive)
        {
            controlsMenuAnim.SetTrigger("Hide");
            startMenuObject.isActive = false;
        }
    }

    public void hideCharacterMenu()
    {
        mainMenuAnim.SetTrigger("Hide");
        characterMenuObject.isActive = false;
    }

    public void quitGame()
    {
        Application.Quit();
    }


    // Save/Load State
    public void saveState()
    {
        string s = "";

        s += "0" + "|";
        s += pesos.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
    }
    public void loadState()
    {

        if (!PlayerPrefs.HasKey("SaveState"))
        {
            return;
        }
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // Change Player skin
        pesos = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        player.onLevelUp();
        // Change Weapon level
        weapon.setWeaponLevel(int.Parse(data[3]));
        showText("The chest in the buttom left cornor is the menu button, press it to open the menu.", 15, Color.white, transform.position + new Vector3(0, 0.24f, 0), Vector3.zero, 6.0f, false);
        gameLoaded = true;
    }

}
