using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] InputField setWpmField;    
    [SerializeField] Dropdown wpmDropdown;
    [SerializeField] Dropdown timeDropdown;
    [SerializeField] Text wpmText;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void Quit()
    {
        audioManager.Play("click-sound");
        Application.Quit();
    }

    public void StartGame()
    {
        string selectedOption = wpmDropdown.options[wpmDropdown.value].text;

        if (selectedOption == "custom")
        {
            if (float.TryParse(setWpmField.text, out float customWpm))
            {
                BotSpeedManager.botSpeed = customWpm;
            }
        }
        else
        {
            BotSpeedManager.botSpeed = float.Parse(selectedOption);
        }

        string selectedTimeOption = timeDropdown.options[timeDropdown.value].text;
        BotSpeedManager.totalTime = float.Parse(selectedTimeOption);

        audioManager.Play("click-sound");
        SceneManager.LoadScene("main");
    }

    public void OnDropdownValueChanged()
    {
        audioManager.Play("click-sound");
        string selectedOption = wpmDropdown.options[wpmDropdown.value].text;

        if (selectedOption == "custom")
        {
            wpmText.gameObject.SetActive(false);
            setWpmField.interactable = true; 
            setWpmField.text = "";           
        }
        else
        {
            wpmText.gameObject.SetActive(true);
            setWpmField.interactable = false;
            setWpmField.text = "";      
        }
    }

    public void ClickSound()
    {
        audioManager.Play("click-sound");
    }

}
