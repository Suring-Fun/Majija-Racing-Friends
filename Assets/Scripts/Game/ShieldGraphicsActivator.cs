using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGraphicsActivator : MonoBehaviour
{
    public GameObject Graphics;

    void Awake() {
        GetComponentInParent<ShockableCar>().IgnoreCollisionShocksChanged += Handle;
    }

    private void Handle(ShockableCar car)
    {
        Graphics.SetActive(car.IgnoreCollisionShocks > 0);
    }
}
