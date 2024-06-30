using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private int hp;
    public int maxHp;

    public int HP
    {
        get { return hp; }
        set { hp = value;  }
    }

    void Start()
    {
        HP = maxHp; // ‰ŠúHP‚ğmaxHp‚Éİ’è
    }


}
