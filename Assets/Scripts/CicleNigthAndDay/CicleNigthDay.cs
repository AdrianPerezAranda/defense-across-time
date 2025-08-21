using UnityEngine;

public class CycleNightDay : MonoBehaviour
{

    [SerializeField] private bool esNoche;  // Variable que controla si es de noche o de dia

    void Update()
    {
        float rotacion = esNoche ? 200f : 45f; // Rota la camara dependiendo si es de noche o de dia
        transform.rotation = Quaternion.Euler(rotacion , 0, 0); //Controla la rotacion de la camara
    }
}
