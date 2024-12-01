using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private bool activeState = false;

    public void ToggleState()
    {
        activeState = !activeState;
        gameObject.SetActive(activeState);
    }

    public bool IsActive()
    {
        return activeState;
    }

    public void TriggerPlayerDamage()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.SendMessage("TakeDamage", 1);
        }
    }

}
