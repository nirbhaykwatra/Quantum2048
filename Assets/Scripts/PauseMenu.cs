using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    private GameObject competencyCounter;
    private GameObject infoButton;
    public GameObject PausePanel;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    private int index = -1;
    private List<string> titleTexts;
    private List<string> descriptionTexts;

    void Awake()
    {
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

        // Initialize texts based on the current GlobalData.level
        InitializeTexts();
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
        // Additional update logic if needed
    }

    // Initializes the titleTexts and descriptionTexts based on GlobalData.level.
    // Call this method whenever GlobalData.level changes.
    public void InitializeTexts()
    {
        if (GlobalData.level == "tunnelling1")
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
        else if (GlobalData.level == "tunnelling2")
        {
            titleTexts = new List<string>
            {
                "More about quantum tunneling...",
                "More about quantum tunneling...",
                "The catch!",
                "The catch!",
                "Time to play!"
            };

            descriptionTexts = new List<string>
            {
                "As mentioned earlier, quantum tunneling is where instead of always having to climb over the tall fence, sometimes—just sometimes—particles can appear on the other side without going over the top!",
                "It’s as if they find a hidden tunnel under the fence!",
                "But here’s the catch: the taller the fence (think of it as a larger number)... \n\n\n ...the harder it is for the particle to use this trick.",
                "If the fence is very tall (a giant number) then it’s much less likely that the particle can “tunnel” through. \n\nSo, even though the particle has this special ability, the bigger the barrier (the higher the number), the less often it happens.",
                "To help you understand this a little better, try tunnelling now! \n\n You'll notice that you can only tunnel through tiles above a certain number... what could that number be?"
            };
        }
        else
        {
            Debug.LogError("Invalid level: " + GlobalData.level);
            // You can set a default set of texts if needed
            titleTexts = new List<string>() { "Invalid Level" };
            descriptionTexts = new List<string>() { "No descriptions available." };
        }

        // Reset index so that showing texts starts from the beginning
        index = -1;
    }

    public void Pause()
    {
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
            // All texts shown, resume game and enable UI elements
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
        // Log the titleTexts
        Debug.Log(titleTexts);

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
        // Reinitialize texts to ensure the correct ones are used
        InitializeTexts();
        
        // If we've reached the end, reset index and show from the start again
        if (index >= titleTexts.Count - 1)
        {
            index = -1;
        }
        ShowNextText();
    }
}