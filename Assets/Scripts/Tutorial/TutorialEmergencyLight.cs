using System.Collections;
using UnityEngine;

public class TutorialEmergencyLight : MonoBehaviour
{
    [SerializeField] GameObject luz;
    [SerializeField] MeshRenderer bombillaRenderer;
    [SerializeField] Material encendidaMat;
    [SerializeField] Material apagadaMat;
    public float intervalo = 0.5f;

    private void Start()
    {
        StartCoroutine(Parpadeo());
    }

    IEnumerator Parpadeo()
    {
        while (true)
        {
            luz.SetActive(true);
            bombillaRenderer.material = encendidaMat;
            yield return new WaitForSeconds(intervalo);

            luz.SetActive(false);
            bombillaRenderer.material = apagadaMat;
            yield return new WaitForSeconds(intervalo);
        }
    }
}
