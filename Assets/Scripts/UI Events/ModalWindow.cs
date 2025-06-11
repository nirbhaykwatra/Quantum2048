using TMPro;
using UnityEngine;

public class ModalWindow : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public void SetTitle(string title)
    {
        this.title.text = title;
    }

    public void SetDescription(string description)
    {
        this.description.text = description;
    }
}
