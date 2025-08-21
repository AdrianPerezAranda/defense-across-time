using System.Collections;
using UnityEngine;

public class CollectablePrestigePoint : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 10);
        StartCoroutine(_AnimPrestigePoints());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other == null) return;

        if (other.gameObject.CompareTag("Player"))
        {
            MusicController.instance.PlayHeadshot(gameObject.transform);
            other.GetComponent<PlayerController>().RecivePrestigeCoins(1000);
            Destroy(gameObject);
        }
    }

    IEnumerator _AnimPrestigePoints()
    {
        transform.position = transform.position + Vector3.up * 2;
        while(true)
        {
            transform.position = transform.position + Vector3.up * Mathf.Sin(Time.time) * .005f;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + Time.deltaTime, transform.rotation.eulerAngles.z);
            yield return null;
        }
    }
}
