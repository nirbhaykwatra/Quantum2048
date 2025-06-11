using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tutorial Objects/Tutorial Selector")]
public class TutorialSelectorData : ScriptableObject
{
    public List<Vector2> tunnelingSelectorPositions = new List<Vector2>();
    public List<Vector2> tunnelingSelectorScale = new List<Vector2>();
    public List<bool> tunnelingSelectorActive = new List<bool>();
    public List<Animation> tunnelingSelectorAnimations = new List<Animation>();
    
    public List<Vector2> superpositionSelectorPositions = new List<Vector2>();
    public List<Vector2> superpositionSelectorScale = new List<Vector2>();
    public List<bool> superpositionSelectorActive = new List<bool>();
    public List<Animation> superpositionSelectorAnimations = new List<Animation>();
    
    public List<Vector2> entanglementSelectorPositions = new List<Vector2>();
    public List<Vector2> entanglementSelectorScale = new List<Vector2>();
    public List<bool> entanglementSelectorActive = new List<bool>();
    public List<Animation> entanglementSelectorAnimations = new List<Animation>();
}
