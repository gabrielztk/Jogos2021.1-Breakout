using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
   private static GameManager _instance;

   public enum GameState { MENU, GAME, PAUSE, ENDGAME };
   public GameState gameState { get; private set; }
   public int vidas;
   public int pontos;
   public bool launched;
   public float angulo;
   public int highScore;

    public static GameManager GetInstance()
    {
        
        if(_instance == null)
        {
            _instance = new GameManager();
        }

        return _instance;
    }

    private GameManager()
    {
        vidas = 3;
        pontos = 0;
        launched = false;
        gameState = GameState.MENU;
        angulo = 90;
        highScore = 0;


        if (PlayerPrefs.HasKey("SavedHighScore"))
        {
            highScore = PlayerPrefs.GetInt("SavedHighScore");
        }
    }

    public delegate void ChangeStateDelegate();
    public static ChangeStateDelegate changeStateDelegate;

    public void ChangeState(GameState nextState)
    {
        if (nextState == GameState.GAME) Reset();
        gameState = nextState;
        changeStateDelegate();
    }

    private void Reset()
    {
        vidas = 3;
        pontos = 0;
    }
}
