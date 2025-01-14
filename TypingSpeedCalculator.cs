using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TypingSpeedCalculator : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Text wpmText;
    [SerializeField] private Text wordsText;
    private AudioManager audioManager;
    private string[] words;
    private string currentWord;
    private float lastWordCompletionTime;
    private float gameStartTime;
    private int totalCharsTyped;
    private float errorPenalty;
    private string lastInput = "";
    private const float CHARS_PER_WORD = 3f;
    public int wpm;
    private bool audioPlayed = false;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void Start()
    {
        gameStartTime = Time.time;
        totalCharsTyped = 0;
        errorPenalty = 0;
        words = new string[] { "answer", "foster", "voyage", "delete", "locate", "crutch",
            "deputy", "mother", "harbor", "nuance", "extend", "cheque", "linear", "credit",
            "winner", "murder", "script", "betray", "growth", "waiter", "horror", "minute",
            "ensure", "gallon", "linger", "depend", "barrel", "temple", "exotic", "chance",
            "clique", "deadly", "school", "defeat", "grudge", "deport", "devote", "prayer",
            "wonder", "notice", "regard", "memory", "couple", "stress", "excuse", "summer",
            "tablet", "garlic", "mosaic", "colony" }; //hardcoded for now
        inputField.interactable = false;
        inputField.onValueChanged.AddListener(CheckWord);
    }

    private void Update()
    {
        if (!RaceCountdown.raceStarted) return;

        if (!inputField.interactable)
        {
            inputField.interactable = true;
            inputField.ActivateInputField();
            gameStartTime = Time.time;
            DisplayWords();
        }
        UpdateWpm();

        if(!audioPlayed && wpm >= 1f)
        {
            audioPlayed = true;
            FindObjectOfType<PlayerCarAudio>().PlayEngineSound();
        }
    }

    private void DisplayWords()
    {
        currentWord = words[Random.Range(0, words.Length)];
        wordsText.text = currentWord;
        inputField.text = "";
        lastInput = "";
        inputField.ActivateInputField();
        lastWordCompletionTime = Time.time;
    }

    private void CheckWord(string input)
    {
        if (string.IsNullOrEmpty(input)) return;

        bool isCurrentInputCorrect = currentWord.StartsWith(input);

        if (input.Length > lastInput.Length)
        {
            int correctPrefix = 0;
            for (int i = 0; i < input.Length && i < currentWord.Length; i++)
            {
                if (input[i] == currentWord[i])
                    correctPrefix++;
                else
                    break;
            }

            if (!isCurrentInputCorrect && input.Length > correctPrefix)
            {
                errorPenalty += 0.2f;
            }
        }

        if (input.Equals(currentWord))
        {
            float wordCompletionTime = Time.time - lastWordCompletionTime;
            if (wordCompletionTime < 0.2f)
            {
                Debug.Log("heckr");
            }
            else
            {
                totalCharsTyped += currentWord.Length + 1;
            }
            DisplayWords();
        }
        else if (input.Length >= currentWord.Length)
        {
            audioManager.Play("race-car-downshift"); //adding a downshift sfx for fumbling yeah i am genius
            DisplayWords();
        }

        lastInput = input;
        UpdateWpm();
    }

    private void UpdateWpm()
    {
        float timeElapsed = (Time.time - gameStartTime) / 60f;
        if (timeElapsed > 0f)
        {
            float adjustedChars = totalCharsTyped - Mathf.Min(errorPenalty, totalCharsTyped * 0.5f);
            wpm = Mathf.FloorToInt((adjustedChars / CHARS_PER_WORD) / timeElapsed);
            wpmText.text = $"WPM: {wpm}";
        }
        else
        {
            wpmText.text = "WPM: 0";
        }
    }

    public float GetWpm()
    {
        return wpm;
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("main menu");
    }
}