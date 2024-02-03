using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SineRotater : MonoBehaviour
{
    public float CircleTime  = 2f;

    public float Mult = 1f;

    public float PhaseOffset = 0f;

    public float Range = 45f;

    public float AngleOffset = 90f;

    public float ShootTime = 1.5f;

    private float m_timePassed = 0f;

    public Transform Graphics;

    public Bullet BulletPrefab;

    public float Speed = 20f;

    public float MinShootLocalAngle = 20f;

    public float ShootTimePassed = -5f;

    void FixedUpdate() {
        m_timePassed += Time.deltaTime;

        float localAngle = Mathf.Sin(Mathf.PI * m_timePassed / CircleTime + Mathf.PI * PhaseOffset) * Mult * Range;
        float angle = localAngle + AngleOffset; 
        Graphics.localEulerAngles = Vector3.forward * angle;

        ShootTimePassed += Time.deltaTime;

        if(ShootTimePassed > ShootTime && Mathf.Abs(localAngle) > MinShootLocalAngle) {
            ShootTimePassed -= ShootTime;
            var bullet = Instantiate(BulletPrefab, transform.position, Quaternion.identity);
            bullet.Init(Graphics.up, default, Speed);
        }
    }
}
