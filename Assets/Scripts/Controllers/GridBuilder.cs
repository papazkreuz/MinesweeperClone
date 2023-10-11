using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GridBuilder
{
    private const float MAX_SCREEN_WIDTH = 1200f;
    private const float MAX_SCREEN_HEIGHT = 1000f;
    
    private readonly GridLayoutGroup _gridContainer;
    private readonly GridCellView _gridCellViewPrefab;
    private readonly int _gridWidth;
    private readonly int _gridHeight;
    
    [Inject] private readonly GridCellView.Factory _gridCellViewFactory;
    
    private List<GridCell> _gridCells;
    private List<GridCellView> _gridCellViews;
    
    public GridBuilder(GridLayoutGroup gridContainer, GridCellView gridCellViewPrefab, int gridWidth, int gridHeight)
    {
        _gridContainer = gridContainer;
        _gridCellViewPrefab = gridCellViewPrefab;
        _gridWidth = gridWidth;
        _gridHeight = gridHeight;
    }

    public List<GridCell> Build()
    {
        _gridCells = new List<GridCell>();
        _gridCellViews = new List<GridCellView>();
        
        float cellSize = Mathf.Min(MAX_SCREEN_WIDTH / _gridWidth, MAX_SCREEN_HEIGHT / _gridHeight);
        
        _gridContainer.constraintCount = _gridWidth;
        _gridContainer.cellSize = Vector2.one * cellSize;
        
        for (int i = 0; i < _gridHeight; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                GridCell gridCell = new GridCell(j, i);
                _gridCells.Add(gridCell);

                GridCellView gridCellView = _gridCellViewFactory.Create(_gridCellViewPrefab);
                _gridCellViews.Add(gridCellView);
                
                Transform gridCellViewTransform = gridCellView.transform;
                gridCellViewTransform.SetParent(_gridContainer.transform);
                gridCellViewTransform.position = Vector3.zero;
                gridCellViewTransform.localScale = Vector3.one;
                
                gridCellView.Init(gridCell);
            }
        }

        return _gridCells;
    }

    public void ClearContainer()
    {
        foreach (GridCellView gridCellView in _gridCellViews)
        {
            Object.Destroy(gridCellView.gameObject);
        }
    }
}