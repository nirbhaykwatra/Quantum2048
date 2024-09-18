using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgress : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float speed;
    float currentValue;

    // Update is called once per frame
    void Update()
    {
        if(currentValue < 100)
        {
            currentValue += speed * Time.deltaTime;
            //text.text = ((int)currentValue).ToString() + "%";
        }
        image.fillAmount = currentValue / 100;
    }
}
