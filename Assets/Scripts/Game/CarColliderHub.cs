using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColliderHub : MonoBehaviour
{
    [field: SerializeField] public Collider2D ToCollectPrizes { get; private set; }
    [field: SerializeField] public Collider2D ToGetOrTakeHit { get; private set; }
}
