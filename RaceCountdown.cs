using UnityEngine;
using UnityEngine.UI;

public class RaceCountdown : MonoBehaviour
{
    [SerializeField] private float countdown= 3f;
    [SerializeField] private Text countdownText;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BotCarAudio botCarAudio;
    [SerializeField] private PlayerCarAudio playerCarAudio;

    public static bool raceStarted { get; private set; } = false;
    private float countdownTimer;

    private void Start()
    {
        countdownTimer = countdown;
        raceStarted = false;
        Invoke(nameof(PlayReadySound), 0.1f); // Delay to ensure setup is complete
        StartCountdown();
    }

    private void PlayReadySound()
    {
        if (botCarAudio != null && playerCarAudio != null)
        {
            botCarAudio.PlayReadySound();
            playerCarAudio.PlayReadySound();
        }
        else
        {
            Debug.LogError("reference is missing!");
        }
    }


    void StartCountdown()
    {
        InvokeRepeating(nameof(UpdateCountdown), 0f, 1f);
    }

    void UpdateCountdown()
    {
        if(countdownTimer > 0f)
        {
            if (countdownTimer == 1f)
            {
                botCarAudio.PlayEngineSound();
            }
            countdownText.text = countdownTimer.ToString("0");
            countdownTimer--;
        }
        else
        {
            countdownText.text = "TYPE!";
            raceStarted = true;

            if(gameManager != null)
            {
                gameManager.StartTimer();
            }
            CancelInvoke(nameof(UpdateCountdown));
            Invoke(nameof(HideText), 1f);
        }
    }

    void HideText()
    {
        countdownText.gameObject.SetActive(false);
    }
}
