using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public string level;  // Corrected: Specified the type as string
    private GameObject competencyCounter;
    private GameObject infoButton;
    public GameObject PausePanel;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    private int index = -1;
    List<string> titleTexts;
    List<string> descriptionTexts;

    void Awake()
    {
        // Initialize the class-level lists instead of redeclaring them
        if (level == "tunnelling1")  // Corrected: Replaced colon with braces
        {
            titleTexts = new List<string>
            {
                "What just happened?",
                "What is tunneling?",
                "How is quantum tunneling possible?"
            };

            descriptionTexts = new List<string>
            {
                "You've just witnessed a tunnelling merge! \n\nA tunnelling merge happens when a tile passes through another tile to merge with a third tile.",
                "This is inspired by the quantum phenomenon (called tunnelling!) where a particle passes through a barrier that it shouldn't be able to cross according to classical physics.",
                "Quantum tunneling is possible because particles in quantum mechanics behave as probabilistic waves, allowing them to have a finite chance of passing through energy barriers, even if they lack the classical energy to overcome them."
            };
        }
        else
        {
            titleTexts = new List<string>
            {
                "Hello world!",
                "Hello qworld!",
                "Hello quantum world!"
            };

            descriptionTexts = new List<string>
            {
                "Welcome to the quantum world!",
                "This is a simple demonstration of quantum concepts.",
                "Have fun exploring the possibilities of quantum computing!"
            };
        }

        // Find the Canvas first
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            competencyCounter = canvas.transform.Find("Competency Counter")?.gameObject;
            infoButton = canvas.transform.Find("Info Button")?.gameObject;
        }
        else
        {
            Debug.LogError("Canvas GameObject not found.");
        }
    }

    void Start()
    {
        // Initially, the game is paused, and the panel is active
        PausePanel.SetActive(true);
        Time.timeScale = 0;

        // Start by showing the first text in the list
        ShowNextText();
    }

    void Update()
    {
        // You can add additional update logic here if needed
    }

    public void Pause()
    {
        // If the player wants to pause, we show the pause panel and stop time
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        // If we still have text to show, show the next one
        if (index < titleTexts.Count - 1)
        {
            ShowNextText();
        }
        else
        {
            // If all the texts have been shown, resume the game and open the Competency Counter and the info button
            if (competencyCounter != null)
            {
                competencyCounter.SetActive(true);
            }
            else
            {
                Debug.LogError("competencyCounter is null. Cannot set active.");
            }

            if (infoButton != null)
            {
                infoButton.SetActive(true);
            }
            else
            {
                Debug.LogError("infoButton is null. Cannot set active.");
            }

            PausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void ShowNextText()
    {
        // Increment the index and update the text fields
        index++;
        // Print out the index to the console to help with debugging
        Debug.Log("Index: " + index);

        // Ensure index is within bounds
        if (index >= 0 && index < titleTexts.Count)
        {
            if (titleText != null)
            {
                titleText.SetText(titleTexts[index]);
            }
            else
            {
                Debug.LogError("titleText is null. Cannot set text.");
            }

            if (descriptionText != null)
            {
                descriptionText.SetText(descriptionTexts[index]);
            }
            else
            {
                Debug.LogError("descriptionText is null. Cannot set text.");
            }
        }
        else
        {
            Debug.LogError("Index out of bounds when trying to access titleTexts and descriptionTexts.");
        }
    }

    public void InfoContinue()
    {
        // If index is greater than or equal to the count of the title texts, reset the index to -1
        if (index >= titleTexts.Count - 1)
        {
            index = -1;
        }
        ShowNextText();
    }
}