using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tutorial Objects/Modal Data")]
public class TutorialModalData : ScriptableObject
{
    public List<string> TunnelingWindowTitles = new List<string>();
    public List<string> TunnelingWindowDescriptions = new List<string>();
    
    public List<string> SuperpositionWindowTitles = new List<string>();
    public List<string> SuperpositionWindowDescriptions = new List<string>();
    
    public List<string> EntanglementWindowTitles = new List<string>();
    public List<string> EntanglementWindowDescriptions = new List<string>();
}
