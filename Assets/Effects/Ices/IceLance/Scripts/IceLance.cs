using System.Collections;
using UnityEngine;

public class IceLance : MonoBehaviour
{
    private enum MoveState
    {
        eWait,
        eShoot,
        eHit
    }
    private MoveState status = MoveState.eWait;

    [SerializeField] private float power = 1.0f;
    [SerializeField] private float moveSpeed = 1.0f;
    private PlayerStatus target = null;
    private ParticleSystem iceParticle = null;
    private bool isTracingTarget = false;

    private GameObject child = null;
    private int childrenNum = 0;

    [SerializeField] private float waitSeconds = 0.01f;
    private readonly int maxColorCount = 255;

    private GameObject waitPos = null;
    private Vector3 moveVec = new();

    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private float rayDist = 1.0f;
    private Vector3 rayDir = new();
    private readonly RaycastHit[] raycastHits = new RaycastHit[1];

    private void SetPosByWaitPos() => transform.position = waitPos.transform.position;

    private bool IsWait => status == MoveState.eWait;

    private bool IsHit => status == MoveState.eHit;

    private bool IsExecutable => !IsHit && target;

    private bool CheckIsHit()
    {
        var pos = transform.position;

        int hitCount = Physics.RaycastNonAlloc(pos, rayDir, raycastHits, rayDist, raycastLayerMask);

        if (hitCount == 0) { return false; }

        return true;
    }

    public void Initialize(PlayerStatus player, ParticleSystem particle, GameObject waitPosObj, bool isTracing)
    {
        target = player;
        iceParticle = particle;
        waitPos = waitPosObj;
        isTracingTarget = isTracing;

        if (iceParticle) 
        {
            iceParticle.transform.position = transform.position;
            iceParticle.Play();
        }

        StartCoroutine(Transparent());
    }

    private void Start()
    {
        Transform children = gameObject.GetComponentInChildren<Transform>();
        childrenNum = children.childCount - 1;

        Color32 color;
        byte R, G, B;

        for (int i = 0; i < childrenNum; i++)
        {
            child = transform.GetChild(i).gameObject;

            color = child.GetComponent<MeshRenderer>().material.color;

            R = color.r;
            G = color.g;
            B = color.b;

            child.GetComponent<MeshRenderer>().material.color = new Color32(R, G, B, 0);
        }
    }

    private void Update()
    {
        if (!IsExecutable) { return; }

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

        if (iceParticle)
        {
            iceParticle.transform.position = transform.position;
        }

        if (CheckIsHit())
        {
            StopIceLance();
        }
    }

    private void UpdateRotateByTarget()
    {
        if (!IsWait) { return; }

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
            Quaternion rot = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            transform.rotation = rot * waitPos.transform.rotation;
        }

        rayDir = moveVec;

        // Move Speed Up or Down
        moveVec *= moveSpeed;
    }

    private void StopIceLance()
    {
        status = MoveState.eHit;
        moveVec = Vector3.zero;

        if (iceParticle && iceParticle.isPlaying) { iceParticle.Stop(); }

        for (int i = 0; i < childrenNum; i++)
        {
            child = transform.GetChild(i).gameObject;
            child.GetComponent<Rigidbody>().useGravity = true;
        }

        StartCoroutine(Transparent());
    }

    public void OnHitIceLance()
    {
        if (!IsExecutable) { return; }

        target.Damage(power);

        StopIceLance();
    }

    public void OnShootingDown()
    {
        if (!IsExecutable) { return; }

        StopIceLance();
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
                }
                else
                {
                    child.GetComponent<MeshRenderer>().material.color -= new Color32(0, 0, 0, 1);
                }
            }
            yield return new WaitForSeconds(waitSeconds);
        }

        if (IsWait)
        {
            OnShoot();
        }

        if(IsHit)
        {
            Destroy(gameObject);
        }
    }
}
