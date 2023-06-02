//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.0
//     from Assets/Controls/Player_Controls.inputactions
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

public partial class @Player_Controls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Player_Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player_Controls"",
    ""maps"": [
        {
            ""name"": ""Player1"",
            ""id"": ""edd91490-1412-43ab-83bf-548edd422293"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""ebe93f6b-4483-4c10-8604-d1350fa2459b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""f81c6585-aec5-4edb-8f2c-db53198be6b6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""4d09afe6-37a7-4055-957a-d213faf024c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftSpell"",
                    ""type"": ""Button"",
                    ""id"": ""4b55b56a-b093-4026-855e-0b04cb4818fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""RightSpell"",
                    ""type"": ""Button"",
                    ""id"": ""b6bc3d88-de88-4761-94b9-df54b51a46ef"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CombineSpell"",
                    ""type"": ""Button"",
                    ""id"": ""0a87ccd5-14f8-49e5-9ae4-78285aefcf2d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SwapLeftSpell"",
                    ""type"": ""Button"",
                    ""id"": ""d1b1c347-45f5-4e21-b0a6-87733fc9b551"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwapRightSpell"",
                    ""type"": ""Button"",
                    ""id"": ""893af57a-db8b-4f2c-8b1c-e70198fd9c8d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SpellWheel"",
                    ""type"": ""Button"",
                    ""id"": ""6649e949-0d5b-4f0e-abc6-9dceb5b510e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""9c38e0a0-ac9e-401f-bc9f-263078485f57"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftTap"",
                    ""type"": ""Button"",
                    ""id"": ""339a1c12-f60e-4d9c-9c54-ab503e9a941d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightTap"",
                    ""type"": ""Button"",
                    ""id"": ""328817e0-eb65-463a-8350-f676d8dfa78d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwitchCamera"",
                    ""type"": ""Button"",
                    ""id"": ""5a9ee4d5-5cea-4b91-a9db-412b0d85c504"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""7ae4566f-07e4-4136-b5a2-937e5b954cd0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""75192a0a-b1e6-4e82-890e-09e71249fc9c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5132b65c-e37b-4fbd-b6d9-88b3f2199e13"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6febf032-a30c-4281-ad2b-12ca2fac437f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c48423cb-f659-482e-977f-65502b172218"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1aec700f-2460-4a13-93a2-11495c061e2f"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aac5105b-24ca-40a4-bf2d-d3df38119088"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""RightAndLeftClick"",
                    ""id"": ""80619524-08cc-4b2a-91a9-5093aa324915"",
                    ""path"": ""OneModifier"",
                    ""interactions"": ""Hold(duration=0.2,pressPoint=0.3)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CombineSpell"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""48110e91-358e-45fd-8f10-74ed0acb2f84"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CombineSpell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""d3c440cc-8a7b-41cf-a3d9-f9045fd6d078"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CombineSpell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0fc5e297-dc6c-4b0b-8ad4-803c327835f5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold(duration=0.3,pressPoint=0.4)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftSpell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69fce274-de88-4e30-b210-5429fd48fdeb"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Hold(duration=0.3,pressPoint=0.4)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightSpell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""266d7985-988a-4844-82cf-64d553ca3e3c"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwapLeftSpell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5b993a1-1ce4-4000-82b5-62bf668522f0"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwapRightSpell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1c3086ae-3be1-4921-b7dd-0c39825441f1"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpellWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""480bfc54-0bb9-4551-b654-8b95808f6af6"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""089771b2-6c50-414c-a29b-bc287d8d49f7"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ceb2966-c96f-464b-87ed-663ea4e9cdc6"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftTap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""668fb14c-6bde-4c6a-86f3-1b3e8be6b0fa"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightTap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35c3a955-6722-435a-9f0c-5a3c19293c32"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Player"",
            ""bindingGroup"": ""Player"",
            ""devices"": []
        }
    ]
}");
        // Player1
        m_Player1 = asset.FindActionMap("Player1", throwIfNotFound: true);
        m_Player1_Move = m_Player1.FindAction("Move", throwIfNotFound: true);
        m_Player1_Look = m_Player1.FindAction("Look", throwIfNotFound: true);
        m_Player1_Jump = m_Player1.FindAction("Jump", throwIfNotFound: true);
        m_Player1_LeftSpell = m_Player1.FindAction("LeftSpell", throwIfNotFound: true);
        m_Player1_RightSpell = m_Player1.FindAction("RightSpell", throwIfNotFound: true);
        m_Player1_CombineSpell = m_Player1.FindAction("CombineSpell", throwIfNotFound: true);
        m_Player1_SwapLeftSpell = m_Player1.FindAction("SwapLeftSpell", throwIfNotFound: true);
        m_Player1_SwapRightSpell = m_Player1.FindAction("SwapRightSpell", throwIfNotFound: true);
        m_Player1_SpellWheel = m_Player1.FindAction("SpellWheel", throwIfNotFound: true);
        m_Player1_Pause = m_Player1.FindAction("Pause", throwIfNotFound: true);
        m_Player1_LeftTap = m_Player1.FindAction("LeftTap", throwIfNotFound: true);
        m_Player1_RightTap = m_Player1.FindAction("RightTap", throwIfNotFound: true);
        m_Player1_SwitchCamera = m_Player1.FindAction("SwitchCamera", throwIfNotFound: true);
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

    // Player1
    private readonly InputActionMap m_Player1;
    private List<IPlayer1Actions> m_Player1ActionsCallbackInterfaces = new List<IPlayer1Actions>();
    private readonly InputAction m_Player1_Move;
    private readonly InputAction m_Player1_Look;
    private readonly InputAction m_Player1_Jump;
    private readonly InputAction m_Player1_LeftSpell;
    private readonly InputAction m_Player1_RightSpell;
    private readonly InputAction m_Player1_CombineSpell;
    private readonly InputAction m_Player1_SwapLeftSpell;
    private readonly InputAction m_Player1_SwapRightSpell;
    private readonly InputAction m_Player1_SpellWheel;
    private readonly InputAction m_Player1_Pause;
    private readonly InputAction m_Player1_LeftTap;
    private readonly InputAction m_Player1_RightTap;
    private readonly InputAction m_Player1_SwitchCamera;
    public struct Player1Actions
    {
        private @Player_Controls m_Wrapper;
        public Player1Actions(@Player_Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player1_Move;
        public InputAction @Look => m_Wrapper.m_Player1_Look;
        public InputAction @Jump => m_Wrapper.m_Player1_Jump;
        public InputAction @LeftSpell => m_Wrapper.m_Player1_LeftSpell;
        public InputAction @RightSpell => m_Wrapper.m_Player1_RightSpell;
        public InputAction @CombineSpell => m_Wrapper.m_Player1_CombineSpell;
        public InputAction @SwapLeftSpell => m_Wrapper.m_Player1_SwapLeftSpell;
        public InputAction @SwapRightSpell => m_Wrapper.m_Player1_SwapRightSpell;
        public InputAction @SpellWheel => m_Wrapper.m_Player1_SpellWheel;
        public InputAction @Pause => m_Wrapper.m_Player1_Pause;
        public InputAction @LeftTap => m_Wrapper.m_Player1_LeftTap;
        public InputAction @RightTap => m_Wrapper.m_Player1_RightTap;
        public InputAction @SwitchCamera => m_Wrapper.m_Player1_SwitchCamera;
        public InputActionMap Get() { return m_Wrapper.m_Player1; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Player1Actions set) { return set.Get(); }
        public void AddCallbacks(IPlayer1Actions instance)
        {
            if (instance == null || m_Wrapper.m_Player1ActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_Player1ActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @LeftSpell.started += instance.OnLeftSpell;
            @LeftSpell.performed += instance.OnLeftSpell;
            @LeftSpell.canceled += instance.OnLeftSpell;
            @RightSpell.started += instance.OnRightSpell;
            @RightSpell.performed += instance.OnRightSpell;
            @RightSpell.canceled += instance.OnRightSpell;
            @CombineSpell.started += instance.OnCombineSpell;
            @CombineSpell.performed += instance.OnCombineSpell;
            @CombineSpell.canceled += instance.OnCombineSpell;
            @SwapLeftSpell.started += instance.OnSwapLeftSpell;
            @SwapLeftSpell.performed += instance.OnSwapLeftSpell;
            @SwapLeftSpell.canceled += instance.OnSwapLeftSpell;
            @SwapRightSpell.started += instance.OnSwapRightSpell;
            @SwapRightSpell.performed += instance.OnSwapRightSpell;
            @SwapRightSpell.canceled += instance.OnSwapRightSpell;
            @SpellWheel.started += instance.OnSpellWheel;
            @SpellWheel.performed += instance.OnSpellWheel;
            @SpellWheel.canceled += instance.OnSpellWheel;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
            @LeftTap.started += instance.OnLeftTap;
            @LeftTap.performed += instance.OnLeftTap;
            @LeftTap.canceled += instance.OnLeftTap;
            @RightTap.started += instance.OnRightTap;
            @RightTap.performed += instance.OnRightTap;
            @RightTap.canceled += instance.OnRightTap;
            @SwitchCamera.started += instance.OnSwitchCamera;
            @SwitchCamera.performed += instance.OnSwitchCamera;
            @SwitchCamera.canceled += instance.OnSwitchCamera;
        }

        private void UnregisterCallbacks(IPlayer1Actions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @LeftSpell.started -= instance.OnLeftSpell;
            @LeftSpell.performed -= instance.OnLeftSpell;
            @LeftSpell.canceled -= instance.OnLeftSpell;
            @RightSpell.started -= instance.OnRightSpell;
            @RightSpell.performed -= instance.OnRightSpell;
            @RightSpell.canceled -= instance.OnRightSpell;
            @CombineSpell.started -= instance.OnCombineSpell;
            @CombineSpell.performed -= instance.OnCombineSpell;
            @CombineSpell.canceled -= instance.OnCombineSpell;
            @SwapLeftSpell.started -= instance.OnSwapLeftSpell;
            @SwapLeftSpell.performed -= instance.OnSwapLeftSpell;
            @SwapLeftSpell.canceled -= instance.OnSwapLeftSpell;
            @SwapRightSpell.started -= instance.OnSwapRightSpell;
            @SwapRightSpell.performed -= instance.OnSwapRightSpell;
            @SwapRightSpell.canceled -= instance.OnSwapRightSpell;
            @SpellWheel.started -= instance.OnSpellWheel;
            @SpellWheel.performed -= instance.OnSpellWheel;
            @SpellWheel.canceled -= instance.OnSpellWheel;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
            @LeftTap.started -= instance.OnLeftTap;
            @LeftTap.performed -= instance.OnLeftTap;
            @LeftTap.canceled -= instance.OnLeftTap;
            @RightTap.started -= instance.OnRightTap;
            @RightTap.performed -= instance.OnRightTap;
            @RightTap.canceled -= instance.OnRightTap;
            @SwitchCamera.started -= instance.OnSwitchCamera;
            @SwitchCamera.performed -= instance.OnSwitchCamera;
            @SwitchCamera.canceled -= instance.OnSwitchCamera;
        }

        public void RemoveCallbacks(IPlayer1Actions instance)
        {
            if (m_Wrapper.m_Player1ActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayer1Actions instance)
        {
            foreach (var item in m_Wrapper.m_Player1ActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_Player1ActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public Player1Actions @Player1 => new Player1Actions(this);
    private int m_PlayerSchemeIndex = -1;
    public InputControlScheme PlayerScheme
    {
        get
        {
            if (m_PlayerSchemeIndex == -1) m_PlayerSchemeIndex = asset.FindControlSchemeIndex("Player");
            return asset.controlSchemes[m_PlayerSchemeIndex];
        }
    }
    public interface IPlayer1Actions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnLeftSpell(InputAction.CallbackContext context);
        void OnRightSpell(InputAction.CallbackContext context);
        void OnCombineSpell(InputAction.CallbackContext context);
        void OnSwapLeftSpell(InputAction.CallbackContext context);
        void OnSwapRightSpell(InputAction.CallbackContext context);
        void OnSpellWheel(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnLeftTap(InputAction.CallbackContext context);
        void OnRightTap(InputAction.CallbackContext context);
        void OnSwitchCamera(InputAction.CallbackContext context);
    }
}
