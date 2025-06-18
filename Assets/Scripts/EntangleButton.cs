using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntangleButton : MonoBehaviour
{
    private Button _button;
    private TileBoard _board;
    [SerializeField] private TextMeshProUGUI _buttonText;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _board = GetComponentInParent<TileBoard>();
    }
    
    public void HandleScoreUpdate(float score)
    {
        _button.interactable = !(score < _board.entanglementCost);
        if (!_button.interactable)
        {
            _buttonText.text = $"Entangle\nCost: {_board.entanglementCost}";
        }
        else
        {
            _buttonText.text = "Entangle";
        }
    }
}
