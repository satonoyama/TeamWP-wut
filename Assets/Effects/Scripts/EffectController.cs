using System;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public enum EffectID
    {
        eRoar,
        eShockwave,
        eFog,
        eBreath,
    }
    private EffectID effectID = EffectID.eRoar;

    [SerializeField] private GenerateEffectMap[] effects;
    private bool isStop = false;

    private void Update()
    {
        if (!effects[(int)effectID].posObject || isStop) { return; }

        var pos = effects[(int)effectID].posObject.transform.position;
        var rot = effects[(int)effectID].posObject.transform.rotation;

        effects[(int)effectID].particle.transform.SetPositionAndRotation(pos, rot);
    }

    public void OnPlayParticle(EffectID id)
    {
        if (!effects[(int)id].particle) { return; }

        if(effects[(int)id].isSettingID)
        {
            effectID = id;
        }

        // 1ÉtÉåÅ[ÉÄíxÇÍÇÈÇÃÇñhÇÆÇΩÇﬂÇÃê›íË
        if (effects[(int)id].posObject)
        {
            var pos = effects[(int)id].posObject.transform.position;
            var rot = effects[(int)id].posObject.transform.rotation;

            effects[(int)id].particle.transform.SetPositionAndRotation(pos, rot);
        }

        effects[(int)id].particle.Play();

        isStop = false;
    }

    public void OnStopParticle()
    {
        if (!effects[(int)effectID].particle ||
            !effects[(int)effectID].particle.isPlaying) { return; }

        effects[(int)effectID].particle.Stop();

        isStop = true;
    }

    public void OnStopAllParticle()
    {
        for(int i = 0; i < effects.Length; i++)
        {
            if (!effects[i].particle.isPlaying) { continue; }

            effects[i].particle.Stop();
        }

        isStop = true;
    }

    [Serializable]
    public class GenerateEffectMap
    {
        public EffectID id;
        public ParticleSystem particle = null;
        public GameObject posObject = null;
        public bool isSettingID = true;
    }
}
