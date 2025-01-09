using UnityEngine;
using UnityEngine.UI;

public class RaceCountdown : MonoBehaviour
{
    [SerializeField] private float countdown= 3f;
    [SerializeField] private Text countdownText;

    public static bool raceStarted { get; private set; } = false;
    private float countdownTimer;

    private void Start()
    {
        countdownTimer = countdown;
        raceStarted = false;
        StartCountdown();
    }

    void StartCountdown()
    {
        InvokeRepeating(nameof(UpdateCountdown), 0f, 1f);
    }

    void UpdateCountdown()
    {
        if(countdownTimer > 0f)
        {
            countdownText.text = countdownTimer.ToString("0");
            countdownTimer--;
        }
        else
        {
            countdownText.text = "GO!";
            raceStarted = true;
            CancelInvoke(nameof(UpdateCountdown));
            Invoke(nameof(HideText), 1f);
        }
    }

    void HideText()
    {
        countdownText.gameObject.SetActive(false);
    }
}
