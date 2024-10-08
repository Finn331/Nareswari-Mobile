using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pegas : MonoBehaviour
{
    [Header("Pegas Requirement")]
    public bool haveKey;

    [Header("Pegas Text")]
    [SerializeField] private GameObject pegasText;

    [Header("Pegas Button")]
    [SerializeField] private GameObject pegasButton;
    [SerializeField] Button interactionButton;

    [Header("Script Reference")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerController playerController2;

    [SerializeField] private Animator penjaraAnim;
    [SerializeField] Animator anim;

    private bool playerInTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        // Dapatkan referensi Animator dari child bernama "GFX"
        Transform gfxTransform = transform.Find("GFX");
        if (gfxTransform != null)
        {
            anim = gfxTransform.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("Child dengan nama 'GFX' tidak ditemukan.");
        }

        interactionButton.onClick.AddListener(OnInteractionButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger)
        {
            KeyChecking();
        }

        // Check for touch input using Raycast2D
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                {
                    PerformAction();
                }
            }
        }
    }

    private void KeyChecking()
    {
        // Handle keyboard input (for testing or if running on a non-touch device)
        if (Input.GetKeyDown(KeyCode.E))
        {
            PerformAction();
        }
    }

    private void PerformAction()
    {
        if (haveKey)
        {
            anim.SetTrigger("open");
            penjaraAnim.SetTrigger("openPenjara");
            playerController.speed = 0;
            playerController2.speed = 0;
            playerController.enabled = false;
            playerController2.enabled = false;
        }
        else
        {
            ShowPegasText();
        }
    }

    private void ShowPegasText()
    {
        pegasText.SetActive(true);
        LeanTween.scale(pegasText, Vector3.one, 1f)
                 .setEase(LeanTweenType.easeOutBack)
                 .setOnComplete(() =>
                 {
                     LeanTween.scale(pegasText, Vector3.zero, 1.5f)
                              .setEase(LeanTweenType.easeOutBack)
                              .setOnComplete(() => pegasText.SetActive(false));
                 });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pegasButton != null)
        {
            playerInTrigger = true;
            pegasButton.SetActive(true);
            LeanTween.scale(pegasButton, Vector3.one, 1f).setEase(LeanTweenType.easeOutBack);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && pegasButton != null)
        {
            playerInTrigger = false;
            LeanTween.scale(pegasButton, Vector3.zero, 0.3f)
                     .setEase(LeanTweenType.easeOutBack)
                     .setOnComplete(() => pegasButton.SetActive(false));
        }
    }

    private void OnInteractionButtonClicked()
    {
        if (playerInTrigger)
        {
            PerformAction();
        }
    }
}
