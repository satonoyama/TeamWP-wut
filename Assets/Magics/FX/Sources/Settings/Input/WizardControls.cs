//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Magics/FX/Sources/Settings/Input/WizardControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @WizardControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @WizardControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""WizardControls"",
    ""maps"": [
        {
            ""name"": ""Magic"",
            ""id"": ""c3909c7f-d43a-49f3-8fa7-907f3a6c2dd4"",
            ""actions"": [
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""3fe14fab-2ac3-48f0-9f1c-af1cc5e4933c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Burst"",
                    ""type"": ""Button"",
                    ""id"": ""8d839f6c-8e1b-4fe6-9324-f3085a118c3f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Burstfire"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FireEnd"",
                    ""type"": ""Button"",
                    ""id"": ""f1544b8a-4d5d-4e16-a303-4c91c316a311"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ADS"",
                    ""type"": ""Button"",
                    ""id"": ""046966f7-b876-4525-b2e6-09e5b38f88e7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AttackMove1"",
                    ""type"": ""Button"",
                    ""id"": ""f3cec250-1e46-41be-9b7a-ebfb863ab1d0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AttackMove2"",
                    ""type"": ""Button"",
                    ""id"": ""f4b374f0-f7f6-4815-a10b-08ffa99e2057"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""StatusMove"",
                    ""type"": ""Button"",
                    ""id"": ""2b2f40fa-a8b3-4dc2-8091-3ec046d585a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SpecialMove"",
                    ""type"": ""Button"",
                    ""id"": ""a4faf913-a083-4e4d-bceb-d696794d8db8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2ed7d73a-35e2-4165-897b-a62b9d1ab010"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4d4fe086-8e06-41e5-95f3-44b8779f0447"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Burstfire"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Burst"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc27c98d-3bb0-42e8-aa35-32ffb583b6bb"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""FireEnd"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cae0d0a5-90d7-4cd4-8ec1-b3fbdc25293b"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""ADS"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""70dbd250-95f3-4ec4-b2a3-d4ca6a9b1192"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Burstfire"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""AttackMove1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b98a5135-07c1-4e3b-84a5-7873d27846e5"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Burstfire"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""AttackMove2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4088b9c4-3761-4d27-942c-50b6aff3a526"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": ""Burstfire"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""StatusMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fafe53a6-22af-4a65-98ac-4936550c18c1"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Burstfire"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""SpecialMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": []
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": []
        }
    ]
}");
        // Magic
        m_Magic = asset.FindActionMap("Magic", throwIfNotFound: true);
        m_Magic_Fire = m_Magic.FindAction("Fire", throwIfNotFound: true);
        m_Magic_Burst = m_Magic.FindAction("Burst", throwIfNotFound: true);
        m_Magic_FireEnd = m_Magic.FindAction("FireEnd", throwIfNotFound: true);
        m_Magic_ADS = m_Magic.FindAction("ADS", throwIfNotFound: true);
        m_Magic_AttackMove1 = m_Magic.FindAction("AttackMove1", throwIfNotFound: true);
        m_Magic_AttackMove2 = m_Magic.FindAction("AttackMove2", throwIfNotFound: true);
        m_Magic_StatusMove = m_Magic.FindAction("StatusMove", throwIfNotFound: true);
        m_Magic_SpecialMove = m_Magic.FindAction("SpecialMove", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Magic
    private readonly InputActionMap m_Magic;
    private IMagicActions m_MagicActionsCallbackInterface;
    private readonly InputAction m_Magic_Fire;
    private readonly InputAction m_Magic_Burst;
    private readonly InputAction m_Magic_FireEnd;
    private readonly InputAction m_Magic_ADS;
    private readonly InputAction m_Magic_AttackMove1;
    private readonly InputAction m_Magic_AttackMove2;
    private readonly InputAction m_Magic_StatusMove;
    private readonly InputAction m_Magic_SpecialMove;
    public struct MagicActions
    {
        private @WizardControls m_Wrapper;
        public MagicActions(@WizardControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fire => m_Wrapper.m_Magic_Fire;
        public InputAction @Burst => m_Wrapper.m_Magic_Burst;
        public InputAction @FireEnd => m_Wrapper.m_Magic_FireEnd;
        public InputAction @ADS => m_Wrapper.m_Magic_ADS;
        public InputAction @AttackMove1 => m_Wrapper.m_Magic_AttackMove1;
        public InputAction @AttackMove2 => m_Wrapper.m_Magic_AttackMove2;
        public InputAction @StatusMove => m_Wrapper.m_Magic_StatusMove;
        public InputAction @SpecialMove => m_Wrapper.m_Magic_SpecialMove;
        public InputActionMap Get() { return m_Wrapper.m_Magic; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MagicActions set) { return set.Get(); }
        public void SetCallbacks(IMagicActions instance)
        {
            if (m_Wrapper.m_MagicActionsCallbackInterface != null)
            {
                @Fire.started -= m_Wrapper.m_MagicActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_MagicActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_MagicActionsCallbackInterface.OnFire;
                @Burst.started -= m_Wrapper.m_MagicActionsCallbackInterface.OnBurst;
                @Burst.performed -= m_Wrapper.m_MagicActionsCallbackInterface.OnBurst;
                @Burst.canceled -= m_Wrapper.m_MagicActionsCallbackInterface.OnBurst;
                @FireEnd.started -= m_Wrapper.m_MagicActionsCallbackInterface.OnFireEnd;
                @FireEnd.performed -= m_Wrapper.m_MagicActionsCallbackInterface.OnFireEnd;
                @FireEnd.canceled -= m_Wrapper.m_MagicActionsCallbackInterface.OnFireEnd;
                @ADS.started -= m_Wrapper.m_MagicActionsCallbackInterface.OnADS;
                @ADS.performed -= m_Wrapper.m_MagicActionsCallbackInterface.OnADS;
                @ADS.canceled -= m_Wrapper.m_MagicActionsCallbackInterface.OnADS;
                @AttackMove1.started -= m_Wrapper.m_MagicActionsCallbackInterface.OnAttackMove1;
                @AttackMove1.performed -= m_Wrapper.m_MagicActionsCallbackInterface.OnAttackMove1;
                @AttackMove1.canceled -= m_Wrapper.m_MagicActionsCallbackInterface.OnAttackMove1;
                @AttackMove2.started -= m_Wrapper.m_MagicActionsCallbackInterface.OnAttackMove2;
                @AttackMove2.performed -= m_Wrapper.m_MagicActionsCallbackInterface.OnAttackMove2;
                @AttackMove2.canceled -= m_Wrapper.m_MagicActionsCallbackInterface.OnAttackMove2;
                @StatusMove.started -= m_Wrapper.m_MagicActionsCallbackInterface.OnStatusMove;
                @StatusMove.performed -= m_Wrapper.m_MagicActionsCallbackInterface.OnStatusMove;
                @StatusMove.canceled -= m_Wrapper.m_MagicActionsCallbackInterface.OnStatusMove;
                @SpecialMove.started -= m_Wrapper.m_MagicActionsCallbackInterface.OnSpecialMove;
                @SpecialMove.performed -= m_Wrapper.m_MagicActionsCallbackInterface.OnSpecialMove;
                @SpecialMove.canceled -= m_Wrapper.m_MagicActionsCallbackInterface.OnSpecialMove;
            }
            m_Wrapper.m_MagicActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Burst.started += instance.OnBurst;
                @Burst.performed += instance.OnBurst;
                @Burst.canceled += instance.OnBurst;
                @FireEnd.started += instance.OnFireEnd;
                @FireEnd.performed += instance.OnFireEnd;
                @FireEnd.canceled += instance.OnFireEnd;
                @ADS.started += instance.OnADS;
                @ADS.performed += instance.OnADS;
                @ADS.canceled += instance.OnADS;
                @AttackMove1.started += instance.OnAttackMove1;
                @AttackMove1.performed += instance.OnAttackMove1;
                @AttackMove1.canceled += instance.OnAttackMove1;
                @AttackMove2.started += instance.OnAttackMove2;
                @AttackMove2.performed += instance.OnAttackMove2;
                @AttackMove2.canceled += instance.OnAttackMove2;
                @StatusMove.started += instance.OnStatusMove;
                @StatusMove.performed += instance.OnStatusMove;
                @StatusMove.canceled += instance.OnStatusMove;
                @SpecialMove.started += instance.OnSpecialMove;
                @SpecialMove.performed += instance.OnSpecialMove;
                @SpecialMove.canceled += instance.OnSpecialMove;
            }
        }
    }
    public MagicActions @Magic => new MagicActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IMagicActions
    {
        void OnFire(InputAction.CallbackContext context);
        void OnBurst(InputAction.CallbackContext context);
        void OnFireEnd(InputAction.CallbackContext context);
        void OnADS(InputAction.CallbackContext context);
        void OnAttackMove1(InputAction.CallbackContext context);
        void OnAttackMove2(InputAction.CallbackContext context);
        void OnStatusMove(InputAction.CallbackContext context);
        void OnSpecialMove(InputAction.CallbackContext context);
    }
}
