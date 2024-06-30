using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSpriteActionCustom : MonoBehaviour
{
    static int hashSpeed = Animator.StringToHash("Speed");
    static int hashFallSpeed = Animator.StringToHash("FallSpeed");
    static int hashGroundDistance = Animator.StringToHash("GroundDistance");
    static int hashIsCrouch = Animator.StringToHash("IsCrouch");
    static int hashAttack1 = Animator.StringToHash("Attack1");
    static int hashAttack2 = Animator.StringToHash("Attack2");
    static int hashAttack3 = Animator.StringToHash("Attack3");
    static int hashDamage = Animator.StringToHash("Damage");
    static int hashIsDead = Animator.StringToHash("IsDead");

    [SerializeField] private float characterHeightOffset = 0.2f; //キャラの高さ
    [SerializeField] private float characterWidthOffset = 1.0f; //キャラの横幅 
    [SerializeField] LayerMask groundMask;
    [SerializeField] public float jumpPower = 10.0f;
    [SerializeField] public float moveSpeed = 5.0f;

    [SerializeField, HideInInspector] Animator animator;
    [SerializeField, HideInInspector] SpriteRenderer spriteRenderer;
    [SerializeField, HideInInspector] Rigidbody2D rig2d;
    [SerializeField, HideInInspector] new Collider2D collider;
    private PlayerStatus playerStatus;
    private Collision2D enemyCollision;
    private CapsuleCollider2D capsuleCollider;

    public bool canJump;

    private int jumpCount = 0;
    private int maxJumps = 2;

    [SerializeField] public float attackCoolTime = 0.5f;
    public bool canAttack = true;
    public bool isAttacking = false;
    public bool isInvincible = false;
    public bool isDamaged = false;
    public bool isDead = false;







    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rig2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        characterHeightOffset = capsuleCollider.size.y;
        characterWidthOffset = capsuleCollider.size.x;  
        playerStatus = GetComponent<PlayerStatus>();
    }

    void Update()
    {

        if(isDead) return;

        if(playerStatus.HP <= 0)
        {
            animator.SetTrigger(hashIsDead);
            isDead = true;
        }

        if (isDamaged == false)
        {


            if (!isAttacking)
            {
                float HorizontalAxis = Input.GetAxisRaw("Horizontal");
                float verticalAxis = Input.GetAxisRaw("Vertical");
                bool isDown = Input.GetAxisRaw("Vertical") < 0;

                var distanceFromGround = Physics2D.Raycast(transform.position, Vector3.down, 1, groundMask);
                var distanceFromWall = Physics2D.Raycast(transform.position, new Vector3(HorizontalAxis, 0, 0), characterWidthOffset, groundMask);

                if (distanceFromGround.distance != 0 && distanceFromGround.distance <= characterHeightOffset)
                {
                    jumpCount = 0;
                }

                if (Input.GetButtonDown("Jump") && jumpCount < maxJumps - 1)
                {
                    rig2d.velocity = new Vector2(rig2d.velocity.x, jumpPower);
                    jumpCount++;
                }

                // update animator parameters
                animator.SetBool(hashIsCrouch, isDown);
                animator.SetFloat(hashGroundDistance, distanceFromGround.distance == 0 ? 99 : distanceFromGround.distance - characterHeightOffset);
                animator.SetFloat(hashFallSpeed, rig2d.velocity.y);
                animator.SetFloat(hashSpeed, Mathf.Abs(HorizontalAxis));

                if (HorizontalAxis != 0)
                {
                    if (HorizontalAxis < 0) transform.rotation = Quaternion.Euler(0, -180, 0);
                    else transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                //壁貫通防止 +0.1 が無いと壁を貫通することがある　追記１マスのブロックだと貫通することがある．rayを頭と足元から出すようにするべき
                if (distanceFromWall.distance + 0.1 < characterWidthOffset)
                {
                    transform.position += new Vector3(HorizontalAxis * moveSpeed * Time.deltaTime, 0, 0);
                }


            }

            // Handle attacks
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (canAttack == false) return;
                animator.SetTrigger(hashAttack1);
            }

        }
        //Debug.Log(playerStatus.HP);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInvincible)
        {
            // 無敵状態
            return;
            
        }
        if (collision.gameObject.tag == "Enemy")
        {
            isInvincible = true;
            Debug.Log("damaged!");
            enemyCollision = collision;
            animator.SetTrigger(hashDamage);
            playerStatus.HP = playerStatus.HP -1;
            if(collision != null) Physics2D.IgnoreCollision(enemyCollision.collider, collider); //敵とのコライダー無効
            StartCoroutine(InvinivibleTime());
        }
        else if(collision.gameObject.tag =="EnemyAttack")
        {
            isInvincible = true;
            enemyCollision = collision;
            animator.SetTrigger(hashDamage);
            playerStatus.HP = playerStatus.HP - 1;
            StartCoroutine(InvinivibleTime());
        }
    }

    // OnCollisionEnter2Dと同じ処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Enter");
        if (isInvincible)
        {
            // 無敵状態
            return;

        }
        if (collision.gameObject.tag == "Enemy")
        {
            isInvincible = true;
            Debug.Log("damaged!");
            animator.SetTrigger(hashDamage);
            playerStatus.HP = playerStatus.HP - 1;
            StartCoroutine(InvinivibleTime());
        }
        else if (collision.gameObject.tag == "EnemyAttack")
        {
            isInvincible = true;
            animator.SetTrigger(hashDamage);
            playerStatus.HP = playerStatus.HP - 1;
            StartCoroutine(InvinivibleTime());
        }
    }

    private void OnDamageStart()
    {
        isDamaged = true;
    }

    private void OnDamageFinish() 
    {
        isDamaged = false;
        isAttacking = false; //攻撃中に割り込まれるとisAttackingが正常に終了しないから

    }

    private IEnumerator InvinivibleTime()
    {
        yield return new WaitForSeconds(1.0f); //無敵時間
        isInvincible = false;
        if(enemyCollision != null )Physics2D.IgnoreCollision(enemyCollision.collider, collider, false);　//コライダーを有効にする
    }
}

