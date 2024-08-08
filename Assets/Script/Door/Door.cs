using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public GameObject interactButton;
    public GameObject teleportTargetB;
    public GameObject cameraObject;
    public GameObject cameraObject2;
    [SerializeField] Button interactionButton;

    private bool canTeleport = false;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        interactionButton.onClick.AddListener(OnTeleportButtonClicked);
    }

    // Teleport function
    private void TeleportToB()
    {
        if (teleportTargetB != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = teleportTargetB.transform.position;
            }
            else
            {
                Debug.LogWarning("Player not found!");
            }
        }
        else
        {
            Debug.LogWarning("Teleport target B is not assigned!");
        }
    }

    private void MoveCamera()
    {
        cameraObject.SetActive(false);
        cameraObject2.SetActive(true);

        if (cameraObject == null)
        {
            Debug.LogWarning("Camera object not assigned!");
        }

        if (cameraObject2 == null)
        {
            Debug.LogWarning("Camera object not assigned!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactButton.SetActive(true);
            LeanTween.scale(interactButton, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeOutBack);
            canTeleport = true;
            anim.SetTrigger("open");
            anim.SetBool("isOpen", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactButton.SetActive(false);
            LeanTween.scale(interactButton, new Vector3(0, 0, 0), 1f).setEase(LeanTweenType.easeOutBack);
            canTeleport = false;
            anim.SetBool("isOpen", false);
        }
    }

    private void OnTeleportButtonClicked()
    {
        if (canTeleport)
        {
            TeleportToB();
            MoveCamera();
            Debug.Log("Teleported to " + name);
        }
    }
}
