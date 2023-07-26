using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    PlayerModel _controller;
    public List<ParticleSystem> flamethrower = new List<ParticleSystem>();

    void Start()
    {
        _controller = GetComponent<PlayerModel>();
        StopParticles(flamethrower);

    }

    private void Update()
    {
        if (_controller.IsShooting)
            PlayParticles(flamethrower);
        else
        {
            StopParticles(flamethrower);            
        }
    }


    public void PlayParticles(List<ParticleSystem> particles)
    {
        foreach (var particle in particles)
        {
            if (!particle.isPlaying)
                particle.Play();
        }
    }

    public void StopParticles(List<ParticleSystem> particles)
    {
        foreach (var particle in particles)
        {
            if (particle.isPlaying)
                particle.Stop();
        }
    }
}
