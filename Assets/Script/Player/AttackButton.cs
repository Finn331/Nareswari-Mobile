using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerAttack playerAttack;
    public PlayerAttack playerAttack2;
    bool isHold;
    private void Awake()
    {
        if (playerAttack == null)
        {
            Debug.LogError("Player Attack is null");
        }

        if (playerAttack2 == null)
        {
            Debug.LogError("Player Attack is null");
        }        
    }

    private void FixedUpdate()
    {
        if (isHold == true)
        {
            playerAttack.AttackCheck();
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
