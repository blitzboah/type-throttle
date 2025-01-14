using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField] private Text timerText;
    [SerializeField] private float currentTime;
    [SerializeField] private TypingSpeedCalculator typingSpeedCalculator;
    [SerializeField] private GameObject typingPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text finalWpmText;
    [SerializeField] private Image winImage;
    [SerializeField] private Image loseImage;
    [SerializeField] private CarController carController;
    [SerializeField] private BotController botController;
    private int finalWpm;
    private bool isRunning = false;
    float difference = 0f;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void Start()
    {
        currentTime = BotSpeedManager.totalTime;
    }
    void Update()
    {
        if (isRunning)
        {
            //Debug.Log(currentTime);
            currentTime -= Time.deltaTime;

            if(currentTime <= 0 )
            {
                finalWpm = (int) typingSpeedCalculator.GetWpm();
                difference = carController.GetDistanceAlongSpline() - botController.GetDistanceAlongSpline();
                //Debug.Log(difference);
                currentTime = 0f;
                isRunning = false;
                EndGame();
            }

            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        timerText.text = currentTime.ToString("0") + "s";
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    private void EndGame()
    {
        audioManager.Play("game-over-theme");
        FindObjectOfType<PlayerCarAudio>().StopEngineSound();
        FindObjectOfType<BotCarAudio>().StopEngineSound();
        typingPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(true);
        if(difference > 0f && finalWpm > BotSpeedManager.botSpeed)
        {
            loseImage.gameObject.SetActive(false);
            winImage.gameObject.SetActive(true);
        }
        else
        {
            winImage.gameObject.SetActive(false);
            loseImage.gameObject.SetActive(true);
        }
        finalWpmText.text = finalWpm.ToString();
        //Debug.Log("owarida");
    }

    public void Reset()
    {
        audioManager.Play("click-sound");
        audioManager.Stop("game-over-theme");
        SceneManager.LoadScene("main");
    }

    public void QuitToMenu()
    {
        audioManager.Play("click-sound");
        audioManager.Stop("game-over-theme");
        SceneManager.LoadScene("main menu");
    }
}
