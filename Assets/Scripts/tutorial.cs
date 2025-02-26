using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * tutorial Class
 * ---------------
 * Handles tutorial animations and transitions to the main game scene.
 * When any key is pressed, it triggers the next tutorial animation.
 * Once all tutorial animations have played, it loads the main game scene ("2048").
 */
public class tutorial : MonoBehaviour
{
    // Reference to the Animator component controlling the tutorial animations.
    Animator animtut;

    // Called before the first frame update.
    // Initializes the Animator component by retrieving it from the current GameObject.
    private void Start()
    {
        animtut = GetComponent<Animator>();
    }

    // Increments the "Change" parameter in the Animator to trigger the next animation.
    void ChangeAnimation()
    {
        animtut.SetInteger("Change", animtut.GetInteger("Change") + 1);
    }

    // Called once per frame.
    // Detects key presses to change animations and checks if the tutorial is complete.
    private void Update()
    {
        // If any key is pressed, trigger the next animation.
        if (Input.anyKeyDown)
        {
            ChangeAnimation();
        }

        // If the "Change" parameter exceeds 5, transition to the main game scene.
        if (animtut.GetInteger("Change") > 5)
        {
            // Optionally, unload the tutorial scene asynchronously.
            // SceneManager.UnloadSceneAsync("Tutorial");

            // Load the main game scene "2048".
            SceneManager.LoadScene("2048", LoadSceneMode.Single);
        }
    }
}