using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameState gameState = GameState.Playing;
    public GameObject[] players;

    public void ChangeGameState(GameState state)
    {
        //Guard statement
        if (gameState == state)
            return;

        gameState = state;

        switch (gameState)
        {
            case GameState.Playing:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
            default:
                break;
        }
    }

    public void CheckWinState()
    {
        int aliveCount = 0;

        foreach (GameObject player in players)
        {
            if (player.activeSelf)
            {
                aliveCount++;
            }
        }

        if (aliveCount <= 1)
        {
            Invoke(nameof(NewRound), 3f);
        }
    }

    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum GameState { Playing, Win, Lose, }
