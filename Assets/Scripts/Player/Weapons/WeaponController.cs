using FMODUnity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class WeaponController : MonoBehaviour
{
    public enum Weapono
    {
        trabuco, 
        raygun
    }

    public Weapono weapon;

    [SerializeField] InputActionAsset input; // Referencia al sistema de entrada de Unity.
    [SerializeField] GameObject projectilePrefab; // Prefab del proyectil que se disparará.

    public Transform weaponMuzzle; // Punto de salida del proyectil.
    public float delayBetweenShots; // Tiempo de espera entre disparos.
    public int bulletsCharger; // Número total de balas en el cargador.
    public float bulletSpreadAngle; // Ángulo de dispersión de las balas.
    public float rechargeTime; // Tiempo necesario para recargar el arma.

    private InputAction shot; // Acción de disparo definida en el sistema de entrada.
    private float delayShot; // Tiempo de espera entre disparos (valor original).
    private int bullets; // Contador de balas restantes en el cargador.

    private Quaternion origPos; // Rotación original del arma.

    private bool isReloading = false;

    [SerializeField] private Vector3 originalMuzzleRotation;

    void Start()
    {
        shot = input.FindActionMap("Player").FindAction("Attack");

        delayShot = delayBetweenShots;

        bullets = bulletsCharger;

        origPos = transform.localRotation;

    }

    void Update()
    {

        CalculateMuzzleDirection();

        // Reduce el tiempo de espera entre disparos cada frame.
        if (delayBetweenShots > 0)
        {
            delayBetweenShots -= Time.deltaTime;
        }

        // Verifica si el jugador presiona el botón de disparo, hay balas disponibles y el tiempo de espera ha terminado.
        if (shot.ReadValue<float>() > 0 && bullets > 0 && delayBetweenShots <= 0)
        {
            Shoot();
        }
        // Si no quedan balas y no se está recargando, inicia el proceso de recarga.
        else if (bullets <= 0 && !isReloading)
        {
            StartCoroutine(_Recharge());
        }
    }

    private void CalculateMuzzleDirection()
    {
        //Vector desde el origen de la pantalla
        Vector3 rayOrigin = new (.5f, .5f, 0);
        //Rayo desde el centro de la pantalla
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * rayOrigin.x, Screen.height * rayOrigin.y, 0));

        //Si chocha con algo
        if(Physics.Raycast(ray, out RaycastHit hit, 1000))
        {
            //print(hit.collider.gameObject.name);

            //Si esta demasiado cerca del objetivo no cambia la direccion de la bala
            if (hit.distance < 10) weaponMuzzle.transform.LookAt(ray.GetPoint(100));
            //Si esta a mas de 10u la bala tomará esa direccion
            else weaponMuzzle.transform.LookAt(hit.point);
        }
        //Si no choca con nada la bala tomara la direccion original
        else
        {
            Debug.DrawLine(ray.origin, ray.GetPoint(100), Color.red);
            weaponMuzzle.transform.LookAt(ray.GetPoint(100));
        }
    }

    /// <summary>
    /// Función encargada de realizar el disparo del arma.
    /// 
    /// Calcula la dirección del disparo considerando la dispersión, 
    /// instancia un proyectil y reproduce el sonido del disparo. 
    /// Además, reinicia el tiempo de espera entre disparos y reduce 
    /// la cantidad de balas disponibles en el cargador.
    /// </summary>
    private void Shoot()
    {
        // Direccion donde esta apuntando el arma.
        Vector3 shotDirection = GetShotDirectionWithinSpread(weaponMuzzle);
        
        PlaySfx();

        // Instancia una bala
        Instantiate(projectilePrefab, weaponMuzzle.position, Quaternion.LookRotation(shotDirection));

        // Se reinicia el tiempo entre disparos
        delayBetweenShots = delayShot;

        // Se resta una bala del cargador
        bullets--;
    }

    private void PlaySfx()
    {
        MusicController.instance.PlayBlunderbussShot(transform);
        switch (weapon)
        {
            case Weapono.trabuco:
                break;

        }
    }

    public Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
    {
        float spreadAngleRatio = bulletSpreadAngle / 180f;
        Vector3 spreadWorldDirection = Vector3.Slerp(shootTransform.forward, UnityEngine.Random.insideUnitSphere, spreadAngleRatio);

        return spreadWorldDirection;
    }

    /// <summary>
    /// Corrutina que se llama al no tener balas el arma.
    /// 
    /// Hay un tiempo de recarga, al terminar el tiempo se recargan todas las balas.
    /// </summary>
    /// <returns></returns>
    private IEnumerator _Recharge()
    {
        isReloading = true;

        //Esto se tiene que cambiar por la animacion de recarga del arma
        transform.localRotation = origPos;

        yield return new WaitForSeconds(rechargeTime);

        bullets = bulletsCharger;
        isReloading = false;

        transform.localRotation = origPos;

        yield return null;
    }

    private void OnDisable()
    {
        bullets = bulletsCharger;
        isReloading = false;
    }
}
