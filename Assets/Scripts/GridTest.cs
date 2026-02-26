using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(Grid))]
public class GridTest : MonoBehaviour
{
    [SerializeField, ReadOnly] private Vector2Int _cursorGridPosition;
    [SerializeField, ReadOnly] private Vector2 _cursorSnappedToGrid;
    private Grid _grid;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
    }

    private void Update()
    {
        Mouse mouse = InputSystem.GetDevice<Mouse>();
        if (mouse == null) return;

        Vector2Control positionControl = mouse.position;
        Vector2 screenPosition = positionControl.value;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        _cursorGridPosition = _grid.GetGridPosition(worldPosition);
        _cursorSnappedToGrid = _grid.SnapWorldPositionToGrid(worldPosition);
    }
}
