using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public static InputReader Instance { get; private set; }

    [SerializeField] private GameSettings _settings = null;

    private InputAction Move;
    private InputAction Look;
    private InputAction Attack;
    private InputAction Interact;
    private InputAction Crouch;
    private InputAction Jump;
    private InputAction Item1;
    private InputAction Item2;
    private InputAction Item3;
    private InputAction Item4;
    private InputAction Sprint;

    private InputAction Escape;
    private InputAction MouseDelta;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError($"{gameObject.name} : Multiple instances of InputReader detected. Destroying duplicate.");
        }
    }

    void Start()
    {
        if(_settings == null)
        {
            Debug.LogError($"{gameObject.name} : GameSettings reference is missing");
        }

        Move = InputSystem.actions.FindAction("Player/Move");
        if(Move == null)
        {
            Debug.LogError($"{gameObject.name} : Move action is missing");
        }
        Look = InputSystem.actions.FindAction("Player/Look");
        if (Look == null)
        {
            Debug.LogError($"{gameObject.name} : Look action is missing");
        }
        Attack = InputSystem.actions.FindAction("Player/Attack");
        if (Attack == null)
        {
            Debug.LogError($"{gameObject.name} : Attack action is missing");
        }
        Interact = InputSystem.actions.FindAction("Player/Interact");
        if (Interact == null)
        {
            Debug.LogError($"{gameObject.name} : Interact action is missing");
        }
        Crouch = InputSystem.actions.FindAction("Player/Crouch");
        if (Crouch == null)
        {
            Debug.LogError($"{gameObject.name} : Crouch action is missing");
        }
        Jump = InputSystem.actions.FindAction("Player/Jump");
        if (Jump == null)
        {
            Debug.LogError($"{gameObject.name} : Jump action is missing");
        }
        Item1 = InputSystem.actions.FindAction("Player/Item1");
        if (Item1 == null)
        {
            Debug.LogError($"{gameObject.name} : Item1 action is missing");
        }
        Item2 = InputSystem.actions.FindAction("Player/Item2");
        if (Item2 == null)
        {
            Debug.LogError($"{gameObject.name} : Item2 action is missing");
        }
        Item3 = InputSystem.actions.FindAction("Player/Item3");
        if (Item3 == null)
        {
            Debug.LogError($"{gameObject.name} : Item3 action is missing");
        }
        Item4 = InputSystem.actions.FindAction("Player/Item4");
        if (Item4 == null)
        {
            Debug.LogError($"{gameObject.name} : Item4 action is missing");
        }
        Sprint = InputSystem.actions.FindAction("Player/Sprint");
        if (Sprint == null)
        {
            Debug.LogError($"{gameObject.name} : Sprint action is missing");
        }
        MouseDelta = InputSystem.actions.FindAction("Player/MouseDelta");
        if (MouseDelta == null)
        {
            Debug.LogError($"{gameObject.name} : MouseDelta action is missing");
        }

        Escape = InputSystem.actions.FindAction("UI/Escape");
        if (Escape == null)
        {
            Debug.LogError($"{gameObject.name} : Escape action is missing");
        }
    }
        
    public Vector2 ReadMove() => Move.ReadValue<Vector2>();
    public Vector2 ReadLook() => Look.ReadValue<Vector2>();
    public bool ReadAttack() => Attack.WasPressedThisFrame();
    public bool ReadInteract() => Interact.WasPressedThisFrame();
    public bool ReadCrouch() => _settings.HoldCrouch ? Crouch.IsPressed() : Crouch.WasPressedThisFrame();
    public bool ReadJump() => Jump.WasPressedThisFrame();
    public bool ReadItem1() => Item1.WasPressedThisFrame();
    public bool ReadItem2() => Item2.WasPressedThisFrame();
    public bool ReadItem3() => Item3.WasPressedThisFrame();
    public bool ReadItem4() => Item4.WasPressedThisFrame();
    public bool ReadSprint() => _settings.HoldSprint ? Sprint.IsPressed() : Sprint.WasPressedThisFrame();
    public Vector2 ReadMouseDelta() => MouseDelta.ReadValue<Vector2>();

    public bool ReadEscape() => Escape.WasPressedThisFrame();
}
