using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleExplosion : MonoBehaviour
{
    public ParticleSystem explosion;

    public void ExplodePlay()
    {
        explosion.Play();
    }

}
