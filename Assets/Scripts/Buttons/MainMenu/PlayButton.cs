using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour, IPointerUpHandler
{
    private Coroutine lastCoroutine;

    private Animator animatior;

    private bool tutorialYaCompletado;

    private void Start()
    {
        animatior = GetComponent<Animator>();

        animatior.enabled = true;

        tutorialYaCompletado = PlayerPrefs.GetInt("TutorialCompletado", 0) == 1;
    }
    /// <summary>
    /// Funcion que se llamara cuando se preione el boton de "Jugar", en el main menu
    /// 
    /// Cuando lastCoroutine es null llama a la corrutina.
    /// </summary
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Presionado");
        if (lastCoroutine == null) lastCoroutine = StartCoroutine(_ButtonAnimation());

    }

    /// <summary>
    /// Corrutina que se llama cuando se preione el boton de "Jugar" y lastCoroutine sea null.
    /// 
    /// Hace que el boton haga una animacion y un sonido, despues se carge la escena de carga y indica la era que se va a jugar.
    /// </summary>
    private IEnumerator _ButtonAnimation()
    {

        animatior.SetTrigger("Pressed");

        MusicController.instance.PlayButtonClick();

        yield return new WaitForSecondsRealtime(0.15f);

        animatior.SetTrigger("Normal");

        yield return new WaitForSecondsRealtime(0.12f);

        SceneManager.LoadScene("LoadingScreen");

        if(tutorialYaCompletado)
        { 
            PlayerPrefs.SetInt("Eras", PlayerPrefs.GetInt("Eras", 1));
        }
        else
        {
            PlayerPrefs.SetInt("Eras", 0);
        }
        

        lastCoroutine = null;
    }
}
