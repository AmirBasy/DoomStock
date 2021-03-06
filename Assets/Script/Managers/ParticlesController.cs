﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{

    public ParticleSystem Destruction;
    public ParticleSystem SmallFire;
    public ParticleSystem BigFire;
    public ParticleSystem Smoke;
    public ParticleSystem AttackTorretta;

    #region API
    public void Init()
    {
        Destruction.Stop();
        SmallFire.Stop();
        BigFire.Stop();
        Smoke.Stop();
    }

    /// <summary>
    /// Play the Particles effect
    /// </summary>
    public void PlayParticles(ParticlesType _type)
    {
        switch (_type)
        {
            case ParticlesType._destruction:
                if (Destruction.isPlaying)
                    Destruction.Stop();
                Destruction.Play();
                break;
            case ParticlesType._smallFire:
                if (SmallFire.isPlaying)
                    SmallFire.Stop();
                SmallFire.Play();
                break;
            case ParticlesType._bigFire:
                if (BigFire.isPlaying)
                    BigFire.Stop();
                BigFire.Play();
                break;
            case ParticlesType._smoke:
                if (Smoke.isPlaying)
                    Smoke.Stop();
                Smoke.Play();
                break;
            case ParticlesType._attackTorretta:
                if (AttackTorretta != null)
                {
                    if (AttackTorretta.isPlaying)
                        AttackTorretta.Stop();
                    AttackTorretta.Play(); 
                }
                break;
            default:
                break;
        }

    }
    /// <summary>
    /// Stop the particles effect
    /// </summary>
    public void StopParticles(ParticlesType _type)
    {
        if (_type != null)
        {
            switch (_type)
            {
                case ParticlesType._destruction:
                    Destruction.Stop();
                    break;
                case ParticlesType._smallFire:
                    SmallFire.Stop();
                    break;
                case ParticlesType._bigFire:
                    BigFire.Stop();
                    break;
                case ParticlesType._smoke:
                    Smoke.Stop();
                    break;
                case ParticlesType._attackTorretta:
                    if (AttackTorretta != null)
                    {
                        AttackTorretta.Stop(); 
                    }
                    break;
                default:
                    break;
            }
        }
    }
    #endregion


}
public enum ParticlesType
{
    _destruction,
    _smallFire,
    _bigFire,
    _smoke,
    _attackTorretta
}