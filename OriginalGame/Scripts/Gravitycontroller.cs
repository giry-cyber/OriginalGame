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
    [SerializeField] private float epsilon = 0.1f; // 許容誤差
    private Vector2 velocity;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Rigidbody2DをKinematicに設定
    }

    void Update()
    {
        // オブジェクトの現在の位置を取得
        Vector2 position = rb.position;

        // Raycastを使って地面との距離を測定
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, capsuleCollider.size.y / 2 + epsilon, groundMask);

        if (hit.collider == null)
        {
            // 地面がない場合は重力を適用
            velocity += Vector2.down * gravityAcceleration * Time.deltaTime;
        }
        else
        {
            // 地面に接触している場合は垂直方向の速度をゼロにする
            velocity.y = 0;
            position.y = hit.point.y + capsuleCollider.size.y / 2; // 地面の表面に位置を設定
        }

        // 位置を更新
        rb.MovePosition(position + velocity * Time.deltaTime);
    }
}
