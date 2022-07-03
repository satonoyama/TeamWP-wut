using System.Collections;
using System.Collections.Generic;
using MagicalFX;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class HandheldMoving : MonoBehaviour
{
    private MovementController movementController;
    private WeaponStateController weaponStateController;

    Vector3 m_WeaponBobLocalPosition;

    void Start()
    {
        movementController = transform.root.gameObject.GetComponent<MovementController>();
        weaponStateController = transform.parent.gameObject.GetComponent<WeaponStateController>();

        newWeaponRot = transform.localRotation.eulerAngles;
        controller = transform.root.gameObject.GetComponent<MovementController>();
        controller.Controls.Movement.Look.performed += context => mouseDelta = context.ReadValue<Vector2>();
    }

    void LateUpdate()
    {
        if (!(Time.deltaTime > 0f)) return;
        
        UpdateSway();
        UpdateBob();

        transform.position = weaponStateController.NowPosition + m_WeaponBobLocalPosition;
        transform.rotation = weaponStateController.NowRotation;
    }

    //---->
    [SerializeField] float swaySensitivity = 15.0f;
    [SerializeField] float swaySmoothing = 0.1f;

    Vector3 newWeaponRot, tgtWeaponRot;
    Vector2 mouseDelta;
    MovementController controller;

    Vector3 smoothNewRotVel, smoothTgtRotVel;
    
    void UpdateSway()
    {
        mouseDelta *= swaySensitivity;

        tgtWeaponRot.y += mouseDelta.x * Time.deltaTime;
        tgtWeaponRot.x += mouseDelta.y * Time.deltaTime;

        tgtWeaponRot = Vector3.SmoothDamp(tgtWeaponRot, Vector3.zero, ref smoothTgtRotVel, swaySmoothing);
        newWeaponRot = Vector3.SmoothDamp(newWeaponRot, tgtWeaponRot, ref smoothNewRotVel, swaySmoothing);

        transform.localRotation = Quaternion.Euler(newWeaponRot);
    }

    //---->
    public float BobFrequency = 10f;
    public float BobSharpness = 10f;
    public float DefaultBobAmount = 0.05f;
    public float AimingBobAmount = 0.02f;

    Vector3 m_LastCharacterPosition;
    float m_WeaponBobFactor;

    float MaxSpeedOnGround = 13;
    float SprintSpeedModifier = 1.5f;

    void UpdateBob()
    {
        Vector3 playerCharacterVelocity =
                    (transform.position - m_LastCharacterPosition) / Time.deltaTime;

        float characterMovementFactor = 0f;
        if (movementController.IsGrounded) // IsGrounded
        {
            characterMovementFactor =
                Mathf.Clamp01(playerCharacterVelocity.magnitude /
                              (MaxSpeedOnGround * SprintSpeedModifier));
        }

        m_WeaponBobFactor =
            Mathf.Lerp(m_WeaponBobFactor, characterMovementFactor, BobSharpness * Time.deltaTime);

        //! ‚±‚±•Ï‚¦‚é
        bool IsAiming = false;

        float bobAmount = IsAiming ? AimingBobAmount : DefaultBobAmount;
        float frequency = BobFrequency;
        float hBobValue = Mathf.Sin(Time.time * frequency) * bobAmount * m_WeaponBobFactor;
        float vBobValue = ((Mathf.Sin(Time.time * frequency * 2f) * 0.5f) + 0.5f) * bobAmount *
                                  m_WeaponBobFactor;

        m_WeaponBobLocalPosition.x = hBobValue;
        m_WeaponBobLocalPosition.y = Mathf.Abs(vBobValue);

        m_LastCharacterPosition = transform.position;
    }

}
