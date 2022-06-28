//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Scenes/scene_Title/GUIContoller.inputactions
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

public partial class @GUIContoller : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GUIContoller()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GUIContoller"",
    ""maps"": [
        {
            ""name"": ""Title"",
            ""id"": ""f8472b0f-50e5-4a6a-bd7d-ff963f70037c"",
            ""actions"": [
                {
                    ""name"": ""Enter"",
                    ""type"": ""Button"",
                    ""id"": ""7fbe883a-8d12-436c-94f8-52d98155e0d2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4ab0ae0c-3134-476c-ae86-6f5b8ca3420d"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Title
        m_Title = asset.FindActionMap("Title", throwIfNotFound: true);
        m_Title_Enter = m_Title.FindAction("Enter", throwIfNotFound: true);
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

    // Title
    private readonly InputActionMap m_Title;
    private ITitleActions m_TitleActionsCallbackInterface;
    private readonly InputAction m_Title_Enter;
    public struct TitleActions
    {
        private @GUIContoller m_Wrapper;
        public TitleActions(@GUIContoller wrapper) { m_Wrapper = wrapper; }
        public InputAction @Enter => m_Wrapper.m_Title_Enter;
        public InputActionMap Get() { return m_Wrapper.m_Title; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TitleActions set) { return set.Get(); }
        public void SetCallbacks(ITitleActions instance)
        {
            if (m_Wrapper.m_TitleActionsCallbackInterface != null)
            {
                @Enter.started -= m_Wrapper.m_TitleActionsCallbackInterface.OnEnter;
                @Enter.performed -= m_Wrapper.m_TitleActionsCallbackInterface.OnEnter;
                @Enter.canceled -= m_Wrapper.m_TitleActionsCallbackInterface.OnEnter;
            }
            m_Wrapper.m_TitleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Enter.started += instance.OnEnter;
                @Enter.performed += instance.OnEnter;
                @Enter.canceled += instance.OnEnter;
            }
        }
    }
    public TitleActions @Title => new TitleActions(this);
    public interface ITitleActions
    {
        void OnEnter(InputAction.CallbackContext context);
    }
}
