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
    List<string> tunnelling1_titleTexts;
    List<string> tunnelling1_descriptionTexts;

    void Awake()
    {
        // Initialize the class-level lists instead of redeclaring them
        tunnelling1_titleTexts = new List<string>
        {
            "What just happened?",
            "What is tunneling?",
            "How is quantum tunneling possible?"
        };

        tunnelling1_descriptionTexts = new List<string>
        {
            "You've just witnessed a tunnelling merge! \n\nA tunnelling merge happens when a tile passes through another tile to merge with a third tile.",
            "This inspired by the quantum phenomenon (called tunnelling!) where a particle passes through a barrier that it shouldn't be able to cross according to classical physics.",
            "Quantum tunneling is possible because particles in quantum mechanics behave as probabilistic waves, allowing them to have a finite chance of passing through energy barriers, even if they lack the classical energy to overcome them."
        };

        // Find the Canvas first
        GameObject canvas = GameObject.Find("Canvas");
        competencyCounter = canvas.transform.Find("Competency Counter")?.gameObject;
        infoButton = canvas.transform.Find("Info Button")?.gameObject;
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
        if (index < tunnelling1_titleTexts.Count - 1)
        {
            ShowNextText();
        }
        else
        {
            // If all the texts have been shown, resume the game and open the Competency Counter and the info button
            competencyCounter.SetActive(true);
            infoButton.SetActive(true);
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
        titleText.SetText(tunnelling1_titleTexts[index]);
        descriptionText.SetText(tunnelling1_descriptionTexts[index]);
    }

    public void InfoContinue()
    {
        // If index is greater than or equal to the count of the title texts, reset the index to -1
        if (index >= tunnelling1_titleTexts.Count - 1)
        {
            index = -1;
        }
        ShowNextText();
    }
}
