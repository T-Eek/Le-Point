using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserNameManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TextMeshProUGUI displayUsername;

    // Start is called before the first frame update
    void Start()
    {
        LoadUsername();
        DisplayUsername();

        // Attach the OnValueChanged listener to restrict input to letters and numbers only
        usernameInput.onValueChanged.AddListener(OnUsernameValueChanged);
    }

    public void SaveUsername()
    {
        string username = usernameInput.text;

        if (string.IsNullOrEmpty(username))
        {
            username = "Quest";
            // You can show a notification here or take other appropriate actions for an empty username.
        }

        PlayerPrefs.SetString("Username", username);
        DisplayUsername();
        Debug.Log("Saved Username: " + username);
    }

    public void ChangeUsername(string newUsername)
    {
        if (string.IsNullOrEmpty(newUsername))
        {
            newUsername = "Quest";
            // You can show a notification here or take other appropriate actions for an empty username.
        }

        PlayerPrefs.SetString("Username", newUsername);
        DisplayUsername();
    }

    private void LoadUsername()
    {
        if (PlayerPrefs.HasKey("Username"))
        {
            string savedUsername = PlayerPrefs.GetString("Username");
            usernameInput.text = savedUsername;
        }
    }

    public void DisplayUsername()
    {
        string storedUsername = PlayerPrefs.GetString("Username");

        if (string.IsNullOrEmpty(storedUsername))
        {
            storedUsername = "Quest";
            PlayerPrefs.SetString("Username", storedUsername); // Save the default username to PlayerPrefs
        }

        displayUsername.text = storedUsername + "'s";
    }

    // Attach this method to the "Save" button's onClick event in the Unity Editor.
    public void OnSaveButtonClick()
    {
        SaveUsername();
    }

    // Restrict input to letters and numbers
    private void OnUsernameValueChanged(string newText)
    {
        string filteredText = "";
        foreach (char c in newText)
        {
            if (char.IsLetterOrDigit(c))
            {
                filteredText += c;
            }
        }

        usernameInput.text = filteredText;
    }
}