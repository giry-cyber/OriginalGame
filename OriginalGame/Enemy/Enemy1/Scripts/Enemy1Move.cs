using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy1Move : MonoBehaviour
{
    [SerializeField] public float enemyMoveSpeed = 3.0f;
    [SerializeField] private float enemyWidthOffset = 1.0f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] int hp = 1;

    static int hashHurt = Animator.StringToHash("hurt");
    static int hashDeath = Animator.StringToHash("death");
    private bool isDamage = false;
    private bool isDead = false;
    [SerializeField, HideInInspector] Animator animator;


    Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {        


        if (isDead == false)
        {
            Vector3 axis = new Vector3(enemyMoveSpeed, 0, 0).normalized;
            var distanceFromwall = Physics2D.Raycast(transform.position, axis, enemyWidthOffset, groundMask);

            Vector3 criffAxiss = new Vector3(enemyMoveSpeed, -100, 0).normalized;
            var criffAhead = Physics2D.Raycast(transform.position, criffAxiss, 2, groundMask);

            if (distanceFromwall.distance != 0)
            {
                enemyMoveSpeed *= -1;
            }

            if (criffAhead.collider == null)
            {
                enemyMoveSpeed *= -1;
            }

            transform.position += new Vector3(enemyMoveSpeed * Time.deltaTime, 0, 0);   
            if(hp <= 0 )
            {
                animator.SetTrigger(hashDeath);
                collider.enabled = false;
                isDead = true;
            
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("test");
        if (collision.gameObject.tag == "PlayerAttack")
        {
            Debug.Log("hit");
            animator.SetTrigger(hashHurt);
            hp--;
        }
    }

    private void Death()
    {
        Debug.Log("test");
        Destroy(gameObject);
    }
}
