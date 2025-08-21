using NUnit.Framework;
using UnityEngine;


/// <summary>
/// LA LISTA DE LOS PREFABS Y LA LISTA DE LOS NUMEROS 
/// TIENEN QUE ESTAR ORDENADAS
/// EL PRIMER PREFAB ES LA CANTIDAD DE LOS PRIMEROS ENEMIGOS
/// </summary

[CreateAssetMenu(fileName = "ParameterScript", menuName = "Config/Oleada")]
public class Oleada : ScriptableObject
{
    public GameObject[] enemigos;
    public int[] cantidadEnemigos;
}
