using UnityEngine;
using UnityEngine.UI;


public class SpecialEffects : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI n_SpecialEffects;
    [SerializeField] Slider specialEffects;
    [SerializeField] Slider overallMusic;

    //private EventBus specialEffectsBus;

    void Start()
    {
        //specialEffectsBus = RuntimeManager.GetBus("bus:/Special_Effects");
    }

    void Update()
    {
        // Modifica el volumen del bus de los efectos especiales
        //specialEffectsBus.setValue(specialEffects.value);

        // Cambia el valor mostrado en pantalla respecto al del slider
        n_SpecialEffects.text = specialEffects.value.ToString();
    }
}
