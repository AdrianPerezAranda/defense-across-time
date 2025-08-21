using UnityEngine;
using UnityEngine.Rendering;

public class ArmaMelee : MonoBehaviour
{
    [SerializeField] private float damage = 2;
    [SerializeField] private Collider colliderPrueba;

    bool hitDone = false;

    void Start()
    {
        colliderPrueba.enabled = false;
    }

 
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponent<Animator>().SetBool("Atacando", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            gameObject.GetComponent<Animator>().SetBool("Atacando", false);
        }
    }

    public void ActivarCollider()
    {
        hitDone = false;
        colliderPrueba.enabled = true;
    }

    public void DesactivarCollider()
    {
        colliderPrueba.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemigo") && !hitDone)
        {
            hitDone = true;
            print("Le has hecho daño: " + damage);
            other.gameObject.GetComponent<Enemy>().ReciveDamage(damage);
        }
    }
}
