using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeftRightPointerClickHandler : MonoBehaviour, IPointerClickHandler
{
    private Action _leftClicked;
    private Action _rightClicked;

    public void Initialize(Action leftClicked, Action rightClicked)
    {
        _leftClicked = leftClicked;
        _rightClicked = rightClicked;
    }

    public void Dispose()
    {
        _leftClicked = null;
        _rightClicked = null;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                _leftClicked?.Invoke();
                break;
            case PointerEventData.InputButton.Right:
                _rightClicked?.Invoke();
                break;
            case PointerEventData.InputButton.Middle:
            default:
                break;
        }
    }
}
