using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectionRoomType
{
    Room, 
    Hallway
}

public class ConnectingRoom : MonoBehaviour
{
    public Vector2 Position { get; private set; }
    [field: SerializeField] public List<Vector2> OtherOccupiedBlockOffsets { get; private set; } = new();
    private bool _connectionPointsAdjusted = false;
    [SerializeField] private List<ConnectionPoint> ConnectionPoints = new();
    private Dictionary<ConnectionPoint, bool> _connectionPointStatus = new();
    [SerializeField] private ConnectionRoomType _roomType = ConnectionRoomType.Room;
    [ShowInInspector][ReadOnly] public ConnectionDirection _roomDirection = ConnectionDirection.Up;
    [field: SerializeField] public List<ConnectionDirection> _possibleRoomDirections { get; private set; } = new();

    private void Awake()
    {
        foreach(ConnectionPoint p in ConnectionPoints)
        {
            _connectionPointStatus.Add(p, false);
        }
    }

    public List<ConnectionPoint> GetFreeConnectionPoints()
    {
        if(!_connectionPointsAdjusted)
        {
            foreach (ConnectionPoint p in ConnectionPoints)
            {
                p.adjustedDirection = CalculateDirection(p.Direction);
            }
        }

        List<ConnectionPoint> adjustedConnectionPoints = new();
        foreach(ConnectionPoint p in ConnectionPoints)
        {
            if (!_connectionPointStatus[p])
            {
                adjustedConnectionPoints.Add(p);
            }
        }
        return adjustedConnectionPoints;
    }

    private ConnectionDirection CalculateDirection(ConnectionDirection direction) =>
        (ConnectionDirection)(
            (
                (int)direction + (int)_roomDirection
            ) % 4
        );

    public void OccupyConnectionPoint(ConnectionPoint p) 
    {
        _connectionPointStatus[p] = true;
    }

    public bool IsConnectionPointOccupied(ConnectionPoint p) => _connectionPointStatus[p];
}
