using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlocoSpawner : MonoBehaviour
{
  public GameObject Bloco;
  public GameObject BlocoAzul;
  public GameObject BlocoVermelho;
  GameManager gm;

    void Start()
    {
        gm = GameManager.GetInstance();
        GameManager.changeStateDelegate += Construir;
        Construir();
    }

    void Construir()
    {
        

        if (gm.gameState == GameManager.GameState.GAME)
        {
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
            for(int i = 0; i < 12; i++)
            {
                for(int j = 0; j < 4; j++){
                    Vector3 posicao = new Vector3(-8.55f + 1.55f * i, 4 - 0.55f * j);
                    if (j % 2 == 0)
                    {
                        if (i % 3 == 0)
                        {
                            Instantiate(BlocoAzul, posicao, Quaternion.identity, transform);
                        }
                        else
                        {
                            Instantiate(Bloco, posicao, Quaternion.identity, transform);
                        }
                    }
                    else
                    {
                        if (i % 4 == 0)
                        {
                            Instantiate(BlocoVermelho, posicao, Quaternion.identity, transform);
                        }
                        else
                        {
                            Instantiate(Bloco, posicao, Quaternion.identity, transform);
                        }
                    }
                }
            }
        }
    }

    void Update()
    {
        if (transform.childCount <= 0 && gm.gameState == GameManager.GameState.GAME)
        {
            gm.ChangeState(GameManager.GameState.ENDGAME);
        }
    }

}
