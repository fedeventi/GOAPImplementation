using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour ,IObserver
{
    public Text score;
    int points;
    PlayerModel player;
    public void OnNotify(string eventName)
    {
        if (eventName == "AddPoints")
        {
            AddScore();
        }
    }

    void Start()
    {
        player=FindObjectOfType<PlayerModel>();
        player.SubEvent(this);
        
    }

    
    void Update()
    {
        UpdateScore();
        
    }
    void UpdateScore()
    {
        score.text = $"puntos : {points}";
    }
    void AddScore()
    {
        points += player.GetPoints;
    }
}
