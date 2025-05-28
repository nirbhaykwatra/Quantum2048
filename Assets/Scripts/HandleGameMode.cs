using UnityEngine;

public class HandleGameMode : MonoBehaviour
{
    public GameModeObject GameModeObject;
    public void SetGameMode(string gameMode)
    {
        switch (gameMode)
        {
            case "newgame":
                GameModeObject.GameMode = GameModeEnum.NEW_GAME;
                break;
            case "tutorialtun":
                GameModeObject.GameMode = GameModeEnum.TUNNELING;
                break;
            case "tutorialsup":
                GameModeObject.GameMode = GameModeEnum.SUPERPOSITION;
                break;
            case "tutorialent":
                GameModeObject.GameMode = GameModeEnum.ENTANGLEMENT;
                break;
        }
    }
}
