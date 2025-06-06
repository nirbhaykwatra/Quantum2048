using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [FoldoutGroup("Tutorial")]
    [SerializeField] private TextMeshProUGUI _tutorialText;
    
    [FoldoutGroup("Tutorial")]
    [SerializeField] private GameObject _tilePrefab;
    
    
}