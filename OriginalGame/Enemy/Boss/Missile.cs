using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    Vector3 velocity;
    Vector3 position;
    public GameObject target;
    float period = 1.5f;
    SpriteRenderer sr;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        position = transform.position;
        velocity = new Vector3(
            Random.Range(-5f, 5f),
            Random.Range(-5f, 5f),
            Random.Range(-5f, 5f)
        );
        sr = GetComponent<SpriteRenderer>();
        //Debug.Log("postion = " +  transform.position);
    }

    // Update is called once per frame
    void Update()
    {



        if (target == null) Destroy(gameObject);

        if (period > 0.5)
        {
            var accelaration = Vector3.zero;
            var diff = target.transform.position - position;
            accelaration += (diff - velocity * period) * 2f / (period * period);

            velocity += accelaration * Time.deltaTime;
        }
        period -= Time.deltaTime;


     
        //Debug.Log("velocity = " + velocity);
        position += velocity * Time.deltaTime;
        transform.position = position;
        //Debug.Log("position"+ transform.position) ;

        if(period < 0&& sr.isVisible == false)
        {
            Destroy(gameObject);
        }

    }
}
