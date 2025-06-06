using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Game Mode")]
public class GameModeObject : ScriptableObject
{
    public GameModeEnum GameMode;
    public int TunnelingStep { get; set; } = 0;
    public int SuperpositionStep { get; set; } = 0;
    public int EntanglementStep { get; set; } = 0;

    public void IncreaseTunnelingStep()
    {
        TunnelingStep++;
    }

    public void IncreaseSuperpositionStep()
    {
        SuperpositionStep++;
    }

    public void IncreaseEntanglementStep()
    {
        EntanglementStep++;
    }

    public void ResetTunnelingStep()
    {
        TunnelingStep = 0;
    }

    public void ResetSuperpositionStep()
    {
        SuperpositionStep = 0;
    }

    public void ResetEntanglementStep()
    {
        EntanglementStep = 0;
    }
}
