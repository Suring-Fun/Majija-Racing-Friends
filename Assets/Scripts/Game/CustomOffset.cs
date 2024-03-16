using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomOffset : MonoBehaviour
{
    public string OffsetName = "Offset";

    void Awake() {
        GetComponent<Animator>().SetFloat(OffsetName, Random.value);
    }
}

