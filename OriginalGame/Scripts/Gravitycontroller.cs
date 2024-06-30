using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class GravityController : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider;
    private Rigidbody2D rb;
    [SerializeField] private float gravityAcceleration = 9.8f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float epsilon = 0.1f; // ���e�덷
    private Vector2 velocity;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Rigidbody2D��Kinematic�ɐݒ�
    }

    void Update()
    {
        // �I�u�W�F�N�g�̌��݂̈ʒu���擾
        Vector2 position = rb.position;

        // Raycast���g���Ēn�ʂƂ̋����𑪒�
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, capsuleCollider.size.y / 2 + epsilon, groundMask);

        if (hit.collider == null)
        {
            // �n�ʂ��Ȃ��ꍇ�͏d�͂�K�p
            velocity += Vector2.down * gravityAcceleration * Time.deltaTime;
        }
        else
        {
            // �n�ʂɐڐG���Ă���ꍇ�͐��������̑��x���[���ɂ���
            velocity.y = 0;
            position.y = hit.point.y + capsuleCollider.size.y / 2; // �n�ʂ̕\�ʂɈʒu��ݒ�
        }

        // �ʒu���X�V
        rb.MovePosition(position + velocity * Time.deltaTime);
    }
}
