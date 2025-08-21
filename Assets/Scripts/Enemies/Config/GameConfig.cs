using NUnit.Framework;
using UnityEngine;


[CreateAssetMenu(fileName = "ParameterScript", menuName = "Config/Parameters")]
public class GameConfig : ScriptableObject
{
    [Header("Player")]
    public float playerHealth;
    public float playerVelocity;
    public float playerJumpImpulse;
    public float sprintMultiplier;

    [Header("Base")]
    public int baseHealth;

    [Header("Dias")]
    public Dia[] dias;
}
