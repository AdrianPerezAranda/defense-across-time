using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI n_Music;
    [SerializeField] Slider music;
    [SerializeField] Slider overallMusic;

    //private EventBus musicBus;

    void Start()
    {
        //musicBus = RuntimeManager.GetBus("bus:/Music");
    }


    void Update()
    {
        // Modifica el volumen del bus de la musica
        //musicBus.setValue(music.value);

        // Cambia el valor mostrado en pantalla respecto al del slider
        n_Music.text = music.value.ToString();
    }
}
