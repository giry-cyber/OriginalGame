using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{

    [SerializeField] private float rotateSpeed = 180;
    Rigidbody2D rb;
    Collider2D collider;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        collider = GetComponent<Collider2D>();
        player = GameObject.FindWithTag("Player");
        if(player == null)
        {
            Debug.Log("no player");
        }
        else if(player.GetComponent<Rigidbody2D>() == null) 
        {
            Debug.Log("no player collider" + player.name);
        }
        Physics2D.IgnoreCollision(collider, player.GetComponent<Collider2D>(), true);
        
    }

    // Update is called once per frame
    void Update()
    {   
    }


}
