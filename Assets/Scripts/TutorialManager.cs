using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [FoldoutGroup("UI Callbacks", expanded: true)]
    // Reference to the CanvasGroup used to show/hide the game over screen.
    [SerializeField] private CanvasGroup _gameOver;
    
    [FoldoutGroup("Tutorial UI")]
    [SerializeField] private GameObject _tunnelingPanel;
    
    [FoldoutGroup("Tutorial UI")]
    [SerializeField] private GameObject _superpositionPanel;
    
    [FoldoutGroup("Tutorial UI")]
    [SerializeField] private GameObject _entanglementPanel;
    
    [FoldoutGroup("Prefabs")]
    [SerializeField] private GameObject _tilePrefab;
    
    [SerializeField] private GameModeObject _gameModeObject;
    
    private TileBoard _board;

    private void Awake()
    {
        _board = GetComponentInChildren<TileBoard>();
    }

    #region Tunneling Tutorial

    public void TutorialTunneling()
    {
        _gameModeObject.ResetTunnelingStep();
        _board.SuperpositionEnabled = false;
        _board.EntanglementEnabled = false;
        _board.CreateNewTilesOnMove = false;
        _gameOver.alpha = 0f;
        _gameOver.interactable = false;
        _board.ClearBoard();
        
        _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(1, 2));
        _board.CreateTile(_board.tileStates[2], _board.grid.GetCell(2, 2));
        _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(3, 2));
        _board.enabled = true;
    }
    
    #endregion
    
    #region Superposition Tutorial

    public void TutorialSuperposition()
    {
        _gameModeObject.ResetSuperpositionStep();
        _board.TunnelingEnabled = false;
        _board.EntanglementEnabled = false;
        _board.CreateNewTilesOnMove = false;
        _gameOver.alpha = 0f;
        _gameOver.interactable = false;
        _board.ClearBoard();

        _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(1, 2), true);
        _board.enabled = true;
    }
    
    #endregion
    
    #region Entanglement Tutorial

    public void TutorialEntanglement()
    {
        _gameModeObject.ResetEntanglementStep();
        _board.TunnelingEnabled = false;
        _board.SuperpositionEnabled = true;
        _board.CreateNewTilesOnMove = false;
        _gameOver.alpha = 0f;
        _gameOver.interactable = false;
        _board.ClearBoard();
        _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(0, 0));
        _board.CreateTile(_board.tileStates[2], _board.grid.GetCell(3, 3));
        _board.enabled = true;
    }

    #endregion
    public void HandleReset()
    {
        switch (_gameModeObject.GameMode)
        {
            case GameModeEnum.NEW_GAME:
                break;
            case GameModeEnum.TUNNELING:
                TutorialTunneling();
                break;
            case GameModeEnum.SUPERPOSITION:
                TutorialSuperposition();
                break;
            case GameModeEnum.ENTANGLEMENT:
                TutorialEntanglement();
                break;
        }
    }

}