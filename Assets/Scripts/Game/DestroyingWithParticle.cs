using UnityEngine;

public class DestroyingWithParticle : MonoBehaviour, IPreDestroying
{
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
    [field: SerializeField]
    public Vector3 Offset { get; private set; } = new Vector3(0, 0, -10);

    public void NotifyObjectAboutDeath()
    {
        Instantiate(Prefab, transform.position + Offset, Quaternion.identity);
    }
}
