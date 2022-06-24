using System;
using System.Collections.Generic;
using UnityEngine;

public class FallingLumpContainer : MonoBehaviour
{
    public static FallingLumpContainer Instance
    {
        get { return instance; }
    }

    private static FallingLumpContainer instance;

    private void Awake()
    {
        if (instance) throw new Exception("FallingLumpContainer instance already exists.");
        instance = this;

        Add();
    }

    [SerializeField] private FallingLump fallingLumpPrefab = null;
    [SerializeField] private FallingLumpInfo fallingLumInfo;
    [SerializeField] private FallingLumpGenInfoMap[] fallingLumps;

    // List to save generated particles
    private readonly Dictionary<int, FallingLumpInfo> fallingLumpMap = new();

    private void Add()
    {
        if ( !fallingLumpPrefab ||
             !fallingLumInfo.fogParticlePrefab ||
             !fallingLumInfo.followParticlePrefab ||
             !fallingLumInfo.explotionPrefab ||
             !fallingLumInfo.target) { return; }

        Vector3 pos;

        var fogRot = 
            fallingLumInfo.fogParticlePrefab
            .gameObject.transform.rotation;
        
        var explotionRot = 
            fallingLumInfo.explotionPrefab
            .gameObject.transform.rotation;

        for (int i = 0; i < fallingLumps.Length; i++)
        {
            pos = GetGeneratePos(i);

            var explotion = Instantiate(
                    fallingLumInfo.explotionPrefab, 
                    pos, explotionRot);
            
            var fog = Instantiate(
                fallingLumInfo.fogParticlePrefab,
                pos, fogRot);
            
            var follow = Instantiate(
                fallingLumInfo.followParticlePrefab,
                pos, Quaternion.identity);

            FallingLumpInfo info = new();
            info.fogParticlePrefab = fog;
            info.followParticlePrefab = follow;
            info.explotionPrefab = explotion;
            info.target = fallingLumInfo.target;

            fallingLumpMap.Add(i, info);
        }
    }

    public void OnGenerateIceLance()
    {
        Vector3 pos;

        for (int i = 0; i < fallingLumps.Length; i++)
        {
            pos = GetGeneratePos(i);

            var fallingLump = Instantiate(
                fallingLumpPrefab, pos,
                gameObject.transform.rotation);

            fallingLump.SetParticles(
                fallingLumpMap[i].explotionPrefab,
                fallingLumpMap[i].fogParticlePrefab,
                fallingLumpMap[i].followParticlePrefab);

            fallingLump.Initialize(
                fallingLumInfo.target, pos,
                fallingLumps[i].isTracingTarget);
        }
    }

    private Vector3 GetGeneratePos(int i)
    {
        Vector3 pos;

        pos = fallingLumInfo.target.transform.position;
        pos.y += fallingLumps[i].generatePosUpVal;

        if (fallingLumps[i].generatePos)
        {
            pos = fallingLumps[i].generatePos.transform.position;
        }

        return pos;
    }

    public void AllRemove()
    {
        fallingLumpMap.Clear();
    }

    [Serializable]
    public class FallingLumpInfo
    {
        [SerializeField] public ParticleSystem fogParticlePrefab = null;
        [SerializeField] public ParticleSystem followParticlePrefab = null;
        [SerializeField] public ParticleSystem explotionPrefab = null;
        [SerializeField] public PlayerStatus target = null;
    }

    [Serializable]
    public class FallingLumpGenInfoMap
    {
        [SerializeField] public GameObject generatePos = null;
        [SerializeField] public float generatePosUpVal = 0.0f;
        [SerializeField] public bool isTracingTarget = true;
    }
}
