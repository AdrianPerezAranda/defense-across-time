using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;

public class RebindButton : MonoBehaviour
{
    // Acción de entrada que se quiere remapear (ej: "Move", "Jump", etc.)
    public InputActionReference actionReference;

    // Índice del binding dentro de la acción. En composites, cada dirección es un índice distinto.
    public int bindingIndex = 0;

    // Referencia al componente de texto que muestra la tecla actual (usualmente hijo del botón)
    public TMP_Text buttonText;

    // Prefab del popup
    public GameObject warningPopupPrefab;

    private void Start()
    {
        UpdateButtonText();
    }

    ///<summary>
    /// Inicia el proceso de remapeo cuando el jugador hace clic en el botón.
    ///</summary>
    public void StartRebind()
    {
        // Guardamos el binding actual antes de hacer el rebind
        string originalBindingPath = actionReference.action.bindings[bindingIndex].effectivePath;

        // Mostrar mensaje de espera
        buttonText.text = "Presiona una tecla...";

        // Deshabilitamos la acción antes de hacer el rebind para evitar conflictos
        actionReference.action.Disable();

        actionReference.action.PerformInteractiveRebinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f) // Espera un corto periodo antes de confirmar la tecla
            .OnComplete(operation =>
            {
                string newBindingPath = operation.selectedControl.path;

                // Aplicamos temporalmente el nuevo binding
                actionReference.action.ApplyBindingOverride(bindingIndex, newBindingPath);

                // Volvemos a habilitar la acción para refrescar los effectivePaths
                actionReference.action.Enable();

                // Ahora hacemos la verificación de conflictos
                if (IsBindingAlreadyUsed(newBindingPath, actionReference, bindingIndex))
                {
                    Debug.LogWarning("Esa tecla ya está asignada a otra acción.");
                    buttonText.text = "Tecla en uso";

                    // Revertimos el binding al original si ya está en uso
                    actionReference.action.ApplyBindingOverride(bindingIndex, originalBindingPath);
                    ShowWarningPopup("La tecla ya está en uso.");

                    operation.Dispose();
                    UpdateButtonText();
                    return;
                }

                // Liberamos los recursos de la operación de rebind
                operation.Dispose();

                // Actualizamos el texto del botón con la nueva tecla asignada
                UpdateButtonText();

                // Guardamos el nuevo binding en las preferencias del jugador
                SaveBinding();

                // Rehabilitamos la acción después del rebind
                actionReference.action.Enable();
            })
            .Start(); // Inicia el proceso de rebind interactivo
    }

    ///<summary>
    /// Comprueba si otro binding (distinto del actual) ya está usando la misma tecla/control.
    ///</summary>
    ///
    ///<param name="newPath"> Ruta del nuevo binding </param>
    ///<param name="currentReference"> Referencia de la acción actual </param>
    ///<param name="currentBindingIndex"> Índice del binding actual </param>
    ///
    ///<return> Devuelve true si la tecla ya está en uso, de lo contrario false </return>
    private bool IsBindingAlreadyUsed(string newPath, InputActionReference currentReference, int currentBindingIndex)
    {
        string newReadable = NormalizeBindingName(
        InputControlPath.ToHumanReadableString(newPath, InputControlPath.HumanReadableStringOptions.OmitDevice)
    );

        InputActionAsset asset = currentReference.asset;

        foreach (var map in asset.actionMaps)
        {
            foreach (var action in map.actions)
            {
                // ? Filtrar mapas y acciones según lo que dijiste
                bool isAllowed =
                    map.name == "Player" || // permitir todas las de Player
                    (map.name == "UI" && (action.name == "Inventario" || action.name == "ArbolHabilidades" || action.name == "RightClick"));

                if (!isAllowed)
                    continue;

                for (int i = 0; i < action.bindings.Count; i++)
                {
                    var binding = action.bindings[i];

                    if (action == currentReference.action && i == currentBindingIndex)
                        continue;

                    if (binding.isComposite || (binding.isPartOfComposite && string.IsNullOrEmpty(binding.effectivePath)))
                        continue;

                    string existingReadable = NormalizeBindingName(
                        InputControlPath.ToHumanReadableString(binding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice)
                    );

                    if (existingReadable.Equals(newReadable, StringComparison.OrdinalIgnoreCase))
                    {
                        Debug.LogWarning($"[CONFLICTO] La tecla '{newReadable}' ya está en uso por: {action.name} ({map.name})!");
                        return true;
                    }
                }
            }
        }

        return false;
    }

    ///<summary>
    /// Normaliza el nombre de una tecla eliminando espacios y convirtiendo a minúsculas.
    ///</summary>
    ///
    ///<param name="bindingName"> Nombre del binding original </param>
    ///
    ///<return> Nombre del binding normalizado </return>
    private string NormalizeBindingName(string bindingName)
    {
        return new string(bindingName.ToLower().Replace(" ", "").ToCharArray());
    }

    ///<summary>
    /// Muestra un popup de advertencia con el mensaje indicado.
    ///</summary>
    ///
    ///<param name="message"> Mensaje de advertencia a mostrar </param>
    void ShowWarningPopup(string message)
    {
        if (warningPopupPrefab == null)
        {
            Debug.LogError("No se ha asignado el warningPopupPrefab en el Inspector.");
            return;
        }
        Instantiate(warningPopupPrefab, FindObjectOfType<Canvas>().transform);
    }

    ///<summary>
    /// Actualiza el texto del botón con la tecla actual asignada.
    ///</summary>
    void UpdateButtonText()
    {
        var binding = actionReference.action.bindings[bindingIndex];
        string readable = InputControlPath.ToHumanReadableString(
            binding.effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );
        buttonText.text = readable;
    }

    ///<summary>
    /// Guarda los bindings personalizados en PlayerPrefs (como JSON) para que sean persistentes.
    ///</summary>
    void SaveBinding()
    {
        PlayerPrefs.SetString(
            actionReference.action.id.ToString(),
            actionReference.action.SaveBindingOverridesAsJson()
        );
    }

    ///<summary>
    /// Carga los bindings guardados en PlayerPrefs y los aplica si existen.
    ///</summary>
    public void LoadBinding()
    {
        if (PlayerPrefs.HasKey(actionReference.action.id.ToString()))
        {
            actionReference.action.LoadBindingOverridesFromJson(
                PlayerPrefs.GetString(actionReference.action.id.ToString())
            );
            UpdateButtonText();
        }
    }

}
