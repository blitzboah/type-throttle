using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] InputField setWpmField;    
    [SerializeField] Dropdown wpmDropdown;
    [SerializeField] Dropdown timeDropdown;
    [SerializeField] Text wpmText;
    [SerializeField] Toggle adaptiveToggle;
    [SerializeField] Button audioOn;
    [SerializeField] Button audioOff;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (!AudioListener.pause == true)
        {
            audioOn.gameObject.SetActive(true);
            audioOff.gameObject.SetActive(false);
        }
        else
        {
            audioOn.gameObject.SetActive(false);
            audioOff.gameObject.SetActive(true);
        }
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
                BotSpeedManager.botSpeed = Mathf.Abs(customWpm); //nice try negative diddy
            }
        }
        else
        {
            BotSpeedManager.botSpeed = float.Parse(selectedOption);
        }

        string selectedTimeOption = timeDropdown.options[timeDropdown.value].text;
        BotSpeedManager.totalTime = float.Parse(selectedTimeOption);
        BotSpeedManager.isAdaptive = adaptiveToggle.isOn;

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

    public void MuteAudio()
    {
        AudioListener.pause = true;
    }

    public void UnmuteAudio()
    {
        AudioListener.pause = false;
    }

}
