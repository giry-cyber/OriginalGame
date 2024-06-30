using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{

    private BattleSpriteActionCustom bsac;
    private Animator animator;
    [SerializeField] Collider2D attackCollider;
    [SerializeField] Collider2D finishBrowCollider;


    // Start is called before the first frame update
    void Start()
    {
        bsac = GetComponent<BattleSpriteActionCustom>();
        animator = GetComponent<Animator>();
        if (attackCollider != null) Debug.Log("find attack collider");
    }

    public void OnAttackStart()
    {
        bsac.isAttacking = true;
    }

    public void OnAttackEnd()
    {
        bsac.isAttacking=false;
    }

    private void OnFinifshBrowStart()
    {
        bsac.isAttacking = true;
        bsac.canAttack = false;
        StartCoroutine(AttackCoolTime());
    }

    private void OnFinishBrowEnd()
    {
        bsac.isAttacking = false;
    }

    private IEnumerator AttackCoolTime()
    {
        yield return new WaitForSeconds(bsac.attackCoolTime);
        bsac.canAttack=true;
    }

    private void EnableAttackColllider()
    {
        Debug.Log("Attack Collider Active");
        attackCollider.enabled = true;
    }

    private void DisableAttackCollider()
    {
        Debug.Log("Attack Collider disActivate");
        attackCollider.enabled = false;
    }

    private void EneableFinishBrowCollider()
    {
        Debug.Log("finish Brow Activate");
        finishBrowCollider.enabled = true;
    }

    private void DisableFinishBrowCollider()
    {
        Debug.Log("finish Brow disActivate");
        finishBrowCollider.enabled = false;
    }
}
