using System;
using UnityEngine;

public class FallingLumpController : MonoBehaviour
{
    [SerializeField] private FallingLump fallingLumpOwner = null;
    [SerializeField] private ParticleSystem fogParticleOwner = null;
    [SerializeField] private ParticleSystem followParticleOwner = null;
    [SerializeField] private ParticleSystem explotionOwner = null;
    [SerializeField] private PlayerStatus target = null;
    [SerializeField] private FallingLumpGenInfoMap[] fallingLumps;

    public void OnGenerateIceLance()
    {
        if (!fallingLumpOwner || !fogParticleOwner || 
            !followParticleOwner || !target) { return; }

        Vector3 pos;
        var fogRot = fogParticleOwner.gameObject.transform.rotation;
        var explotionRot = explotionOwner.gameObject.transform.rotation;

        for(int i = 0; i < fallingLumps.Length; i++)
        {
            pos = target.transform.position;
            pos.y += fallingLumps[i].generatePosUpVal;

            if (fallingLumps[i].generatePos)
            {
                pos = fallingLumps[i].generatePos.transform.position;
            }

            var iceLance = Instantiate(fallingLumpOwner, pos, gameObject.transform.rotation);

            var explotion = Instantiate(explotionOwner, pos, explotionRot);
            var fog = Instantiate(fogParticleOwner, pos, fogRot);
            var follow = Instantiate(followParticleOwner, pos, Quaternion.identity);

            iceLance.SetParticles(explotion, fog, follow);
            iceLance.Initialize(target, pos, fallingLumps[i].isTracingTarget);
        }
    }

    [Serializable]
    public class FallingLumpGenInfoMap
    {
        [SerializeField] public GameObject generatePos = null;
        [SerializeField] public float generatePosUpVal = 0.0f;
        [SerializeField] public bool isTracingTarget = true;
    }
}
