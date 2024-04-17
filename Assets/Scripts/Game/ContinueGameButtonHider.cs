using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueGameButtonHider : MonoBehaviour
{
    void Start() {
        GetComponent<Button>().interactable = (PlayerProgress.Main.PlayerLevel >= 0);

    }
}
