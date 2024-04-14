using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtonFunction : MonoBehaviour
{
    public InputField nameField;
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        if (string.IsNullOrEmpty(nameField.text))
        {
            nameField.placeholder.GetComponent<Text>().text = "Click here to type your name";
        }

        else
        {
            PlayerPrefs.SetString("PlayerName", nameField.text);
            SceneManager.LoadScene(1);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
