using System;
using UnityEngine;

public class AcidBlastSkillVisual : MonoBehaviour
{
    private const float scaleY = 60f;
    [SerializeField] private ParticleSystem trailParticleSystem, poisonMeshParticleSystem, waterBeamParticleSystem;

    private void Awake()
    {
        var trailMain = trailParticleSystem.main;
        var poisonMeshMain = poisonMeshParticleSystem.main;
        var waterBeamMain = waterBeamParticleSystem.main;

        trailMain.startSize3D = true;
        trailMain.startSizeY = scaleY;
        poisonMeshMain.startSize3D = true;
        poisonMeshMain.startSizeY = scaleY;
        waterBeamMain.startSize3D = true;
        waterBeamMain.startSizeY = scaleY;
    }
}