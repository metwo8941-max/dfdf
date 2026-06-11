using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class FlightSession : MonoBehaviour
{
    public static FlightSession Instance { get; private set; }

    [SerializeField] private RocketController rocket;
    [SerializeField] private GateSpawner gateSpawner;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject launchButton;
    [SerializeField] private GameObject missionFailedBanner;
    [SerializeField] private GameObject touchHint;

    public int score { get; private set; } = 0;

    private bool isRunActive;
    private bool hasPlayedOnce;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        SetPaused();
        ShowIdleUi(true, false);
    }

    private void Update()
    {
        if (isRunActive || !HasPrimaryPressStarted()) {
            return;
        }

        StartRun();
    }

    public void SetPaused()
    {
        Time.timeScale = 0f;
        rocket.enabled = false;
        isRunActive = false;
    }

    public void StartRun()
    {
        hasPlayedOnce = true;
        isRunActive = true;
        score = 0;
        scoreText.text = score.ToString();

        ShowIdleUi(false, false);

        Time.timeScale = 1f;
        rocket.enabled = true;

        EnergyGate[] gates = FindObjectsOfType<EnergyGate>();

        for (int i = 0; i < gates.Length; i++) {
            Destroy(gates[i].gameObject);
        }
    }

    public void EndRun()
    {
        SetPaused();
        ShowIdleUi(true, true);
    }

    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    private void ShowIdleUi(bool showLaunchButton, bool showFailedBanner)
    {
        launchButton.SetActive(showLaunchButton);
        missionFailedBanner.SetActive(showFailedBanner);

        if (touchHint == null) {
            return;
        }

        touchHint.SetActive(true);

        Text hintText = touchHint.GetComponent<Text>();

        if (hintText != null) {
            hintText.text = hasPlayedOnce && showFailedBanner ? "Tap to retry" : "Tap anywhere to fly";
        }
    }

    private bool HasPrimaryPressStarted()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            return true;
        }

        if (Input.GetMouseButtonDown(0)) {
            return EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject();
        }

        if (Input.touchCount == 0) {
            return false;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began) {
            return false;
        }

        return EventSystem.current == null || !EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }

}
