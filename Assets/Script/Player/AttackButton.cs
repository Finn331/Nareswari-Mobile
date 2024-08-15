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

    [Header("Player Gameobject Reference")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject player2;

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
        
        // Check if player 1 is alive before attacking
        if (isHold && playerStatus != null && !playerStatus.dead && playerAttack != null && player.activeInHierarchy)
        {
            playerAttack.AttackCheck();
        }

        // Check if player 2 is alive before attacking
        if (isHold && playerStatus2 != null && !playerStatus2.dead && playerAttack2 != null && player2.activeInHierarchy)
        {
            playerAttack2.AttackCheck();
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
