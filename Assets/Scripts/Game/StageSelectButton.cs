using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageSelectButton : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public StageInfo StageInfo { get; private set; }

    [field: SerializeField] public GameObject SelectionGO { get; private set; }
    
    [field: SerializeField] public GameObject CheckedMark { get; private set; }
    [field: SerializeField] public GameObject UnavailableMark { get; private set; }

    public event Action<StageSelectButton> Clicked;

    public bool Selected {
        set => SelectionGO.SetActive(value);
    }

    public void UpdateStatusWithLevel(int currentPlayerLevel) {
        CheckedMark.SetActive(currentPlayerLevel > StageInfo.LevelRequired);
        UnavailableMark.SetActive(currentPlayerLevel < StageInfo.LevelRequired);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke(this);
    }
}
