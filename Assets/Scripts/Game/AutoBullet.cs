using System.Linq;
using UnityEngine;

public class AutoBullet : MonoBehaviour
{
    private Rigidbody2D m_rigidbody2D;

    private PathData m_pathData;

    [field: SerializeField]
    public Transform Target { get; set; }

    public float Speed = 30f;

    public float MaxAngleFromRoad = 45f;

    [field: SerializeField]
    public Transform Graphics { get; private set; }

    private void Awake()
    {
        m_pathData = FindObjectOfType<PathData>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

    }

    public void Init(Transform myCar)
    {
        Physics2D.IgnoreCollision(myCar.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        var cars = FindObjectsOfType<Movenment>().Select(x => x.GetComponent<Transform>()).Where(t => t != myCar).ToArray();

        float d = float.PositiveInfinity;
        for (int x = 0; x < cars.Length; ++x)
        {
            float cd = -Vector2.Dot(cars[x].position - myCar.position, myCar.up);

            if (cd < d)
            {
                d = cd;
                Target = cars[x];
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        IPreDestroying.NotifyObjectAboutDeath(gameObject);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        // Due to small & fixed steps this approximation works.
        Vector2 mineTargetPos = transform.position;
        var myCoords = m_pathData.GetLocationAtTrack(transform.position);

        mineTargetPos += myCoords.roadDirection * Speed * Time.deltaTime;

        var myTargetCoords = m_pathData.GetLocationAtTrack(mineTargetPos);
        var targetCoords = m_pathData.GetLocationAtTrack(Target.position);


        float targetPos01 = targetCoords.distance / targetCoords.radius;
        float mineTargetPos01 = myTargetCoords.distance / myTargetCoords.radius;

        float delta01 = targetPos01 - mineTargetPos01;
        mineTargetPos += (Vector2)Vector3.Cross(-Vector3.forward, myTargetCoords.roadDirection) * (delta01 * myTargetCoords.radius);

        var dir = (mineTargetPos - (Vector2)transform.position).normalized;
        float dirAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float roadDirAngle = Mathf.Atan2(myTargetCoords.roadDirection.y, myTargetCoords.roadDirection.x) * Mathf.Rad2Deg;

        float angleDelta = Mathf.Clamp(Mathf.DeltaAngle(roadDirAngle, dirAngle), -MaxAngleFromRoad, +MaxAngleFromRoad);
        float resultAngle = roadDirAngle + angleDelta;
        float resultDegreesAngle = resultAngle;
        resultAngle *= Mathf.Deg2Rad;

        dir = new(Mathf.Cos(resultAngle), Mathf.Sin(resultAngle));

        m_rigidbody2D.velocity = dir * Speed;

        //Let's rotate the graphics across the road direction axis.
        Graphics.transform.eulerAngles = Vector3.forward *  (resultDegreesAngle - 90f);       
    }
}