using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartEraButton : MonoBehaviour
{
    /// <summary>
    /// Funcion que se llamara cuando se pulse el boton de "Reiniciar Epoca", en el lose menu
    /// 
    /// Hace que se carge la escena de carga y vuelve a jugar la era en la que ha perdido. 
    /// </summary>
    public void RestartEra()
    {
        SceneManager.LoadScene("LoadingScreen");
    }
}
