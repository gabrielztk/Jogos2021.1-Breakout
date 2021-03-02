using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parte de colisão da raquete com as laterais copiada com pequenas mudanças desse deste link: 
// https://forum.unity.com/threads/im-not-able-to-keep-a-sprite-on-screen-properly.382540/

public class MovimentoRaquete : MonoBehaviour
{   
    [Range(1, 30)]
    public float velocidade;
    GameManager gm;

    private Vector2 leftBottom;
    private Vector2 rightTop;
    private SpriteRenderer spriteRenderer;
    private Vector2 spriteSize;
    private Vector2 spriteHalfSize;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        // get the world location of the bottom left corner and top right corner of camera
        // if your camera moves, this will have to be in Update or LateUpdate
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
        float inputX = Input.GetAxis("Horizontal");

        transform.position += new Vector3(inputX, 0, 0) * Time.deltaTime * velocidade;

        if(Input.GetKeyDown(KeyCode.Escape) && gm.gameState == GameManager.GameState.GAME) {
            gm.ChangeState(GameManager.GameState.PAUSE);
        }
    }

    // do your normal movement in Update, then LateUpdate will correct the position before rendering
    private void LateUpdate()
    {
        // get the sprite's edge positions
        float spriteLeft   = transform.position.x - spriteHalfSize.x;
        float spriteRight  = transform.position.x + spriteHalfSize.x;
 
        // initialize the new position to the current position
        Vector3 clampedPosition = transform.position;
 
        // if any of the edges surpass the camera's bounds,
        // set the position TO the camera bounds (accounting for sprite's size)
        if(spriteLeft < leftBottom.x)
        {
            clampedPosition.x = leftBottom.x + spriteHalfSize.x;
        }
        else if(spriteRight > rightTop.x)
        {
            clampedPosition.x = rightTop.x - spriteHalfSize.x;
        }
 
        transform.position = clampedPosition;
    }
}
