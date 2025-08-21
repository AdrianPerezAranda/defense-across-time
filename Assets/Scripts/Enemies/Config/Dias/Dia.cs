using UnityEngine;

[CreateAssetMenu(fileName = "ParameterScript", menuName = "Config/Dias")]
public class Dia : ScriptableObject
{
    public enum Epoca
    {
        Prehistoria,
        Edad_Media,
        Futuro
    }
    public Epoca epoca;

    [Header("Oleadas")]
    public Oleada[] oleadas;
}