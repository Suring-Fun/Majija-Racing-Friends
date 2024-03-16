using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgressVisualizer : MonoBehaviour
{
    [Serializable]
    public class PlayerGraphicalInfo
    {
        [field: SerializeField]
        public Sprite[] NormalState { get; private set; }

        [field: SerializeField]
        public Sprite[] ScareState { get; private set; }
    }

    [field: SerializeField]
    public PlayerGraphicalInfo[] GraphicalInfoPerLevel { get; private set; }

    [field: SerializeField]
    public float TimePerFrame { get; private set; } = 1f / 12f;

    [field: SerializeField]
    public Image PlayerContainer { get; private set; }

    [field: SerializeField]
    public StageSelectionManager SelectionManager { get; private set; }

    void Update()
    {
        PlayerProgress progress = PlayerProgress.Main;
        PlayerGraphicalInfo gi = GraphicalInfoPerLevel[Mathf.Clamp(progress.PlayerLevel, 0, GraphicalInfoPerLevel.Length - 1)];

        Sprite[] animSource = progress.PlayerLevel < SelectionManager.Info.LevelRequired ? gi.ScareState : gi.NormalState;
        Sprite current = animSource[(int)(Time.time / TimePerFrame) % animSource.Length];

        PlayerContainer.sprite = current;
    }
}
