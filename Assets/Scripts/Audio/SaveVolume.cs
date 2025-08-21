using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class SaveVolume : MonoBehaviour
{
    private Bus generalBus;
    private Bus musicBus;
    private Bus effectBus;

    private float a, b, c;
    void Start()
    {
        generalBus = RuntimeManager.GetBus("bus:/General Sound");
        musicBus = RuntimeManager.GetBus("bus:/General Sound/Music Group");
        effectBus = RuntimeManager.GetBus("bus:/General Sound/SFX");

        effectBus.setVolume(PlayerPrefs.GetFloat("Effect Volume", 1f));
        generalBus.setVolume(PlayerPrefs.GetFloat("General Volume", 1f));
        musicBus.setVolume(PlayerPrefs.GetFloat("Music Volume", 1f));

        
    }

    private void Update()
    {
        effectBus.getVolume(out a);

        musicBus.getVolume(out b);

        generalBus.getVolume(out c);
    }

}