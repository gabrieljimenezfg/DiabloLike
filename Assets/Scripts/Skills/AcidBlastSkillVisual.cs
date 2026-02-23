using System;
using UnityEngine;

public class AcidBlastSkillVisual : MonoBehaviour
{
    private const float scaleY = 60f;
    private const float scaleXZ = 3f;
    [SerializeField] private ParticleSystem trailParticleSystem, poisonMeshParticleSystem, waterBeamParticleSystem;

    private void Awake()
    {
        var scale = new Vector3(scaleXZ, scaleY, scaleXZ);

        SetParticleScale(trailParticleSystem, scale);
        SetParticleScale(poisonMeshParticleSystem, scale);
        SetParticleScale(waterBeamParticleSystem, scale);
    }

    private void SetParticleScale(ParticleSystem particleObject, Vector3 scale)
    {
        var main = particleObject.main;

        main.startSize3D = true;
        main.startSizeX = scale.x;
        main.startSizeY = scale.y;
        main.startSizeZ = scale.z;
    }
}