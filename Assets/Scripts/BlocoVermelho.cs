using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocoVermelho : MonoBehaviour
{
    private int vida = 5;
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (vida > 0)
        {
            vida--;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
