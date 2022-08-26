using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TalentMenu : MonoBehaviour
{
    public bool isActive = false;
    public Text strength, stamina, talentpoints;
    public int strPoints = 0;
    public int stamPoints = 0;


    public void onStaminaArrowClick(bool up)
    {
        if (up)
        {
            if (GameManager.instance.player.talentPoints != 0)
            {
                stamPoints++;
                GameManager.instance.player.addStamina();
            }
        }
        else
        {
            if (stamPoints != 0)
            {
                stamPoints--;
                GameManager.instance.player.removeStamina();
            }
        }
        updateMenu();

    }
    public void onStrengthArrowClick(bool up)
    {
        if (up)
        {
            if (GameManager.instance.player.talentPoints != 0)
            {
                strPoints++;
                GameManager.instance.player.addStrength();
            }
        }
        else
        {
            if (strPoints != 0)
            {
                strPoints--;
                GameManager.instance.player.removeStrength();
            }
        }
        updateMenu();
    }
    public void updateMenu()
    {
        talentpoints.text = GameManager.instance.player.talentPoints.ToString();
        strength.text = strPoints.ToString();
        stamina.text = stamPoints.ToString();
    }
}
