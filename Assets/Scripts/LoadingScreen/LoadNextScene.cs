using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI text;

    private int eras;

    private AsyncOperation loadOperation;
    void Start()
    {
        GameController.Instance.NewMapLoaded();

        eras = PlayerPrefs.GetInt("Eras", 0);
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(.1f);
            text.text = (i*5).ToString() + "%";
        }


        switch (eras)
        {
            case 0: loadOperation = SceneManager.LoadSceneAsync("TutorialScene");
                break;
            case 1: loadOperation = SceneManager.LoadSceneAsync("Prehistory");
                break;

            case 2: loadOperation = SceneManager.LoadSceneAsync("EdadMedia_Nueva");
                break;

            case 3: loadOperation = SceneManager.LoadSceneAsync("Future");
                break;
        }
        

        while (!loadOperation.isDone)
        {
            if(loadOperation.progress >= .5f) text.text = (loadOperation.progress * 100) + "%";

            yield return null;
        }

        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);

        while (unloadOperation != null)
        {
            Debug.Log("No hay una scena activa. La actual es: " + SceneManager.GetActiveScene().name);
        }

        while (!unloadOperation.isDone)
        {
            yield return null;
        }

        loadOperation.allowSceneActivation = true;

    }
}
