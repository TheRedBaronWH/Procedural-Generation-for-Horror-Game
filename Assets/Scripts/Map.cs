using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "Scriptable Objects/Map")]
public class Map : ScriptableObject
{
    public static void GridToWorldCoordinate(Vector2 gridCoord, out Vector3 worldCoord)
    {
        worldCoord = new Vector3(gridCoord.x * -12, 0, gridCoord.y * -12);
    }

    public static void WorldToGridCoordinate(Vector3 worldCoord, out Vector2 gridCoord)
    {
        gridCoord = new Vector2(worldCoord.x / -12, worldCoord.z / -12);
    }

    public Dictionary<Vector2, ConnectingRoom> Rooms { get; private set; } = new();
    public Dictionary<Vector2, bool> OccupiedTiles { get; private set; } = new();

    public void AddRoom(Vector2 position, ConnectingRoom room)
    {
        Rooms.Add(position, room);
        OccupiedTiles[position] = true;
        foreach(Vector2 p in room.OtherOccupiedBlockOffsets)
        {
            OccupiedTiles[p] = true;
        }
    }
}
