using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToRoadSnapper : MonoBehaviour
{
    public float Offset = 0f;

    public bool AddRoadRadius;

    public float RoadRadiusMult = 1f;

    public bool ReverseAngle;

    public void Start()
    {
        var pd = FindObjectOfType<PathData>();
        var data = pd.GetNearestPoint(transform.position);

        float offset = Offset;

        if (AddRoadRadius)
            offset += data.radius * RoadRadiusMult;

        Vector2 direction = data.direction;

        transform.position = data.point + new Vector2(direction.y, -direction.x) * offset;

        if (ReverseAngle)
            direction = -direction;
            
        transform.eulerAngles = Vector3.forward * (Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg);

    }
}
