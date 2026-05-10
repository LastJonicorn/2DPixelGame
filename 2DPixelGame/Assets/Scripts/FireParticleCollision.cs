using UnityEngine;
using System.Collections.Generic;

public class FireParticleCollision : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag("Player"))
            return;

        List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();

        int count = ps.GetCollisionEvents(other, events);

        ParticleSystem.Particle[] particles =
            new ParticleSystem.Particle[ps.main.maxParticles];

        int alive = ps.GetParticles(particles);

        for (int i = 0; i < alive; i++)
        {
            for (int j = 0; j < count; j++)
            {
                Vector3 hitPos = events[j].intersection;

                // etsi lähellä oleva particle
                if (Vector3.Distance(
                    particles[i].position + transform.position,
                    hitPos
                ) < 0.2f)
                {
                    particles[i].remainingLifetime = 0f;
                }
            }
        }

        ps.SetParticles(particles, alive);
    }
}