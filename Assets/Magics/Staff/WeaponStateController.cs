using System;
using System.Collections;
using System.Collections.Generic;
using MagicalFX;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponStateController : MonoBehaviour
{
    [SerializeField] Transform DefaultPosition, ADSPosition;
    [SerializeField] float StaffHoldSpeed = 15.0f;

    enum NowState
    {
        DEFAULT, ADS
    }

    private NowState nowState;
    bool refState;

    private Transform targetTransform;
    public Vector3 NowPosition { get; private set; }
    public Quaternion NowRotation { get; private set; }

    void Awake()
    {
        transform.root.gameObject.GetComponent<Wizard>().
            Controls.Magic.ADS.performed += SwitchHoldState;

        // ‰ŠúÀ•W
        targetTransform = DefaultPosition;
        NowPosition = targetTransform.position;

    }

    void LateUpdate()
    {
        if (!(Time.deltaTime > 0f)) return;

        NowPosition = Vector3.Lerp(NowPosition, targetTransform.position, StaffHoldSpeed * Time.deltaTime);
        NowRotation = Quaternion.Lerp(NowRotation, targetTransform.rotation, StaffHoldSpeed * Time.deltaTime);
        UpdateFov();
    }

    [SerializeField] private Camera playerCam;
    [SerializeField] private int defualtFov = 60;
    [SerializeField] private float ADSFovRatio = 1.15f;
    [SerializeField] private float FovChangingSpeed = 14f;
    void UpdateFov()
    {
        switch (nowState)
        {
            case NowState.DEFAULT:
                playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, defualtFov * ADSFovRatio, 
                    FovChangingSpeed * Time.deltaTime);
                break;
            case NowState.ADS:
                playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, defualtFov,
                    FovChangingSpeed * Time.deltaTime);
                break;
            default:
                return;
        }
    }

    void SwitchHoldState(InputAction.CallbackContext context)
    {
        switch (nowState)
        {
            case NowState.DEFAULT:
                nowState = NowState.ADS;
                targetTransform = ADSPosition;
                break;
            case NowState.ADS:
                nowState = NowState.DEFAULT;
                targetTransform = DefaultPosition;
                break;
            default:
                return;
        }
    }
    public void _SwitchHoldState(bool shootState)
    {
        if (Convert.ToInt32(shootState) == (int)nowState) return;

        switch (nowState)
        {
            case NowState.DEFAULT:
                nowState = NowState.ADS;
                targetTransform = ADSPosition;
                break;
            case NowState.ADS:
                nowState = NowState.DEFAULT;
                targetTransform = DefaultPosition;
                break;
            default:
                return;
        }
    }
}
