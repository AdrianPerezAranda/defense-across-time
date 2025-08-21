using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    [SerializeField] GameObject programmers;

    [SerializeField] GameObject programmer1;

    [SerializeField] GameObject programmer2;

    [SerializeField] GameObject programmer3;

    [SerializeField] GameObject artists;

    [SerializeField] GameObject artist1;

    [SerializeField] GameObject artist2;

    [SerializeField] GameObject artist3;

    [SerializeField] GameObject artist4;

    [SerializeField] GameObject artist5;
    void Start()
    {
        StartCoroutine(_WaitCredits());
    }

    private IEnumerator _WaitCredits()
    {
        programmers.SetActive(true);

        yield return new WaitForSeconds(1);

        programmer1.SetActive(true);

        yield return new WaitForSeconds(1);

        programmer2.SetActive(true);

        yield return new WaitForSeconds(1);

        programmer3.SetActive(true);

        yield return new WaitForSeconds(1);

        artists.SetActive(true);

        yield return new WaitForSeconds(1);

        artist1.SetActive(true);

        yield return new WaitForSeconds(1);

        artist2.SetActive(true);

        yield return new WaitForSeconds(1);

        artist3.SetActive(true);

        yield return new WaitForSeconds(1);

        artist4.SetActive(true);

        yield return new WaitForSeconds(1);

        artist5.SetActive(true);

        yield return new WaitForSeconds(12);

        ChangeToWinScene();
    }

    public void ChangeToWinScene()
    {
        SceneManager.LoadScene("WinScene");
    }
}
