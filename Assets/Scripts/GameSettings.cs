using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
public class GameSettings : ScriptableObject
{
    public float Sensitivity = 1f;
    public int Volume = 50;
    public bool HoldSprint = true;
    public bool HoldCrouch = false;
}
