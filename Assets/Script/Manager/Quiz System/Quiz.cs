using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    public GameObject quizPanel;
    public LevelsManager levelsManager;
    [SerializeField] LevelManager2 levelManager2;
    [SerializeField] LevelManager3 levelManager3;

    public PlayerController playerController;
    public PlayerController playerController2;
    [SerializeField] private GameObject buttonInteract;
    [SerializeField] Button interactionButton;
    [SerializeField] GameObject joystick;
    private bool playerInRange = false;

    private void Awake()
    {
        // Registering event handler for the interactionButton
        interactionButton.onClick.AddListener(OnInteractionButtonClicked);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            buttonInteract.SetActive(true);
            LeanTween.scale(buttonInteract, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeOutBack);
            playerInRange = true;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && buttonInteract.activeSelf)
        {
            OpenQuizPanel();
        }

        HandleTouchInput();
    }

    private void HandleTouchInput()
    {
        if (playerInRange && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                {
                    OpenQuizPanel();
                }
            }
        }
    }

    private void OnInteractionButtonClicked()
    {
        if (playerInRange && buttonInteract.activeSelf)
        {
            OpenQuizPanel();
        }
    }

    private void OpenQuizPanel()
    {
        quizPanel.SetActive(true);
        Time.timeScale = 0;
        if (levelsManager != null)
        {
            levelsManager.isPaused = true;
        }

        if (levelManager2 != null)
        {
            levelManager2.isPaused = true;
        }

        if (levelManager3 != null)
        {
            levelManager3.isPaused = true;
        }

        joystick.SetActive(false);
        playerController.enabled = false;
        playerController2.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        buttonInteract.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            buttonInteract.SetActive(false);
            LeanTween.scale(buttonInteract, new Vector3(0, 0, 0), 1f).setEase(LeanTweenType.easeOutBack);
            playerInRange = false;
        }
    }

    private void OnDestroy()
    {
        joystick.SetActive(true);
    }
}
