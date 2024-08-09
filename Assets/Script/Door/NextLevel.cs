using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel : MonoBehaviour
{
    [Header("Score Requirement")]
    [SerializeField] int scoreRequirement;
    [SerializeField] string nextLevelName;
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("End Level Panel")]
    [SerializeField] GameObject endLevelPanel;
    [SerializeField] GameObject endLevelHolder;
    [SerializeField] TextMeshProUGUI endLevelScoreText;

    [Header("Setup")]
    [SerializeField] int levelAdd; // using an increment like 1 or 0
    [SerializeField] GameObject interactButton;
    [SerializeField] GameObject loadingCanvas;
    [SerializeField] Slider loadingSlider;
    [SerializeField] Button interactionButton;
    [SerializeField] GameObject[] objectToDisable;
    [SerializeField] DynamicJoystick joystick;
    private int currScore;
    private Animator anim;
    private bool canTeleport = false;

    private void Awake()
    {
        interactionButton.onClick.AddListener(OpenMenuButton);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
                if (playerStatus.score >= scoreRequirement)
                {
                    anim.SetTrigger("open");
                    interactButton.SetActive(true);
                    LeanTween.scale(interactButton, new Vector3(1, 1, 1), 1f).setEase(LeanTweenType.easeOutBack);
                    canTeleport = true;
                }

                if (playerStatus.score < scoreRequirement)
                {
                    scoreText.gameObject.SetActive(true);
                    LeanTween.scale(scoreText.gameObject, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.easeOutBack);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interactButton.SetActive(false);
            LeanTween.scale(interactButton, new Vector3(0, 0, 0), 1f).setEase(LeanTweenType.easeOutBack);
            canTeleport = false;

            GameObject player = GameObject.FindWithTag("Player");
            PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
            if (playerStatus.score >= scoreRequirement)
            {
                anim.SetTrigger("close");
            }

            if (playerStatus.score < scoreRequirement)
            {
                LeanTween.scale(scoreText.gameObject, new Vector3(0, 0, 0), 0.5f).setEase(LeanTweenType.easeOutBack).setOnComplete(() => scoreText.gameObject.SetActive(false));
            }
        }
    }

    private void Update()
    {
        if (canTeleport && Input.GetKeyDown(KeyCode.E))
        {
            OpenMenu();
        }

        HandleTouchInput();

        currScore = FindObjectOfType<PlayerStatus>().score;
    }

    private void HandleTouchInput()
    {
        if (canTeleport && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main != null ? Camera.main.ScreenToWorldPoint(touch.position) : Vector2.zero;

                // Check if the Camera.main was found
                if (Camera.main == null)
                {
                    Debug.LogError("Main Camera is not found. Make sure the camera is tagged as 'MainCamera'.");
                    return;
                }

                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                // Check if a collider was hit
                if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                {
                    OpenMenu();
                }
            }
        }
    }

    private void OpenMenuButton()
    {
        if (canTeleport)
        {
            OpenMenu();
        }
    }

    void OpenMenu()
    {
        endLevelPanel.SetActive(true);
        endLevelHolder.SetActive(true);
        endLevelScoreText.text = "" + FindObjectOfType<PlayerStatus>().score;
        joystick.enabled = false;

        LeanTween.scale(endLevelHolder, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.easeOutBack).setIgnoreTimeScale(true);

        if (levelAdd == 2)
        {
            SaveManager.instance.level2Unlocked = true;
            SaveManager.instance.Save();
        }
        else if (levelAdd == 3)
        {
            SaveManager.instance.level3Unlocked = true;
            SaveManager.instance.Save();
        }

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void NextLevelBtn()
    {
        SaveCurrentScore();

        Time.timeScale = 1;
        LoadingLevelBtn(nextLevelName);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (levelAdd == 2)
        {
            SaveManager.instance.level2Unlocked = true;
            SaveManager.instance.Save();
        }
        else if (levelAdd == 3)
        {
            SaveManager.instance.level3Unlocked = true;
            SaveManager.instance.Save();
        }
    }

    public void MainMenuBtn()
    {
        SaveCurrentScore();
        if (levelAdd == 2)
        {
            SaveManager.instance.level2Unlocked = true;
            SaveManager.instance.Save();
        }
        else if (levelAdd == 3)
        {
            SaveManager.instance.level3Unlocked = true;
            SaveManager.instance.Save();
        }
        Time.timeScale = 1;
        LoadingLevelBtn("Mainmenu");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SaveCurrentScore()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            SaveManager.instance.level1Score = currScore;
        }
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            SaveManager.instance.level2Score = currScore;
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            SaveManager.instance.level3Score = currScore;
        }
        //SaveManager.instance.Save();
    }

    public void LoadingLevelBtn(string nextLevelName)
    {
        loadingCanvas.SetActive(true);
        StartCoroutine(LoadLevel(nextLevelName));
    }

    IEnumerator LoadLevel(string nextLevelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextLevelName);
        Time.timeScale = 1;
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;

            yield return null;
        }
    }
}
