using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    public float stamina = 80;
    private Image staminaBar;

    private bool staminaConsume, canRecover = true;
    void Awake()
    {
        staminaBar = GetComponent<Image>();
    }
    void Update()
    {
        staminaBar.fillAmount = stamina/80;

        if(staminaConsume)
        {
            stamina -= 15 * Time.deltaTime;
        }
        else
        {
            if(stamina < 80 && canRecover)
            {
                stamina += 15 * Time.deltaTime;
            }
            else if(stamina >= 80 && canRecover)
            {
               stamina = 80;
            }
        }
    }

    public void dashStamina()
    {
        if(stamina >= 25)
        {
            stamina -= 25;
        }
    }

    public void staminaOn()
    {
        staminaConsume = true;
    }

    public void staminaOff()
    {
        staminaConsume = false;
    }

    public void canRecoverOn()
    {
        canRecover = true;
    }

    public void canRecoverOff()
    {
        canRecover = false;
    }
}
