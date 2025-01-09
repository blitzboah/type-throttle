using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] InputField setWpmField;
    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        if (float.TryParse(setWpmField.text, out float wpm))
        {
            BotSpeedManager.botSpeed = wpm;
        }
        else
        {
            BotSpeedManager.botSpeed = 50f; //default
        }

        SceneManager.LoadScene("type");
    }

}
