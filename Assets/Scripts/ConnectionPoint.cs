using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public enum ConnectionDirection
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}

public class ConnectionPoint : SerializedMonoBehaviour
{
    /// <summary>
    /// Coordinate of the block the connection point belongs to
    /// </summary>
    [field: SerializeField] public Vector2 ParentRoomPartPosition;
    /// <summary>
    /// Offset from the coordinate of the room origo, ONLY USABLE BEFORE INSTANTIATE
    /// </summary>
    [ShowInInspector][ReadOnly] public Vector3 PositionOffset => new Vector3(-transform.position.x, 0, -transform.position.z);
    /// <summary>
    /// Position of the ConnectionPoint in WorldCoordinates
    /// </summary>
    [ShowInInspector][ReadOnly] public Vector3 Position => new Vector3(transform.position.x, 0, transform.position.z);
    [field: SerializeField] public ConnectionDirection Direction { get; private set; }
    public ConnectionDirection adjustedDirection;
    [SerializeField] private bool _showGizmo = true;
    [SerializeField] private GameObject _gizmo = null;

    void Start()
    {
        if (_gizmo == null)
        {
            Debug.LogError($"{gameObject.name} : Gizmo reference is missing");
        }
        else
        {
            _gizmo.SetActive(_showGizmo);
        }
    }
}
