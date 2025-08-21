using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyDisplayUpdater : MonoBehaviour
{
    [Header("Referencia a la acci�n de input")]
    public InputActionReference actionReference;

    [Header("�ndice del binding que quieres mostrar")]
    public int bindingIndex = 0;

    [Header("Texto donde se mostrar� la tecla")]
    public TMP_Text keyText;

    private string lastBindingPath = "";

    void Update()
    {
        if (actionReference == null || keyText == null)
            return;

        var binding = actionReference.action.bindings[bindingIndex];
        string currentPath = binding.effectivePath;

        if (currentPath != lastBindingPath)
        {
            // Ha cambiado el binding, actualizamos el texto
            string readable = InputControlPath.ToHumanReadableString(
                currentPath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );

            keyText.text = readable;
            lastBindingPath = currentPath;
        }
    }
}
