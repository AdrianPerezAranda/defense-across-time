using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] Slider volumeGeneral;
    [SerializeField] Slider volumeMusic;
    [SerializeField] Slider volumeEffect;

    private Bus generalBus;
    private Bus musicBus;
    private Bus effectBus;

    void Start()
    {
        Debug.Log("Volume Controller Start");
        volumeEffect.value = (PlayerPrefs.GetFloat("Effect Volume", 1f) * 100f);
        volumeGeneral.value = (PlayerPrefs.GetFloat("General Volume", 1f) * 100f);
        volumeMusic.value = (PlayerPrefs.GetFloat("Music Volume", 1f) * 100f);
         
        volumeEffect.onValueChanged.AddListener(SetVolumeEffect);
        volumeGeneral.onValueChanged.AddListener(SetVolumeGeneral);
        volumeMusic.onValueChanged.AddListener(SetVolumeMusic);

        generalBus = RuntimeManager.GetBus("bus:/General Sound");
        musicBus = RuntimeManager.GetBus("bus:/General Sound/Music Group");
        effectBus = RuntimeManager.GetBus("bus:/General Sound/SFX");
    }

    private void SetVolumeEffect(float value)
    {
        effectBus.setVolume((value / 100f));
        PlayerPrefs.SetFloat("Effect Volume", (value / 100f));
        PlayerPrefs.Save();
    }

    private void SetVolumeGeneral(float value)
    {
        generalBus.setVolume((value / 100f));
        PlayerPrefs.SetFloat("General Volume", (value / 100f));
        PlayerPrefs.Save();
    }

    private void SetVolumeMusic(float value)
    {
        musicBus.setVolume((value / 100f));
        PlayerPrefs.SetFloat("Music Volume", (value / 100f));
        PlayerPrefs.Save();
    }
}
