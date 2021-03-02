using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoPonteiro : MonoBehaviour
{
    GameManager gm;
    public bool side;
    // Start is called before the first frame update
    void Start()
    {
        side = true;
        gm = GameManager.GetInstance();
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Bola").transform.position;
        transform.position = playerPosition + new Vector3(0, 0.3f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // primeira execução nas rotinas de Update() da Bola e Raquete.
        if (gm.gameState != GameManager.GameState.GAME) return;

        if (!gm.launched)
        {   
            if (side)
            {
                gm.angulo += 100 * Time.deltaTime;
            }
            else
            {
                gm.angulo -= 100 * Time.deltaTime;
            }

            float rad = gm.angulo * (Mathf.PI / 180);
            
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Bola").transform.position;
            transform.position = playerPosition + new Vector3(Mathf.Cos(rad)*0.3f, Mathf.Sin(rad)*0.3f, 0);

            if (gm.angulo > 170.0f)
            {
                side = false;
            }

            if (gm.angulo < 10.0f)
            {
                side = true;
            }
        }

        
    }
}
