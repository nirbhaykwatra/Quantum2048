using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class UIEvents : MonoBehaviour
{
    [FoldoutGroup("UI Components", expanded:true)]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [FoldoutGroup("UI Components", expanded:true)]
    [SerializeField] private TextMeshProUGUI _highScoreText;
    
    public void UpdateScore(float score)
    {
        _scoreText.text = score.ToString();
    }

    public void UpdateHighScore(float score)
    {
        _highScoreText.text = score.ToString();
    }
}
