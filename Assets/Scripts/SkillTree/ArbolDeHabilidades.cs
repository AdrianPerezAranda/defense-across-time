using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ArbolDeHabilidades : MonoBehaviour
{
    public enum TipoHabilidad
    {
        CuerpoACuerpo,
        ADistancia,
        Trampa
    }
    [System.Serializable]
    public class Habilidad
    {
        [Header("Clase habilidad")]
        public int id;
        public string nombreHabilidad;
        public int costoPuntosHabilidad;
        public Button boton;
        public List<int> requisitos = new List<int>();
        public bool estaDesbloqueada;
        public GameObject iconoInventario;

        [Header("Trampa asociada")]
        public GameObject prefabTrampaAsociada;
        public float precioEnScena;

        [Header("Tipo de habilidad")]
        public TipoHabilidad tipoHabilidad;

    }

    [Header("Atributos principales")]
    public List<Habilidad> habilidades;
    public int puntosJugador = 10000;
    public TextMeshProUGUI textoPuntos;
    public Inventario refInventario;

    [Header("Panle de informacion")]
    public GameObject panelInfo;
    public TextMeshProUGUI textoNombreHabilidad;
    public TextMeshProUGUI costoHabilidad;
    public TextMeshProUGUI textoEstadoHabilidad;

    private GameObject iconoActual;

    private void Update()
    {
        puntosJugador = GameController.Instance.playerInstance.prestigePoints;
    }


    void Start()
    {
        panelInfo.SetActive(false);
        ActualizarUI();

        foreach (var habilidad in habilidades)
        {
            habilidad.boton.onClick.AddListener(() => IntentarDesbloquear(habilidad));
            ConfigurarEventosUI(habilidad);
        }
    }

    public void IntentarDesbloquear(Habilidad habilidad)
    {
        if (habilidad.estaDesbloqueada)
            return;

        if (!CumpleRequisitos(habilidad))
        {
            Debug.Log($"No cumples los requisitos para desbloquear {habilidad.nombreHabilidad}.");
            return;
        }

        if (puntosJugador >= habilidad.costoPuntosHabilidad)
        {
            puntosJugador -= habilidad.costoPuntosHabilidad;
            habilidad.estaDesbloqueada = true;

            if (habilidad.iconoInventario != null)
            {
                GameObject habilidadUI = refInventario.AgregarHabilidadAlInventario(habilidad.iconoInventario, habilidad.tipoHabilidad);

                // Asiganr el prefab al componnete habilidadUI
                var habilidadUIScript = habilidadUI.GetComponent<HabilidadUI>();
                if (habilidadUIScript != null && habilidad.tipoHabilidad == TipoHabilidad.Trampa)
                {
                    habilidadUIScript.prefabTrampa = habilidad.prefabTrampaAsociada;
                }
            }


            if (habilidad.tipoHabilidad == TipoHabilidad.Trampa && habilidad.prefabTrampaAsociada != null)
            {
                refInventario.AgregarPrefabTrampa(habilidad.prefabTrampaAsociada);
                Debug.Log($"Prefab de trampa {habilidad.prefabTrampaAsociada.name} agregado al inventario.");
            }
        }
        else
        {
            Debug.Log("No tienes suficiente dinero.");
        }

        ActualizarUI();
    }

    public bool CumpleRequisitos(Habilidad habilidad)
    {
        foreach (var requisitoId in habilidad.requisitos)
        {
            var habilidadRequisito = habilidades.Find(h => h.id == requisitoId);
            if (habilidadRequisito == null || !habilidadRequisito.estaDesbloqueada)
                return false;
        }
        return true;
    }

    public void ActualizarUI()
    {
        puntosJugador = GameController.Instance.playerInstance.prestigePoints;
        textoPuntos.text = $"{puntosJugador}";
        foreach (var habilidad in habilidades)
        {
            bool puedeDesbloquear = !habilidad.estaDesbloqueada &&
            CumpleRequisitos(habilidad) &&
            puntosJugador >= habilidad.costoPuntosHabilidad;
            habilidad.boton.interactable = puedeDesbloquear;
        }
    }

    private void ConfigurarEventosUI(Habilidad habilidad)
    {
        EventTrigger trigger = habilidad.boton.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((eventData) => MostrarInfoHabilidad(habilidad));

        EventTrigger.Entry entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((eventData) => OcultarInfoHabilidad());

        trigger.triggers.Add(entryEnter);
        trigger.triggers.Add(entryExit);
    }

    public void MostrarInfoHabilidad(Habilidad habilidad)
    {
        textoNombreHabilidad.text = habilidad.nombreHabilidad;
        costoHabilidad.text = $"Costo: {habilidad.costoPuntosHabilidad}";
        textoEstadoHabilidad.text = $"Estado: {(habilidad.estaDesbloqueada ? "Desbloqueada" : "Bloqueada")}";

        // Elimina el icono anterior si existe
        if (iconoActual != null)
        {
            Destroy(iconoActual);
        }

        // Instancia el nuevo icono si existe
        if (habilidad.iconoInventario != null)
        {
            iconoActual = Instantiate(habilidad.iconoInventario, panelInfo.transform);
            iconoActual.SetActive(true);

            RectTransform rectTransform = iconoActual.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(0, 20f);
                rectTransform.localScale = Vector3.one * 4.0f;
            }
        }

        panelInfo.SetActive(true);
    }

    public void OcultarInfoHabilidad()
    {
        panelInfo.SetActive(false);

        if (iconoActual != null)
        {
            Destroy(iconoActual);
            iconoActual = null;
        }

        costoHabilidad.text = "";
    }

    public void GuardarProgresoArbolHabilidades()
    {
        List<int> idsHabilidadesDesbloqueadas = new List<int>();

        foreach (var habilidad in habilidades)
        {
            if (habilidad.estaDesbloqueada)
            {
                idsHabilidadesDesbloqueadas.Add(habilidad.id);
            }

        }

        PlayerPrefs.SetString("HabilidadesDesbloqueadas", string.Join(",", idsHabilidadesDesbloqueadas));
        PlayerPrefs.SetInt("PuntosJugador", puntosJugador);
        PlayerPrefs.Save();
    }

    public void CargarProgresoArbolHabilidades()
    {
        string habilidadesDesbloqueadasString = PlayerPrefs.GetString("HabilidadesDesbloqueadas", "");

        if (!string.IsNullOrEmpty(habilidadesDesbloqueadasString))
        {
            var ids = habilidadesDesbloqueadasString.Split(',');
            foreach (var habilidad in habilidades)
            {
                habilidad.estaDesbloqueada = System.Array.Exists(ids, id => id == habilidad.id.ToString());
            }
        }

        puntosJugador = PlayerPrefs.GetInt("PuntosJugador", 10000);
        ActualizarUI();
    }


}