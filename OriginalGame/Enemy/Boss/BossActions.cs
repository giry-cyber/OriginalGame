using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossActions : MonoBehaviour
{
    static int hashSpeed = Animator.StringToHash("Speed");
    static int hashFallSpeed = Animator.StringToHash("FallSpeed");
    static int hashGroundDistance = Animator.StringToHash("GroundDistance");
    static int hashIsCrouch = Animator.StringToHash("IsCrouch");
    static int hashAttack1 = Animator.StringToHash("Attack1");
    static int hashAttack2 = Animator.StringToHash("Attack2"); //HeavySwing
    static int hashAttack3 = Animator.StringToHash("Attack3");
    static int hashDamage = Animator.StringToHash("Damage");
    static int hashIsDead = Animator.StringToHash("IsDead");

    [SerializeField] private float characterHeightOffset = 0.2f; //キャラの高さ
    [SerializeField] private float characterWidthOffset = 1.0f; //キャラの横幅 
    [SerializeField] LayerMask groundMask;
    [SerializeField] public float jumpPower = 10.0f;
    [SerializeField] public float moveSpeed = 5.0f;
    [SerializeField] public int hP;

    [SerializeField, HideInInspector] Animator animator;
    [SerializeField, HideInInspector] SpriteRenderer spriteRenderer;
    [SerializeField, HideInInspector] Rigidbody2D rig2d;
    [SerializeField, HideInInspector] new Collider2D collider;
    private Collision2D enemtCollision;
    private CapsuleCollider2D capsuleCollider;

    public bool canJump;


    [SerializeField] public float attackCoolTime = 5.0f;
    public bool canAttack = true;
    public bool isAttacking = false;
    public bool isMoveing = false;
    public bool isDamaged = false;
    public bool isDead = false;

    private GameObject target;
    [SerializeField] Transform laserPosition;
    [SerializeField] Missile laser;

    [SerializeField] private float hevySwingChaseTimeLImitBase = 3.0f;

    [SerializeField] GameObject enemyWarpGate;
    [SerializeField] Transform sPAttackPosition;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rig2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        characterHeightOffset = capsuleCollider.size.y;
        characterWidthOffset = capsuleCollider.size.x;
        target = GameObject.FindWithTag("Player");
        if(target == null)
        {
            Debug.Log("no player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead ) return;



        // 走るモーションの切り替え　もっといい方法ありそう
        if (isMoveing)
        {
            animator.SetFloat(hashSpeed, 1);
        }
        else
        {
            animator.SetFloat(hashSpeed, 0);
        }
        //

        if (canAttack)
        {
            //StartCoroutine(HeavySwing());
            //StartCoroutine(HormingLaser());

            if (hP > 15)
            {
                //Debug.Log("hp = " + hP);
                int attackCase = Random.Range(0, 2);
                //Debug.Log("Attack" + attackCase);
                switch (attackCase)
                {
                    case 0:
                        StartCoroutine(HeavySwing());
                        break;

                    case 1:
                        StartCoroutine(HormingLaser());
                        break;
                }
            }else
            {
                StartCoroutine(SpecialAttack());
            }

        }
        else
        {
            
        }
        Debug.Log(hP);
    }


    // 攻撃
    private IEnumerator HeavySwing()
    {
        isMoveing = true;
        //Debug.Log("Swing Attack Start");
        canAttack = false;
        float distination = target.transform.position.x;
        if(distination < transform.position.x ) 
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
        while (Mathf.Abs( transform.position.x - distination )> 1)
        {
            Vector3 direction = new Vector3(distination - transform.position.x, 0,0 ).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            yield return null;
        }
        isMoveing = false;
        // スイング攻撃アニメーションを再生
        //Debug.Log("Swing Attack");
        animator.SetTrigger(hashAttack2);

        StartCoroutine(AttackCooling(10.0f));
 
        //animator.SetTrigger("SwingAttack");
    }

    private IEnumerator HormingLaser()
    {
        canAttack = false;
        animator.SetTrigger(hashAttack1);
        StartCoroutine(AttackCooling(attackCoolTime));
        yield return null;

    }

    private void HormingLaserStart()
    {
        // 追尾弾を２個生成
        Debug.Log(laserPosition.transform);
        var laser1 = Instantiate(laser, laserPosition.transform.position, laserPosition.transform.rotation);
        laser1.target = target;

        var laser2 = Instantiate(laser, laserPosition.transform.position, laserPosition.transform.rotation);
        laser2.target = target;


    }

    private IEnumerator SpecialAttack()
    {
        //追尾団を無数に生成
        int angle = 0;
        int rad = 10;
        canAttack = false;

        transform.position = sPAttackPosition.transform.position;
        Instantiate(enemyWarpGate, transform.position, transform.rotation);

        yield return new WaitForSeconds(2.0f);

        while(angle < 360)
        {
            Vector3 instanitatePosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle),0) * rad;
            var bullet = Instantiate(laser, target.transform.position + instanitatePosition, laserPosition.transform.rotation);
            bullet.target = target;
            angle += 20;
        }
        StartCoroutine(AttackCooling(attackCoolTime));
        yield return null;

    }

    // 移動
    private void MoveHorizontal()
    {

    }

    private void JumpBack()
    {

    }

    //スキ
    private void Rest()
    {

    }

    private IEnumerator AttackCooling(float coolTime)
    {
        yield return new WaitForSeconds(coolTime);
        canAttack = true;
    }
    private void AttackStart()
    {
        isAttacking = true;
        canAttack = false;
    }

    private void AttackEnd() 
    {
        isAttacking = false;
    }
    private void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collider" + collision.gameObject.tag);
        if (collision.gameObject.tag == "PlayerAttack")
        {
            hP -= 1;
        }

        if (collision.gameObject.tag == "PlayerAttackFinishBrow")
        {
            hP -= 2;
        }

        if (hP <= 0)
        {
            animator.SetTrigger(hashIsDead);
            canAttack = false;
           
        }
        //Debug.Log("hp=" + hP);
    }
}
