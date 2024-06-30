using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWarp : MonoBehaviour
{
    private BattleSpriteActionCustom bsac;
    [SerializeField] private GameObject warpMarkerPrefab;
    [SerializeField] private GameObject warpGate;
    private GameObject warpMarker;
    [SerializeField] private float throwingSpeed = 15.0f;
    [SerializeField] Transform throwingPoint;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        bsac = GetComponent<BattleSpriteActionCustom>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bsac.isDamaged == false && bsac.isAttacking == false) 
        {
            if(Input.GetKeyDown(KeyCode.X)) 
            {
                float HorizontalAxis = Input.GetAxisRaw("Horizontal");
                float verticalAxis = Input.GetAxisRaw("Vertical");

                if(warpMarker == null)
                {
                    warpMarker = Instantiate(warpMarkerPrefab, throwingPoint.position, throwingPoint.rotation);
                    warpMarker.GetComponent<Rigidbody2D>().velocity = new Vector2 (HorizontalAxis, verticalAxis).normalized * throwingSpeed;
                    rb.velocity = Vector2.zero;
                }
                else
                {
                    
                    Instantiate(warpGate,warpMarker.transform.position, warpMarker.transform.rotation);
                    transform.position = warpMarker.transform.position;
                    Destroy(warpMarker.gameObject);
                    warpMarker = null;
                }
            }

            if(Input.GetKeyDown(KeyCode.C))
            {
                if(warpMarker != null)
                {
                    Destroy(warpMarker.gameObject);
                    warpMarker = null;
                }
            }
        }
        
    }




}
