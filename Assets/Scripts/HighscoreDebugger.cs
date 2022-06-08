using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreDebugger : MonoBehaviour
{
    [SerializeField] private Highscores highscores;
    [SerializeField] private Text playerName;
    [SerializeField] private Text score;
    [SerializeField] private int amount = 10;
    
    public void GetScore()
    {
        highscores.GetHighscores(amount);
        highscores.OnHighscoresReceived += OnHighscoresReceived;
    }

    private void OnHighscoresReceived(List<SingleNameScore> highscores)
    {
        Debug.Log("Highscores received");
        
        foreach (SingleNameScore score in highscores)
            Debug.Log(score.name + ": " + score.score);
    }

    public void CreateScore()
    {
        highscores.CreateHighscore(playerName.text, int.Parse(score.text));
    }
}
