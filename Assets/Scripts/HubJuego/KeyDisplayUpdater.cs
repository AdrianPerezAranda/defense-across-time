using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyDisplayUpdater : MonoBehaviour
{
    [Header("Referencia a la acción de input")]
    public InputActionReference actionReference;

    [Header("Índice del binding que quieres mostrar")]
    public int bindingIndex = 0;

    [Header("Texto donde se mostrará la tecla")]
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
