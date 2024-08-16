using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Script Reference")]
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerController playerController2;
    [SerializeField] PlayerStatus playerStatus; // Reference to PlayerStatus
    [SerializeField] PlayerStatus playerStatus2;

    [Header("Gameobject Reference")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject player2;

    private void Awake()
    {
        if (playerController == null)
        {
            Debug.LogError("Player Attack is null");
        }

        if (playerController2 == null)
        {
            Debug.LogError("Player Attack2 is null");
        }

        if (playerStatus == null)
        {
            Debug.LogError("Player Status is null");
        }

        if (playerStatus2 == null)
        {
            Debug.LogError("Player Status2 is null");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if player is alive before attacking
        if (playerStatus != null && !playerStatus.dead && player.activeInHierarchy)
        {
            playerController.Jump();
            
        }

        if (playerController2 != null && !playerStatus2.dead && player2.activeInHierarchy) // Check if playerAttack2 is assigned
        {
            playerController2.Jump();
        }
    }
}