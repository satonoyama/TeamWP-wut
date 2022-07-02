using System;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController Instance
    {
        get { return instance; }
    }

    private static EffectController instance;

    public enum EffectID
    {
        eRoar,
        eShockwave,
        eFog,
        eFogBreath,
        eBreath,
    }
    private EffectID effectID = EffectID.eRoar;

    [SerializeField] private GenerateEffectMap[] effects;
    private bool isStop = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!effects[(int)effectID].posObject || isStop) { return; }

        var pos = effects[(int)effectID].posObject.transform.position;
        var rot = effects[(int)effectID].posObject.transform.rotation;

        effects[(int)effectID].particle.transform.SetPositionAndRotation(pos, rot);

        for (int i = 0; i < effects[(int)effectID].se.Length; i++)
        {
            if (effects[(int)effectID].se[i])
            {
                effects[(int)effectID].se[i].transform.position = pos;
            }
        }
    }

    public void OnPlayParticle(EffectID id)
    {
        if (!effects[(int)id].particle) { return; }

        if(effects[(int)id].isSettingID)
        {
            effectID = id;
        }

        // 1ƒtƒŒ[ƒ€’x‚ê‚é‚Ì‚ð–h‚®‚½‚ß‚ÌÝ’è
        if (effects[(int)id].posObject)
        {
            var pos = effects[(int)id].posObject.transform.position;
            var rot = effects[(int)id].posObject.transform.rotation;

            effects[(int)id].particle.transform.SetPositionAndRotation(pos, rot);
        }

        effects[(int)id].particle.Play();

        for(int i = 0; i < effects[(int)id].se.Length; i++)
        {
            if (effects[(int)id].se[i])
            {
                effects[(int)id].se[i].Play();
            }
        }

        isStop = false;
    }

    public void OnStopParticle()
    {
        if (!effects[(int)effectID].particle ||
            !effects[(int)effectID].particle.isPlaying) { return; }

        effects[(int)effectID].particle.Stop();

        OnStopSE((int)effectID);

        isStop = true;
    }

    public void OnStopAllParticle()
    {
        for(int i = 0; i < effects.Length; i++)
        {
            if (!effects[i].particle.isPlaying) { continue; }

            effects[i].particle.Stop();

            OnStopSE(i);
        }

        isStop = true;
    }

    private void OnStopSE(int id)
    {
        for (int i = 0; i < effects[id].se.Length; i++)
        {
            if (!effects[id].se[i]) { continue; }

            if (effects[id].se[i].loop &&
                effects[id].se[i].isPlaying)
            {
                effects[id].se[i].Stop();
            }
        }
    }

    [Serializable]
    public class GenerateEffectMap
    {
        public EffectID id;
        public ParticleSystem particle = null;
        public GameObject posObject = null;
        public AudioSource[] se = null;
        public bool isSettingID = true;
    }
}
