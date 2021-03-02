using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parte de colisão da bola com as laterais copiada com pequenas mudanças desse deste link: 
// https://forum.unity.com/threads/im-not-able-to-keep-a-sprite-on-screen-properly.382540/

public class MovimentoBola : MonoBehaviour
{
    [Range(1, 15)]
    public float velocidade = 5.0f;
    private Vector3 direcao;
    GameManager gm;
    GameObject ponteiro;

    private Vector2 leftBottom;
    private Vector2 rightTop;
    private SpriteRenderer spriteRenderer;
    private Vector2 spriteSize;
    private Vector2 spriteHalfSize;

    private void NaRaquete()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.position = playerPosition + new Vector3(0, 0.5f, 0);
    }

    // Start is called before the first frame update
    void Start()
    {   
        NaRaquete();
        gm = GameManager.GetInstance();
        ponteiro = GameObject.FindGameObjectWithTag("Ponteiro");

        leftBottom = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightTop   = Camera.main.ViewportToWorldPoint(Vector3.one);
 
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteSize     = spriteRenderer.bounds.size;
        spriteHalfSize = spriteRenderer.bounds.extents;
    }

    // Update is called once per frame
    void Update()
    {   
        // primeira execução nas rotinas de Update() da Bola e Raquete.
        if (gm.gameState != GameManager.GameState.GAME) return;

        if(Input.GetKeyDown(KeyCode.Space) && gm.gameState == GameManager.GameState.GAME && !gm.launched) {
            gm.launched = true;

            float rad = gm.angulo * (Mathf.PI / 180);
            direcao = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

            ponteiro.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (!gm.launched) 
        {
            NaRaquete();
        }
        else
        {
            transform.position += direcao * Time.deltaTime * velocidade;

            Vector2 posicaoViewport = Camera.main.WorldToViewportPoint(transform.position);

            // get the sprite's edge positions
            float spriteLeft   = transform.position.x - spriteHalfSize.x;
            float spriteRight  = transform.position.x + spriteHalfSize.x;
            float spriteBottom = transform.position.y - spriteHalfSize.y;
            float spriteTop    = transform.position.y + spriteHalfSize.y;
    
            // if any of the edges surpass the camera's bounds,
            // set the position TO the camera bounds (accounting for sprite's size)
            if(spriteLeft < leftBottom.x)
            {
                direcao = new Vector3(-direcao.x, direcao.y);
            }
            else if(spriteRight > rightTop.x)
            {
                direcao = new Vector3(-direcao.x, direcao.y);
            }
    
            if(spriteTop < leftBottom.y)
            {
                Reset();
            }
            else if(spriteTop > rightTop.y)
            {
                direcao = new Vector3(direcao.x, -direcao.y);
            }
        }
    }

    private void Reset()
    {
        gm.launched = false;
        ponteiro.GetComponent<SpriteRenderer>().enabled = true;

        NaRaquete();

        gm.vidas--;

        if(gm.vidas <= 0 && gm.gameState == GameManager.GameState.GAME)
        {   
            if (gm.pontos > gm.highScore)
            {
                gm.highScore = gm.pontos;
                PlayerPrefs.SetInt("SavedHighScore", gm.pontos);
            }
            gm.ChangeState(GameManager.GameState.ENDGAME);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {   
        bool upDown = false; 

        if(col.gameObject.CompareTag("Player"))
        {   
            float dirX;
            float dirY;
            if (transform.localPosition[1] >= col.bounds.center[1] + col.bounds.extents[1] || transform.localPosition[1] <= col.bounds.center[1] - col.bounds.extents[1]) 
            {
                upDown = true;
            }

            if (upDown)
            {
                dirX = (transform.localPosition[0] - col.bounds.center[0]);
                dirY = 1.0f;
            }
            else
            {
                dirX = Random.Range(-1.0f, 1.0f);
                dirY = Random.Range(1.0f, 1.0f);
            }

            direcao = new Vector3(dirX, dirY).normalized;
        }
        else if(col.gameObject.CompareTag("Bloco"))
        {   
            if (transform.localPosition[1] >= col.bounds.center[1] + 0.25 || transform.localPosition[1] <= col.bounds.center[1] - 0.25) 
            {
                upDown = true;
            }   

            if (upDown)
            {
                direcao = new Vector3(direcao.x, -direcao.y);
            } 
            else
            {
                direcao = new Vector3(-direcao.x, direcao.y);
            }
            
            gm.pontos++;
        }
    }
}
