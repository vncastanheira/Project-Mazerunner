using UnityEngine;
using UnityEngine.Networking;

public class ParticlesController : NetworkBehaviour
{
    ParticleSystem particles;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (isServer)
        {
            if (!particles.IsAlive())
            {
                NetworkServer.Destroy(gameObject);
            }
        }
    }
}
