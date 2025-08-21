    using UnityEngine;

public class EnemiesAnimationController : MonoBehaviour
{
    private Enemy enemy;
    private Animator animator;
    
    private void Awake()
    {
        enemy = transform.parent.GetComponent<Enemy>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        enemy.onStartAttacking.AddListener(StartAnimAttacking);
        enemy.onStartChasing.AddListener(StartAnimWalking);
        enemy.onStartPathToBase.AddListener(StartAnimWalking);
        enemy.onStartDying.AddListener(StartAnimDying);
    }

    private void OnDisable()
    {
        enemy.onStartAttacking.RemoveListener(StartAnimAttacking);
        enemy.onStartChasing.RemoveListener(StartAnimWalking);
        enemy.onStartPathToBase.RemoveListener(StartAnimWalking);
        enemy.onStartDying.RemoveListener(StartAnimDying);
    }

    private void StartAnimWalking()
    {
        animator.SetBool("Walking", true);
    }

    private void StartAnimAttacking()
    {
        animator.SetBool("Walking", false);
    }
    
    private void StartAnimDying()
    {
        animator.SetTrigger("Die");
        EnemiesController.instance.KillEnemy(transform.parent.gameObject);
    }


    #region AnimationEvents
    public void StartAttacking()
    {
        transform.parent.GetComponent<EnemyAttack>().StartAttacking();
    }
    public void StopAttacking()
    {
        transform.parent.GetComponent<EnemyAttack>().StopAttacking();
    }
    #endregion
}
