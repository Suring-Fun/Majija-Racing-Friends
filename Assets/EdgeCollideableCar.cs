using UnityEngine;

public class EdgeCollideableCar : MonoBehaviour
{
    private Movenment m_movable;

    private ShockableCar m_shockable;

    // Start is called before the first frame update
    void Start()
    {
        m_movable = GetComponent<Movenment>();
        m_shockable = GetComponent<ShockableCar>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var trck = m_movable.Tracking;

        if (trck.CarInTheWater && trck.EdgeIsSolid)
        {
            var vector = m_movable.Rigidbody2D.velocity;

            if(Vector2.Dot(trck.DirectionToRoadCenter, vector) < 0f) {
                vector = Vector2.Reflect(vector, trck.DirectionToRoadCenter);
            }

            m_movable.Rigidbody2D.position = trck.RoadCenter - trck.DirectionToRoadCenter * trck.DeepWaterDistance;
            m_shockable.Shock(vector.normalized, vector.magnitude);
        }
    }
}
