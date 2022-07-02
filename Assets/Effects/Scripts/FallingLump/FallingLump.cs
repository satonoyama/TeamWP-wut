using System.Collections;
using UnityEngine;

public class FallingLump: MonoBehaviour
{
    private enum MoveState
    {
        eWait,
        eShoot,
        eHit
    }
    private MoveState status = MoveState.eWait;

    [SerializeField] private Collider hitDetector = null;
    [SerializeField] private float power = 1.0f;
    [SerializeField] private float moveSpeed = 1.0f;
    private PlayerStatus target = null;
    private EnemyStatus owner = null;
    private ParticleSystem fogParticle = null;
    private ParticleSystem followParticle = null;
    private ParticleSystem explotionParticle = null;
    private AudioSource followSE = null;
    private AudioSource generateSE = null;
    private AudioSource explotionSE = null;
    private bool isTracingTarget = false;

    private GameObject child = null;
    private int childrenNum = 0;

    [SerializeField] private float waitSeconds = 0.01f;
    private readonly int maxColorCount = 255;

    [SerializeField] private float lifeSpan = 0.0f;
    private Vector3 waitPos = new();
    private Vector3 moveVec = new();

    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private float rayDist = 1.0f;
    private Vector3 rayDir = new();
    private readonly RaycastHit[] raycastHits = new RaycastHit[1];

    private void SetPosByWaitPos() => transform.position = waitPos;

    private bool IsWait => status == MoveState.eWait;

    private bool IsHit => status == MoveState.eHit;

    private bool IsExecutable => !IsHit && target;

    private bool CheckCollisionByStage()
    {
        var pos = transform.position;

        int hitCount = Physics.RaycastNonAlloc(pos, rayDir, raycastHits, rayDist, raycastLayerMask);

        if (hitCount == 0) { return false; }

        return true;
    }

    public void SetParticles(ParticleSystem explotion, ParticleSystem fog, ParticleSystem follow)
    {
        explotionParticle = explotion;
        fogParticle = fog;
        followParticle = follow;
    }

    public void SetSE(AudioSource generate, AudioSource explotion, AudioSource follow)
    {
        generateSE = generate;
        explotionSE = explotion;
        followSE = follow;
    }

    private void SetChildrenColor(byte A)
    {
        Color32 color = child.GetComponent<MeshRenderer>().material.color;
        byte R, G, B;
        R = color.r;
        G = color.g;
        B = color.b;

        child.GetComponent<MeshRenderer>().material.color = new Color32(R, G, B, A);
    }

    public void Initialize(
        PlayerStatus player, EnemyStatus myOwner, 
        Vector3 waitPosition, bool isTracing)
    {
        target = player;
        owner = myOwner;
        waitPos = waitPosition;
        isTracingTarget = isTracing;

        if (fogParticle)
        {
            fogParticle.transform.position = waitPos;
            fogParticle.Play();
        }

        if (followParticle)
        {
            followParticle.transform.position = transform.position;
            followParticle.Play();
        }

        if (!isTracingTarget)
        {
            var rot = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            transform.rotation = rot;
        }

        if(generateSE)
        {
            generateSE.transform.position = waitPos;
            generateSE.Play();
        }

        if(followSE)
        {
            followSE.transform.position = waitPos;
            followSE.Play();
        }

        WeakPointContainer.Instance.Add(hitDetector, true);
        WeakPointContainer.Instance.GetWeakPoint(hitDetector).OnActive();

        StartCoroutine(Transparent());
    }

    private void Start()
    {
        Transform children = gameObject.GetComponentInChildren<Transform>();
        childrenNum = children.childCount - 1;

        for (int i = 0; i < childrenNum; i++)
        {
            child = transform.GetChild(i).gameObject;

            SetChildrenColor(0);
            child.GetComponent<MeshCollider>().enabled = false;
        }
    }

    private void Update()
    {
        if (!IsExecutable) { return; }

        if(owner.IsDie)
        {
            StopFallingLump();
            return;
        }

        UpdateMove();

        UpdateRotateByTarget();
    }

    private void UpdateMove()
    {
        if(IsWait)
        {
            SetPosByWaitPos();
            return;
        }

        if(!isTracingTarget)
        {
            moveVec.y = Physics.gravity.y * Time.deltaTime;
        }

        transform.position += moveVec;

        if (followParticle)
        {
            followParticle.transform.position = transform.position;
        }

        if (followSE && followSE.isPlaying)
        {
            followSE.transform.position = transform.position;
        }

        if (CheckCollisionByStage())
        {
            StopFallingLump();
        }

        if((lifeSpan -= Time.deltaTime) <= 0.0f)
        {
            BreakFallingLump();
        }
    }

    private void UpdateRotateByTarget()
    {
        if (!IsWait || !isTracingTarget) { return; }

        transform.LookAt(target.transform);
    }

    private void OnShoot()
    {
        status = MoveState.eShoot;

        if(isTracingTarget)
        {
            moveVec = target.transform.position - transform.position;
        }
        else
        {
            moveVec = Vector3.down;
        }

        if (generateSE)
        {
            generateSE.Stop();
        }

        rayDir = moveVec;

        // Move Speed Up or Down
        moveVec *= moveSpeed;
    }

    public void BreakFallingLump()
    {
        status = MoveState.eHit;

        transform.GetComponent<Collider>().enabled = false;
        
        if(WeakPointContainer.Instance.GetWeakPoint(hitDetector))
        {
            WeakPointContainer.Instance.Remove(hitDetector);
            hitDetector.enabled = false;
        }

        if (fogParticle && fogParticle.isPlaying) { fogParticle.Stop(); }
        if (followParticle && followParticle.isPlaying) { followParticle.Stop(); }

        if (explotionParticle)
        {
            explotionParticle.transform.position = transform.position;
            explotionParticle.Play();
        }

        if(explotionSE)
        {   
            explotionSE.transform.position = transform.position;
            explotionSE.Play();
        }

        for (int i = 0; i < childrenNum; i++)
        {
            child = transform.GetChild(i).gameObject;
            
            SetChildrenColor(255);
            
            child.GetComponent<Rigidbody>().useGravity = true;
            child.GetComponent<MeshCollider>().enabled = true;
        }

        StartCoroutine(Transparent());
    }

    private void StopFallingLump()
    {
        moveVec = Vector3.zero;
        BreakFallingLump();
    }

    public void OnHitTarget()
    {
        if (!IsExecutable) { return; }

        target.Damage(power);

        BreakFallingLump();
    }

    public void OnShootingDown(GameObject magicObj)
    {
        if (!IsExecutable) { return; }

        var magic = magicObj.GetComponent<MagicalFX.MagicInfo>();

        if (!magic) { return; }

        BreakFallingLump();
    }

    private IEnumerator Transparent()
    {
        for (int i = 0; i < maxColorCount; i++)
        {
            for (int num = 0; num < childrenNum; num++)
            {
                child = transform.GetChild(num).gameObject;

                if (IsWait)
                {
                    child.GetComponent<MeshRenderer>().material.color += new Color32(0, 0, 0, 1);

                    if(i >= maxColorCount - 1) { OnShoot(); }
                }
                else if(IsHit)
                {
                    child.GetComponent<MeshRenderer>().material.color -= new Color32(0, 0, 0, 1);

                    if(i >= maxColorCount - 1) 
                    {
                        if (explotionParticle && explotionParticle.isPlaying) 
                        { explotionParticle.Stop(); }

                        if (followSE) { followSE.Stop(); }

                        Destroy(gameObject);
                    }
                }
            }
            yield return new WaitForSeconds(waitSeconds);
        }
    }
}
