using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupDestoy : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] GameObject parent;
    private Coroutine lastCoroutine;

    private Animator animatior;

    private void Start()
    {
        animatior = GetComponent<Animator>();

        animatior.enabled = true;
    }

    /// <summary>
    /// Funcion que se llamara cuando se pulse el boton de "Entendido" del popup que sale cuando intentas cambiar a una tecla ya asignada.
    /// 
    /// Cuando lastCoroutine es null llama a la corrutina.
    /// </summary>

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Presionado");
        if (lastCoroutine == null) lastCoroutine = StartCoroutine(_ButtonAnimation());

    }

    /// <summary>
    /// Corrutina que se llama cuando se presione el boton de "Entendido" y lastCoroutine sea null.
    /// 
    /// Hace que el boton haga una animacion y un sonido, despues hace que se destuya el popup.
    /// </summary>
    private IEnumerator _ButtonAnimation()
    {

        animatior.SetTrigger("Pressed");

        MusicController.instance.PlayButtonClick();

        yield return new WaitForSecondsRealtime(0.15f);

        animatior.SetTrigger("Normal");

        yield return new WaitForSecondsRealtime(0.12f);

        MusicController.instance.PlayButtonClick();

        Destroy(parent);

        lastCoroutine = null;
    }
}
