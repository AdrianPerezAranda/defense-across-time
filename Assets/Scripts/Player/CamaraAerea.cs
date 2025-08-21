using System.Collections;
using UnityEngine;

public class CamaraAerea : MonoBehaviour
{
    [SerializeField] private GameObject avisoTextGameObject;

    bool playerInside = false;
    bool viewChanged = false;

    private void Update()
    {
        if(playerInside) avisoTextGameObject.transform.LookAt(GameController.Instance.playerInstance.gameObject.transform);


        if(playerInside && GameController.Instance.playerInstance.inputMovement.FindActionMap("Player").FindAction("Interact").ReadValue<float>() != 0 && !viewChanged)
        {
            CambiarCamara(true);
        }
        else if (viewChanged && GameController.Instance.playerInstance.inputMovement.FindActionMap("Player").FindAction("Interact").ReadValue<float>() != 0)
        {
            CambiarCamara(false);
        }

    }

    private void CambiarCamara(bool cambiarAerea)
    {
        GameObject player = GameController.Instance.playerInstance.gameObject;
        player.transform.GetChild(1).gameObject.SetActive(!cambiarAerea);
        player.GetComponent<PlayerMovimiento>().enabled = !cambiarAerea;
        player.GetComponent<CamaraController>().enabled = !cambiarAerea;

        StartCoroutine(CambiarViewChanged(cambiarAerea));
    }


    IEnumerator CambiarViewChanged(bool cambiarAerea)
    {
        yield return new WaitForSeconds(1f);
        viewChanged = cambiarAerea;
    }
    #region triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            avisoTextGameObject.SetActive(true);
            playerInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            avisoTextGameObject.SetActive(false);
            playerInside = false;
        }
    }
    #endregion
}

