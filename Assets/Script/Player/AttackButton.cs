using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerAttack playerAttack;
    public PlayerAttack playerAttack2;
    public PlayerStatus playerStatus; // Add reference to PlayerStatus
    public PlayerStatus playerStatus2;

    bool isHold;

    private void Awake()
    {
        if (playerAttack == null)
        {
            Debug.LogError("Player Attack is null");
        }

        if (playerAttack2 == null)
        {
            Debug.LogError("Player Attack2 is null");
        }

        if (playerStatus == null)
        {
            Debug.LogError("Player Status is null");
        }
    }

    private void FixedUpdate()
    {
        // Check if player is alive before attacking
        if (isHold && playerStatus != null && !playerStatus.dead)
        {
            playerAttack.AttackCheck();
            if (playerAttack2 != null) // Check if playerAttack2 is assigned
            {
                playerAttack2.AttackCheck();
            }
        }

        if (isHold && playerStatus2 != null && !playerStatus2.dead)
        {
            playerAttack2.AttackCheck();
            if (playerAttack != null) // Check if playerAttack is assigned
            {
                playerAttack.AttackCheck();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHold = true;
        Debug.Log("Hold");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHold = false;
    }
}
