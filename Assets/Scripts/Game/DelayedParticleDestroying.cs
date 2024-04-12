using UnityEngine;

public class DelayedParticleDestroying : MonoBehaviour, IPreDestroying
{
    [field: SerializeField]
    public float Delay { get; private set; } = 5f;
    public void OnNotifiedObjectAboutDeath()
    {
        transform.parent = null;
        var particleSystem = GetComponent<ParticleSystem>();
        if (particleSystem)
        {
            var emission = particleSystem.emission;
            emission.enabled = false;
        }
        Destroy(gameObject, Delay);
    }
}
