using UnityEngine;

public class OilBullet: MonoBehaviour {
    public GameObject OilPuddlePrefab;
    
    private float m_time = 0f;

    private float m_speed = 0f;
    
    private Vector2 m_from;
    
    private Vector2 m_to;

    public void Init(Vector2 from, Vector2 to, float time) {
        m_speed = 1f / time;
        m_from = from;
        m_to = to;
    }

    private void Update() {

        m_time += Time.deltaTime * m_speed;
        transform.position = Vector2.Lerp(m_from, m_to, m_time);

        if(m_time > 1f) {
            Destroy(gameObject);
            Instantiate(OilPuddlePrefab, m_to, Quaternion.identity);
        }
    }
}
