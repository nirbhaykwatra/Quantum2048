using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Button modalButton;

    public void SetTitle(string title)
    {
        this.title.text = title;
    }

    public void SetDescription(string description)
    {
        this.description.text = description;
    }
}
