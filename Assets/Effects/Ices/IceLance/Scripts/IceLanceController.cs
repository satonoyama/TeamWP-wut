using System;
using UnityEngine;

public class IceLanceController : MonoBehaviour
{
    [SerializeField] private IceLance iceLanceOwner = null;
    [SerializeField] private ParticleSystem iceParticleOwner = null;
    [SerializeField] private PlayerStatus target = null;
    [SerializeField] private IceLanceGenInfoMap[] iceLances;

    public void OnGenerateIceLance()
    {
        if (!iceLanceOwner || !iceParticleOwner || !target) { return; }

        for(int i = 0; i < iceLances.Length; i++)
        {
            var pos = iceLances[i].generatePos.transform.position;
            var iceLance = Instantiate(iceLanceOwner, pos, gameObject.transform.rotation);

            var iceParticle = Instantiate(iceParticleOwner, pos, Quaternion.identity);

            iceLance.Initialize(target, iceParticle, iceLances[i].generatePos, iceLances[i].isTracingTarget);
        }
    }

    [Serializable]
    public class IceLanceGenInfoMap
    {
        [SerializeField] public GameObject generatePos;
        [SerializeField] public bool isTracingTarget = true;
    }
}
