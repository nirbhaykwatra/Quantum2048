using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tutorial : MonoBehaviour
{
    Animator animtut;

    private void Start()
    {
        animtut = GetComponent<Animator>();
    }

    void ChangeAnimation()
    {
        animtut.SetInteger("Change", animtut.GetInteger("Change") + 1);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            ChangeAnimation();
        }

        if (animtut.GetInteger("Change") > 5)
        {
            //SceneManager.UnloadSceneAsync("Tutorial");
            SceneManager.LoadScene("2048", LoadSceneMode.Single);
        }
    }
}
