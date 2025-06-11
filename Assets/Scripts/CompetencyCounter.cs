using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using GameEvents;

public class CompetencyCounter : MonoBehaviour
{
    [SerializeField] private int _maxValue;
    
    [FoldoutGroup("Events")] 
    [SerializeField] private IntEventAsset _tunnelingTutorialStageChangeEvent;
    [FoldoutGroup("Events")] 
    [SerializeField] private IntEventAsset _superpositionTutorialStageChangeEvent;
    [FoldoutGroup("Events")] 
    [SerializeField] private IntEventAsset _entanglementTutorialStageChangeEvent;
    
    private Slider _counter;
    private TextMeshProUGUI _counterText;
    private bool _count;

    private void Awake()
    {
        _counter = GetComponentInChildren<Slider>();
        _counterText = GetComponentInChildren<TextMeshProUGUI>();
        
        _counter.maxValue = _maxValue;
        _counterText.text = $"{_counter.value}/{_maxValue}";
    }

    private void OnEnable()
    {
        _count = true;
        _counter.onValueChanged.AddListener(delegate { OnCounterChange();});
    }

    private void OnCounterChange()
    {
        _counterText.text = $"{_counter.value}/{_maxValue}";
    }

    public void HandleSpecialMoveEvent(string moveType)
    {
        if (!_count) return;
        switch (moveType)
        {
            case "Tunnelling":
                if (_counter.value < _maxValue)
                {
                    _counter.value++;
                    _counterText.text = $"{_counter.value}/{_maxValue}";
                }
                else if (_counter.value >= _maxValue)
                {
                    // Set tunnelling tutorial stage to 7
                    _tunnelingTutorialStageChangeEvent.Invoke(7);
                }
                
                break;
        }
    }

    private void OnDisable()
    {
        _count = false;
        _counter.onValueChanged.RemoveAllListeners();
    }
}
