using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class SingleNameScore
{
    public string name { get; set; }
    public int score { get; set; }
}

public class Highscores : MonoBehaviour
{
    public Action<List<SingleNameScore>> OnHighscoresReceived;

    [SerializeField] private string gameName;
    [SerializeField] private string highscoresUrl;

    private List<SingleNameScore> highscores = new List<SingleNameScore>();

    public void GetHighscores(int amount)
    {
        StartCoroutine(LoadHighscores(amount));
    }

    public void CreateHighscore(string name, int score)
    {
        StartCoroutine(NewHighscore(name, score));
    }

    private IEnumerator NewHighscore(string name, int score)
    {
        print("Creating highscore");

        string url = highscoresUrl + "?game=" + gameName + "&command=create&name=" + name + "&score=" + score;
        UnityWebRequest uwr = UnityWebRequest.Get(url);

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
            Debug.Log("Error While Sending: " + uwr.error);
        else
            Debug.Log("Received: " + uwr.downloadHandler.text);
    }

    private IEnumerator LoadHighscores(int amount)
    {
        print("Loading highscores");

        string url = highscoresUrl + "?game=" + gameName + "&command=get&amount=" + amount;
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
            Debug.Log("Error While Sending: " + uwr.error);
        else
        {
            string result = uwr.downloadHandler.text;
            string[] results = result.Split('&');
            List<SingleNameScore> newHighscores = new List<SingleNameScore>();

            for (int i = 0; i < results.Length - 1; i = i + 2)
            {
                SingleNameScore newHighscore = new SingleNameScore();
                newHighscore.name = results[i];
                newHighscore.score = int.Parse(results[i + 1]);

                newHighscores.Add(newHighscore);
            }

            highscores = newHighscores;
            OnHighscoresReceived?.Invoke(highscores);
        }
    }
}
