using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TutorialManager : MonoBehaviour
{
    [FoldoutGroup("UI Callbacks", expanded: true)]
    // Reference to the CanvasGroup used to show/hide the game over screen.
    [SerializeField] private CanvasGroup _gameOver;
    
    [FoldoutGroup("UI Callbacks", expanded: true)]
    // Reference to the CanvasGroup used to show/hide the game over screen.
    [SerializeField] private Button _entangleButton;
    
    [FoldoutGroup("Tutorial UI")]
    [SerializeField] private GameObject _tutorialPanel;
    
    [FoldoutGroup("Tutorial UI")]
    [SerializeField] private ModalWindow _tutorialModal;
    
    [FoldoutGroup("Tutorial UI")]
    [SerializeField] private TutorialSelectorUI _tutorialSelector;
    
    [FoldoutGroup("Tutorial UI")]
    [SerializeField] private CompetencyCounter _competencyCounter;
    
    [FoldoutGroup("Tutorial Data")]
    [SerializeField] private TutorialData _tutorialData;
    
    [FoldoutGroup("Tutorial Data")]
    [SerializeField] private TutorialModalData _tutorialModalData;
    
    [FoldoutGroup("Tutorial Data")]
    [SerializeField] private TutorialSelectorData _tutorialSelectorData;

    [FoldoutGroup("Events")] 
    [SerializeField] private IntEventAsset _tunnelingTutorialStageChangeEvent;
    [FoldoutGroup("Events")] 
    [SerializeField] private IntEventAsset _superpositionTutorialStageChangeEvent;
    [FoldoutGroup("Events")] 
    [SerializeField] private IntEventAsset _entanglementTutorialStageChangeEvent;
    
    [FoldoutGroup("Prefabs")]
    [SerializeField] private GameObject _tilePrefab;
    
    [SerializeField] private GameModeObject _gameModeObject;
    
    private TileBoard _board;
    private TileGrid _grid;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _board = GetComponentInChildren<TileBoard>();
        _grid = GetComponentInChildren<TileGrid>();
        _playerInput = GetComponent<PlayerInput>();
        _tutorialPanel.SetActive(false);
    }

    /*
    #region Tunneling Tutorial

    public void TutorialTunneling()
    {
        _tutorialData.ResetTunnelingStage();
        _gameModeObject.ResetTunnelingStep();
        _tutorialPanel.SetActive(true);
        _entangleButton.gameObject.SetActive(false);
        _playerInput.enabled = false;
        ChangeModalContent();
        ChangeSelector();
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
        _tutorialData.ResetSuperpositionStage();
        _gameModeObject.ResetSuperpositionStep();
        _tutorialPanel.SetActive(true);
        _playerInput.enabled = false;
        ChangeModalContent();
        ChangeSelector();
        _board.TunnelingEnabled = false;
        _board.EntanglementEnabled = false;
        _board.CreateNewTilesOnMove = false;
        _gameOver.alpha = 0f;
        _gameOver.interactable = false;
        _board.ClearBoard();

        _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(1, 2), false);
        _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(2, 2), false);
        _board.enabled = true;
    }
    
    #endregion*/
    
    /*#region Entanglement Tutorial

    public void TutorialEntanglement()
    {
        _tutorialData.ResetEntanglementStage();
        _gameModeObject.ResetEntanglementStep();
        _tutorialPanel.SetActive(true);
        _playerInput.enabled = false;
        ChangeModalContent();
        ChangeSelector();
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
    #endregion*/

    public void HandleResetTutorialStage()
    {
        _tutorialPanel.SetActive(false);
        switch (_gameModeObject.GameMode)
        {
            case GameModeEnum.TUNNELING:
                _tutorialData.ResetTunnelingStage();
                break;
            case GameModeEnum.SUPERPOSITION:
                _tutorialData.ResetSuperpositionStage();
                break;
            case GameModeEnum.ENTANGLEMENT:
                _tutorialData.ResetEntanglementStage();
                break;
        }
    }
    
    


    #region Modal Button Methods
    public void ChangeModalContent()
    {
        switch (_gameModeObject.GameMode)
        {
            case GameModeEnum.TUNNELING:
                _tutorialModal.title.text = _tutorialModalData.TunnelingWindowTitles[_tutorialData.tunnelingStage];
                _tutorialModal.description.text = _tutorialModalData.TunnelingWindowDescriptions[_tutorialData.tunnelingStage];
                break;
            case GameModeEnum.SUPERPOSITION:
                _tutorialModal.title.text = _tutorialModalData.SuperpositionWindowTitles[_tutorialData.superpositionStage];
                _tutorialModal.description.text = _tutorialModalData.SuperpositionWindowDescriptions[_tutorialData.superpositionStage];
                break;
            case GameModeEnum.ENTANGLEMENT:
                _tutorialModal.title.text = _tutorialModalData.EntanglementWindowTitles[_tutorialData.entanglementStage];
                _tutorialModal.description.text = _tutorialModalData.EntanglementWindowDescriptions[_tutorialData.entanglementStage];
                break;
        }
    }

    public void ChangeSelector()
    {
        switch (_gameModeObject.GameMode)
        {
            case GameModeEnum.TUNNELING:
                _tutorialSelector.ChangePosition(_tutorialSelectorData.tunnelingSelectorPositions[_tutorialData.tunnelingStage]);
                _tutorialSelector.ChangeScale(_tutorialSelectorData.tunnelingSelectorScale[_tutorialData.tunnelingStage]);
                _tutorialSelector.SetActive(_tutorialSelectorData.tunnelingSelectorActive[_tutorialData.tunnelingStage]);
                break;
            case GameModeEnum.SUPERPOSITION:
                _tutorialSelector.ChangePosition(_tutorialSelectorData.superpositionSelectorPositions[_tutorialData.superpositionStage]);
                _tutorialSelector.ChangeScale(_tutorialSelectorData.superpositionSelectorScale[_tutorialData.superpositionStage]);
                _tutorialSelector.SetActive(_tutorialSelectorData.superpositionSelectorActive[_tutorialData.superpositionStage]);
                break;
            case GameModeEnum.ENTANGLEMENT:
                _tutorialSelector.ChangePosition(_tutorialSelectorData.entanglementSelectorPositions[_tutorialData.entanglementStage]);
                _tutorialSelector.ChangeScale(_tutorialSelectorData.entanglementSelectorScale[_tutorialData.entanglementStage]);
                _tutorialSelector.SetActive(_tutorialSelectorData.entanglementSelectorActive[_tutorialData.entanglementStage]);
                break;
        }
    }
    
    public void IncreaseTunnelingStage()
    {
        _tutorialData.tunnelingStage++;
        ChangeModalContent();
        ChangeSelector();
        _tunnelingTutorialStageChangeEvent.Invoke(_tutorialData.tunnelingStage);
    }

    public void DecreaseTunnelingStage()
    {
        _tutorialData.tunnelingStage--;
        ChangeModalContent();
        ChangeSelector();
        _tunnelingTutorialStageChangeEvent.Invoke(_tutorialData.tunnelingStage);
    }

    public void IncreaseSuperpositionStage()
    {
        _tutorialData.superpositionStage++;
        ChangeModalContent();
        ChangeSelector();
        _superpositionTutorialStageChangeEvent.Invoke(_tutorialData.superpositionStage);
    }

    public void DecreaseSuperpositionStage()
    {
        _tutorialData.superpositionStage--;
        ChangeModalContent();
        ChangeSelector();
        _superpositionTutorialStageChangeEvent.Invoke(_tutorialData.superpositionStage);
    }

    public void IncreaseEntanglementStage()
    {
        _tutorialData.entanglementStage++;
        ChangeModalContent();
        ChangeSelector();
        _entanglementTutorialStageChangeEvent.Invoke(_tutorialData.entanglementStage);
    }

    public void DecreaseEntanglementStage()
    {
        _tutorialData.entanglementStage--;
        ChangeModalContent();
        ChangeSelector();
        _entanglementTutorialStageChangeEvent.Invoke(_tutorialData.entanglementStage);
    }

    public void HandleModalButtonClicked()
    {
        switch (_gameModeObject.GameMode)
        {
            case GameModeEnum.TUNNELING:
                IncreaseTunnelingStage();
                break;
            case GameModeEnum.SUPERPOSITION:
                IncreaseSuperpositionStage();
                break;
            case GameModeEnum.ENTANGLEMENT:
                IncreaseEntanglementStage();
                break;
        }
    }
    
    /*public void HandleReset()
    {
        _tutorialPanel.SetActive(false);
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
    }*/
    #endregion

    public void HandleTunnelingTutorialStageChanged(int stage)
    {
        switch (stage)
        {
            case 0:
                _tutorialData.ResetTunnelingStage();
                _gameModeObject.ResetTunnelingStep();
                _tutorialPanel.SetActive(true);
                _entangleButton.gameObject.SetActive(false);
                _playerInput.enabled = false;
                ChangeModalContent();
                ChangeSelector();
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
                break;
            case 2: 
                _board.Move(Vector2Int.left, 1, 1, 0, 1);
                break;
            
            case 6:
                _playerInput.enabled = true;
                _tutorialModal.gameObject.SetActive(false);
                _board.CreateNewTilesOnMove = true;
                _competencyCounter.gameObject.SetActive(true);
                break;
            
            case 7:
                _playerInput.enabled = false;
                _tutorialModal.gameObject.SetActive(true);
                _board.CreateNewTilesOnMove = false;
                _competencyCounter.gameObject.SetActive(false);
                GlobalData.level = "tunnelling2";
                _board.ClearBoard();
                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(1, 2));
                _board.CreateTile(_board.tileStates[4], _board.grid.GetCell(2, 2));
                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(3, 2));
                break;
            case 8:
                _board.Move(Vector2Int.left, 1, 1, 0, 1);
                break;
            case 9:
                GlobalData.level = "tunnelling2";
                _board.ClearBoard();
                _board.CreateTile(_board.tileStates[3], _board.grid.GetCell(1, 2));
                _board.CreateTile(_board.tileStates[4], _board.grid.GetCell(2, 2));
                _board.CreateTile(_board.tileStates[3], _board.grid.GetCell(3, 2));
                break;
            case 10:
                _board.Move(Vector2Int.left, 1, 1, 0, 1);
                break;
            case 11:
                _board.ClearBoard();
                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(2, 2));
                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(3, 2));
                break;
            case 12:
                _tutorialModal.gameObject.SetActive(false);
                _playerInput.enabled = true;
                _board.CreateNewTilesOnMove = true;
                break;
            
            // At the stage when the player is told to try it themselves, instantiate the competency counter
            // The competency counter will simply fill a bar if it detects a tunneling merge (or whichever special move it's hooked up to detect) while it is active.
        }
    }
    
    public void HandleSuperpositionTutorialStageChanged(int stage)
    {
        switch (stage)
        {
            case 0:
                _tutorialData.ResetSuperpositionStage();
                _gameModeObject.ResetSuperpositionStep();
                _entangleButton.gameObject.SetActive(false);
                _tutorialPanel.SetActive(true);
                _playerInput.enabled = false;
                ChangeModalContent();
                ChangeSelector();
                _board.TunnelingEnabled = false;
                _board.EntanglementEnabled = false;
                _board.CreateNewTilesOnMove = false;
                _gameOver.alpha = 0f;
                _gameOver.interactable = false;
                _board.ClearBoard();

                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(1, 2), false);
                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(2, 2), false);
                _board.enabled = true;
                break;
            case 1: 
                _board.Move(Vector2Int.left, 1, 1, 0, 1);
                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(3, 2), true);
                break;
            case 3:
                _board.ClearBoard();
                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(1, 2), true);
                _playerInput.enabled = true;
                break;
            case 4:
                _playerInput.enabled = false;
                break;
            case 6:
                _board.ClearBoard();
                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(0, 0), false);
                //_board.CreateTile(_board.tileStates[1], _board.grid.GetCell(0, 1), false);
                _board.CreateTile(_board.tileStates[2], _board.grid.GetCell(0, 2), false);
                _board.CreateTile(_board.tileStates[1], _board.grid.GetCell(0, 3), false);
                
                _board.CreateTile(_board.tileStates[1], _board.grid.GetCell(1, 0), false);
                _board.CreateTile(_board.tileStates[2], _board.grid.GetCell(1, 1), true);
                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(1, 2), false);
                _board.CreateTile(_board.tileStates[3], _board.grid.GetCell(1, 3), false);
                
                _board.CreateTile(_board.tileStates[4], _board.grid.GetCell(2, 0), false);
                _board.CreateTile(_board.tileStates[2], _board.grid.GetCell(2, 1), false);
                _board.CreateTile(_board.tileStates[3], _board.grid.GetCell(2, 2), false);
                _board.CreateTile(_board.tileStates[1], _board.grid.GetCell(2, 3), false);
                
                _board.CreateTile(_board.tileStates[2], _board.grid.GetCell(3, 0), false);
                _board.CreateTile(_board.tileStates[3], _board.grid.GetCell(3, 1), false);
                _board.CreateTile(_board.tileStates[4], _board.grid.GetCell(3, 2), false);
                _board.CreateTile(_board.tileStates[3], _board.grid.GetCell(3, 3), false);
                break;
            case 7:
                _board.Move(Vector2Int.left, 1, 1, 0, 1);
                _board.ClearCell(0, 1);
                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(0, 1), true);
                break;
            case 8:
                _board.SuperpositionEnabled = false;
                _board.Move(Vector2Int.up, 0, 1, 1, 1);
                break;
            case 9:
                _board.SuperpositionEnabled = true;
                break;
            case 10:
                _board.ClearBoard();
                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(3, 3), false);
                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(1, 0), false);
                _tutorialModal.gameObject.SetActive(false);
                _playerInput.enabled = true;
                _board.CreateNewTilesOnMove = true;
                break;
        }
    }
    
    public void HandleEntanglementTutorialStageChanged(int stage)
    {
        switch (stage)
        {
            case 0:
                _tutorialData.ResetEntanglementStage();
                _gameModeObject.ResetEntanglementStep();
                _tutorialPanel.SetActive(true);
                _playerInput.enabled = false;
                ChangeModalContent();
                ChangeSelector();
                _board.TunnelingEnabled = false;
                _board.SuperpositionEnabled = true;
                _board.CreateNewTilesOnMove = false;
                _gameOver.alpha = 0f;
                _gameOver.interactable = false;
                _board.ClearBoard();
        
                _board.CreateTile(_board.tileStates[0], _board.grid.GetCell(0, 0));
                _board.CreateTile(_board.tileStates[2], _board.grid.GetCell(3, 3));
                _board.enabled = true;
                break;
            case 2: 
                _board.Move(Vector2Int.down, 0, 1, _grid.Height - 2, -1);
                break;
        }
    }

}