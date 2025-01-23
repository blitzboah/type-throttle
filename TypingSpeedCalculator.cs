using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TypingSpeedCalculator : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Text wpmText;
    [SerializeField] private Text wordsText;
    [SerializeField] private WordList wordList;
    private AudioManager audioManager;
    private string currentWord;
    private float lastWordCompletionTime;
    private float gameStartTime;
    private int totalCharsTyped;
    private float errorPenalty;
    private string lastInput = "";
    private const float CHARS_PER_WORD = 3f; //the reason for not putting 5 in here coz the words appear one by one and not in a sequence, doing 5 would drop down wpm significantly
    //for the game that is reflex based 3 was the option i had to take
    public int wpm;
    private bool audioPlayed = false;

    public int wordCount { get; private set; } = 0;



    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void Start()
    {
        gameStartTime = Time.time;
        totalCharsTyped = 0;
        errorPenalty = 0;
        inputField.interactable = false;
        inputField.onValueChanged.AddListener(CheckWord);
    }

    private void Update()
    {
        if (!RaceCountdown.raceStarted) return;

        if(Input.GetKeyDown(KeyCode.Return)) inputField.ActivateInputField();
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
        wordCount++;
        currentWord = GetRandomWord();
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

    public string GetRandomWord()
    {
        int randomIndex = Random.Range(0, wordList.words.Length);
        return wordList.words[randomIndex];
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