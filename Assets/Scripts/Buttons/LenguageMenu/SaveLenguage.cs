using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveLenguage : MonoBehaviour
{

    private TMP_Dropdown dropdown;

    /// <summary>
    /// Se guarda el idioma del juego para que este el mismo en todas las escenas
    /// </summary>
    private void Start()
    {
        /*dropdown = GetComponent<TMP_Dropdown>();

        dropdown.value = PlayerPrefs.GetInt("LocaleId",0);

        dropdown.onValueChanged.AddListener(LocalizationManager.instance.ChangeLocale);*/

        dropdown = GetComponent<TMP_Dropdown>();

        int savedValue = PlayerPrefs.GetInt("LocaleId", 0);

        Debug.Log("Valor Idioma: " + savedValue);

        if (savedValue >= 0 && savedValue < dropdown.options.Count)
        {
            dropdown.value = savedValue;
        }
        else
        {
            dropdown.value = 0; // valor por defecto
        }

        dropdown.onValueChanged.AddListener(LocalizationManager.instance.ChangeLocale);
    }
}
