using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringParticle : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    float force;
    [SerializeField]
    ParticleSystem psp;
    [SerializeField]
    ParticleSystem pss;

    Vector3 activeSpring;

    //private void FixedUpdate()
    //{
    //    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[psp.particleCount];
    //    psp.GetParticles(particles);
    //    for (int i = 0; i < particles.Length; i++)
    //    {
    //        ParticleSystem.Particle p = particles[i];
    //        Vector3 pZ = new Vector3(p.position.x, p.position.y, player.position.z);
    //        p.position = pZ;
    //        particles[i] = p;
    //    }
    //    psp.SetParticles(particles, particles.Length);
    //}

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null)
            return;

        transform.position = player.position;

        Vector3 springPos = player.GetComponent<PlayerController>().ActiveSpringPoint;

        psp.transform.position = player.position;
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[psp.particleCount];
        psp.GetParticles(particles);
        for (int i = 0; i < particles.Length; i++)
        {
            ParticleSystem.Particle p = particles[i];
            Vector3 dir = springPos - transform.TransformPoint(p.position);
            p.position += dir.normalized * force * Time.deltaTime;

            particles[i] = p;
        }
        psp.SetParticles(particles, particles.Length);
        
        pss.transform.position = springPos;
        particles = new ParticleSystem.Particle[pss.particleCount];
        pss.GetParticles(particles);
        for (int i = 0; i < particles.Length; i++)
        {
            ParticleSystem.Particle p = particles[i];
            Vector3 dir = player.position - (springPos + p.position);
            p.position += dir.normalized * force * Time.deltaTime;

            particles[i] = p;
        }
        pss.SetParticles(particles, particles.Length);
    }
}
