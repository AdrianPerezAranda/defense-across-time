using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour
{
    #region SINGLETONE
    public static LocalizationManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    bool changingLocale = false;

    private void Start()
    {
        int storedId = PlayerPrefs.GetInt("LocaleId", 0);
        ChangeLocale(storedId);
    }

    public void ChangeLocale(int localeId)
    {
        if (changingLocale) return;

        StartCoroutine(SetLocale(localeId));
    }

    IEnumerator SetLocale(int localeId)
    {
        changingLocale = true;
        yield return LocalizationSettings.InitializationOperation;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
        PlayerPrefs.SetInt("LocaleId", localeId);
        changingLocale = false;
    }
}
