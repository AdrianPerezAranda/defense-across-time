using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] GameObject audioMenu;
    [SerializeField] GameObject optionsMenu;

    private Coroutine lastCoroutine;

    private Animator animatior;

    private void Start()
    {
        animatior = GetComponent<Animator>();

        animatior.enabled = true;
    }
    /// <summary>
    /// Funcion que se llamara cuando se presione el boton de "Audio", tanto en el main menu como en el menu de pausa
    /// 
    /// Hace que el menu actual se desactive y que se active el menu de audio.
    /// 
    /// Cuando la ultima corrutina es null llama a la corrutina.
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (lastCoroutine == null) lastCoroutine = StartCoroutine(_ButtonAnimation());
    }

    private IEnumerator _ButtonAnimation()
    {
        animatior.SetTrigger("Pressed");

        MusicController.instance.PlayButtonClick();

        yield return new WaitForSecondsRealtime(0.15f);

        animatior.SetTrigger("Normal");

        yield return new WaitForSecondsRealtime(0.12f);

        optionsMenu.SetActive(false);

        audioMenu.SetActive(true);

        lastCoroutine = null;
    }

    public void debug()
    {
        print("TOcado");
    }
}
