using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Enemy enemyClass;

    private enum tipoAtaque
    {
        CuerpoACuerpo,
        Distancia,
        Ambos
    }
    [SerializeField] private tipoAtaque tipoDeAtaque;

    [Header("Cuerpo a Cuerpo")]
    [SerializeField] float radius;
    [SerializeField] Transform attackPoint;
    [Header("A distancia")]
    [SerializeField] GameObject proyectil;
    [SerializeField] float proyectilSpeed;
    [SerializeField] Transform spawnPoint;

    float damage;
    bool attacking;
    bool hitDone;

    private void Awake()
    {
        enemyClass = GetComponent<Enemy>();
        damage = enemyClass.damage;
    }

    #region Eventos

    public void StartAttacking()
    {
        attacking = true;
        hitDone = false;
    }
    public void StopAttacking()
    {
        attacking = false;
        hitDone = false;

        enemyClass.CheckNewState();
    }
    #endregion

    private void Start()
    {
        attacking = false;
    }

    private void Update()
    {
        //Si esta atacando y no ha hecho daño aún
        if (attacking && !hitDone) 
            CheckAttackColliders();
    }

    #region CaC
    /// <summary>
    /// Hace daño al jugador si esta dentro del collider del ataque cuerpo a cuerpo
    /// </summary>
    private void CheckAttackColliders()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, radius);

        foreach (Collider collider in hits)
        {
            if(collider.CompareTag("Player"))
            {
                collider.GetComponent<PlayerController>().ReciveDamage(damage);
                hitDone = true;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, radius);
    }
    #endregion

    #region A Distancia
    /// <summary>
    /// Funcion que se llamara desde la animacion de disparar
    /// </summary>
    public void SpawnBullet()
    {
        GameObject bullet = Instantiate(proyectil, spawnPoint.position, spawnPoint.rotation, null);
        bullet.GetComponent<Rigidbody>().linearVelocity = proyectilSpeed * Vector3.forward;
    }
    #endregion



}
