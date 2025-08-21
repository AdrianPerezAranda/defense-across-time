using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OverallVolume : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI n_OverallVolume;
    [SerializeField] Slider overallVolume;

    //private EventBus overallVolumeBus;

    void Start()
    {
        //overallVolumeBus = RuntimeManager.GetBus("bus:/Overall_Volume");
    }

 
    void Update()
    {
        // Modifica el volumen del bus del volumen general
        //overallVolumeBus.setValue(overallVolume.value);

        // Cambia el valor mostrado en pantalla respecto al del slider
        n_OverallVolume.text = overallVolume.value.ToString();
    }
}
