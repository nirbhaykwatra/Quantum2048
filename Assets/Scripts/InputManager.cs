using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private TileBoard _board;
    private bool _pressed = false;

    private void Awake()
    {
        _board = GetComponentInChildren<TileBoard>();
    }

    public void OnPress(InputValue value)
    {
        _pressed = value.isPressed;
    }

    public void OnMoveUp(InputValue value)
    {
        Debug.Log("OnMoveUp");
        if (!_board.Waiting) 
        {
            _board.Move(Vector2Int.up, 0, 1, 1, 1);
        }
    }

    public void OnMoveDown(InputValue value)
    {
        Debug.Log("OnMoveDown");
        if (!_board.Waiting) 
        {
            _board.Move(Vector2Int.down, 0, 1, _board.grid.Height - 2, -1);
        }
    }

    public void OnMoveLeft(InputValue value)
    {
        Debug.Log("OnMoveLeft");
        if (!_board.Waiting) 
        {
            _board.Move(Vector2Int.left, 1, 1, 0, 1);
        }
    }

    public void OnMoveRight(InputValue value)
    {
        Debug.Log("OnMoveRight");
        if (!_board.Waiting) 
        {
            _board.Move(Vector2Int.right, _board.grid.Width - 2, -1, 0, 1);
        }
    }

    public void OnSwipeUp(InputValue value)
    {
        Debug.Log($"OnSwipeUp: {value.Get()}");
        if (!_board.Waiting && value.Get<float>() > 2f) 
        {
            _board.Move(Vector2Int.up, 0, 1, 1, 1);
        }
    }

    public void OnSwipeDown(InputValue value)
    {
        Debug.Log($"OnSwipeDown: {value.Get()}");
        if (!_board.Waiting && value.Get<float>() > 2f) 
        {
            _board.Move(Vector2Int.down, 0, 1, _board.grid.Height - 2, -1);
        }
    }

    public void OnSwipeLeft(InputValue value)
    {
        Debug.Log($"OnSwipeLeft: {value.Get()}");
        if (!_board.Waiting && value.Get<float>() > 2f) 
        {
            _board.Move(Vector2Int.left, 1, 1, 0, 1);
        }
    }

    public void OnSwipeRight(InputValue value)
    {
        Debug.Log($"OnSwipeRight: {value.Get()}");
        if (!_board.Waiting && value.Get<float>() > 2f) 
        {
            _board.Move(Vector2Int.right, _board.grid.Width - 2, -1, 0, 1);
        }
    }
}
