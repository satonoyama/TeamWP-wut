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
    [SerializeField] private FallingLumpInfo fallingLumpInfo;
    [SerializeField] private FallingLumpGenInfoMap[] fallingLumps;

    // List to save generated particles
    private readonly Dictionary<int, FallingLumpInfo> fallingLumpMap = new();

    private bool IsSetting()
    {
        return  fallingLumpPrefab                    &&
                fallingLumpInfo.fogParticlePrefab    &&
                fallingLumpInfo.followParticlePrefab &&
                fallingLumpInfo.explotionPrefab      &&
                fallingLumpInfo.followSE             &&
                fallingLumpInfo.generateSE           &&
                fallingLumpInfo.explotionSE          &&
                fallingLumpInfo.target               &&
                fallingLumpInfo.owner;
    }

    private void Add()
    {
        if ( !IsSetting()) { return; }

        Vector3 pos;

        var fogRot = 
            fallingLumpInfo.fogParticlePrefab
            .gameObject.transform.rotation;
        
        var explotionRot = 
            fallingLumpInfo.explotionPrefab
            .gameObject.transform.rotation;

        for (int i = 0; i < fallingLumps.Length; i++)
        {
            pos = GetGeneratePos(i);

            var explotion = Instantiate(
                    fallingLumpInfo.explotionPrefab, 
                    pos, explotionRot);
            
            var fog = Instantiate(
                fallingLumpInfo.fogParticlePrefab,
                pos, fogRot);
            
            var follow = Instantiate(
                fallingLumpInfo.followParticlePrefab,
                pos, Quaternion.identity);

            FallingLumpInfo info = new();
            info.fogParticlePrefab = fog;
            info.followParticlePrefab = follow;
            info.explotionPrefab = explotion;
            info.generateSE = fallingLumpInfo.generateSE;
            info.explotionSE = fallingLumpInfo.explotionSE;
            info.followSE = fallingLumpInfo.followSE;

            fallingLumpMap.Add(i, info);
        }
    }

    public void OnGenerateIceLance()
    {
        Vector3 pos;

        for (int i = 0; i < fallingLumps.Length; i++)
        {
            pos = GetGeneratePos(i);

            var checkCollider = Instantiate(
                fallingLumpInfo.checkColliderPrefab,
                pos, Quaternion.identity);

            var fallingLump = Instantiate(
                fallingLumpPrefab, pos,
                gameObject.transform.rotation);

            checkCollider.SetOwner(fallingLump);

            fallingLump.SetParticles(
                fallingLumpMap[i].explotionPrefab,
                fallingLumpMap[i].fogParticlePrefab,
                fallingLumpMap[i].followParticlePrefab);

            fallingLump.SetSE(
                fallingLumpMap[i].generateSE,
                fallingLumpMap[i].explotionSE,
                fallingLumpMap[i].followSE);

            fallingLump.Initialize(
                fallingLumpInfo.target, 
                fallingLumpInfo.owner,
                pos,
                fallingLumps[i].isTracingTarget);
        }
    }

    private Vector3 GetGeneratePos(int i)
    {
        Vector3 pos;

        pos = fallingLumpInfo.target.transform.position;
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
        [SerializeField] public AudioSource followSE = null;
        [SerializeField] public AudioSource generateSE = null;
        [SerializeField] public AudioSource explotionSE = null;
        [SerializeField] public PlayerStatus target = null;
        [SerializeField] public EnemyStatus owner = null;
        [SerializeField] public MagicHitChecker checkColliderPrefab = null;
    }

    [Serializable]
    public class FallingLumpGenInfoMap
    {
        [SerializeField] public GameObject generatePos = null;
        [SerializeField] public float generatePosUpVal = 0.0f;
        [SerializeField] public bool isTracingTarget = true;
    }
}
