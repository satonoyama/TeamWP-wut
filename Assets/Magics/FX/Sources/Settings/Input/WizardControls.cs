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
    public struct MagicActions
    {
        private @WizardControls m_Wrapper;
        public MagicActions(@WizardControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fire => m_Wrapper.m_Magic_Fire;
        public InputAction @Burst => m_Wrapper.m_Magic_Burst;
        public InputAction @FireEnd => m_Wrapper.m_Magic_FireEnd;
        public InputAction @ADS => m_Wrapper.m_Magic_ADS;
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
    }
}