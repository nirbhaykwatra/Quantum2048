using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tutorial Objects/TutorialData")]
public class TutorialData : ScriptableObject
{
    public int tunnelingStage = 0;
    public int superpositionStage = 0;
    public int entanglementStage = 0;

    public void ResetTunnelingStage()
    {
        tunnelingStage = 0;
    }

    public void ResetSuperpositionStage()
    {
        superpositionStage = 0;
    }

    public void ResetEntanglementStage()
    {
        entanglementStage = 0;
    }
}
