using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManeger : MonoBehaviour
{
    [SerializeField] private Slider hp;
    private PlayerStatus status;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            status = player.GetComponent<PlayerStatus>();
            Debug.Log("Chatch");
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        UpDatePlayerHP();
    }

    private void UpDatePlayerHP()
    {
        hp.maxValue = status.maxHp;
        hp.value = status.HP;
    }
}
