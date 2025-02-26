using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * PauseMenu Class
 * ---------------
 * Manages the pause menu functionality, including displaying context-sensitive tutorial text
 * based on the current game level, pausing/resuming gameplay, and handling related UI elements.
 */
public class PauseMenu : MonoBehaviour
{
    // Reference to the "Competency Counter" UI element, used to display game competency data.
    private GameObject competencyCounter;

    // Reference to the "Info Button" UI element, which may provide additional game information.
    private GameObject infoButton;

    // The GameObject representing the pause menu panel.
    public GameObject PausePanel;

    // Text component for displaying the title text in the pause menu.
    public TMP_Text titleText;

    // Text component for displaying the description text in the pause menu.
    public TMP_Text descriptionText;

    // Index for the current position in the text lists.
    private int index = -1;

    // List of title strings to be displayed based on the current game level.
    private List<string> titleTexts;

    // List of description strings corresponding to the title texts.
    private List<string> descriptionTexts;

    // Called when the script instance is being loaded.
    // Locates essential UI elements and initializes the tutorial texts according to the current level.
    void Awake()
    {
        // Locate the Canvas GameObject in the scene.
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            // Find the "Competency Counter" and "Info Button" as children of the Canvas.
            competencyCounter = canvas.transform.Find("Competency Counter")?.gameObject;
            infoButton = canvas.transform.Find("Info Button")?.gameObject;
        }
        else
        {
            Debug.LogError("Canvas GameObject not found.");
        }

        // Initialize the tutorial texts based on the current GlobalData.level.
        InitializeTexts();
    }

    // Called before the first frame update.
    // Activates the pause panel, pauses the game, and displays the first text entry.
    void Start()
    {
        // Activate the pause menu panel.
        PausePanel.SetActive(true);
        // Pause the game by setting the timescale to 0.
        Time.timeScale = 0;

        // Display the first set of tutorial text.
        ShowNextText();
    }

    // Update is called once per frame.
    // Reserved for future per-frame logic if needed.
    void Update()
    {
        // Additional update logic can be added here if required.
    }

    // Initializes the titleTexts and descriptionTexts lists based on the current game level.
    // This method should be called whenever GlobalData.level changes.
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
            // Set default texts if the level is invalid.
            titleTexts = new List<string>() { "Invalid Level" };
            descriptionTexts = new List<string>() { "No descriptions available." };
        }

        // Reset the index to start showing texts from the beginning.
        index = -1;
    }

    // Activates the pause menu and pauses the game.
    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    // Continues the game by either displaying the next text in the sequence or resuming gameplay if all texts have been shown.
    // Also reactivates UI elements once the pause menu is dismissed.
    public void Continue()
    {
        // Check if there are more texts to display.
        if (index < titleTexts.Count - 1)
        {
            ShowNextText();
        }
        else
        {
            // All texts have been displayed; resume the game.
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

            // Deactivate the pause panel and resume the game by restoring the timescale.
            PausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    // Displays the next set of title and description texts in the pause menu.
    // Increments the internal index and updates the UI text elements accordingly.
    private void ShowNextText()
    {
        // Increment the text index.
        index++;
        // Log the current list of title texts for debugging purposes.
        Debug.Log(titleTexts);

        // Ensure the index is within valid bounds.
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

    // Advances the pause menu text sequence.
    // Reinitializes texts if necessary and resets the index when the end of the sequence is reached.
    public void InfoContinue()
    {
        // Reinitialize the texts to ensure they are up to date with the current game level.
        InitializeTexts();
        
        // If the end of the text list is reached, reset the index to start over.
        if (index >= titleTexts.Count - 1)
        {
            index = -1;
        }
        ShowNextText();
    }
}