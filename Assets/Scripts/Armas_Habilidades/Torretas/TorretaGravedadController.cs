using System.Collections;
using UnityEngine;

public class TorretaGravedadController : MonoBehaviour, ActivarTrampa
{
    public float radioAtaque = 15f;
    public float daño = 20f;
    public float tiempoEntreAtaques = 2f;

    public Light luzAtaque;
    public Renderer parteVisual;
    public Color colorEmisivo = Color.black;
    public float duracionFlash = 0.2f;

    private float temporizador;
    private Material materialBase;
    private Color colorOriginal;
    private bool puedeAtacar = false; // Controla si la torreta puede atacar

    void Start()
    {
        temporizador = 0f;

        if (parteVisual != null)
        {
            materialBase = parteVisual.material;
            if (materialBase.HasProperty("_EmissionColor"))
            {
                colorOriginal = materialBase.GetColor("_EmissionColor");
            }
        }

        if (luzAtaque != null)
        {
            luzAtaque.enabled = false;
        }
    }

    void Update()
    {
        if (!puedeAtacar) return;

        temporizador += Time.deltaTime;

        if (temporizador >= tiempoEntreAtaques)
        {
            AtacarEnemigos();
            temporizador = 0f;
        }
    }

    void AtacarEnemigos()
    {
        StartCoroutine(FlashVisual());

        Collider[] enemigos = Physics.OverlapSphere(transform.position, radioAtaque);
        foreach (Collider col in enemigos)
        {
            if (col.CompareTag("Enemigo"))
            {
                Enemy vida = col.GetComponent<Enemy>();
                if (vida != null)
                {
                    vida.ReciveDamage(daño);
                }
            }
        }
    }

    IEnumerator FlashVisual()
    {
        if (luzAtaque != null)
        {
            luzAtaque.intensity = 200f;
            luzAtaque.range = 20f;
            luzAtaque.color = colorEmisivo;
            luzAtaque.enabled = true;
        }

        if (parteVisual != null && materialBase != null)
        {
            materialBase.SetColor("_EmissionColor", colorEmisivo * 100f);
        }

        yield return new WaitForSeconds(duracionFlash);

        if (luzAtaque != null)
            luzAtaque.enabled = false;

        if (parteVisual != null && materialBase != null)
        {
            materialBase.SetColor("_EmissionColor", colorOriginal);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioAtaque);
    }

    public void Activar()
    {
        puedeAtacar = true; // Permitir que la torreta ataque
        Debug.Log("Torreta de gravedad activada");
    }
}
