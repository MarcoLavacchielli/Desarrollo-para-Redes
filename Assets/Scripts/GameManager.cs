using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    //[SerializeField] TextMeshProUGUI _timerText, _authoriryText;

    [Networked] private float Timer { get; set; }
    private bool gameStarted = false;

    public override void Spawned()
    {
        Debug.Log(Object.HasStateAuthority);
    }

    public override void FixedUpdateNetwork()
    {
        if (!gameStarted && Object.HasStateAuthority)
        {
            CheckPlayers();
            Timer += Runner.DeltaTime;
        }

        //_timerText.text = $"Timer: {Timer}";
        //_authoriryText.text = $"Authority: {Object.HasStateAuthority}";
    }

    void CheckPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length >= 2)
        {
            gameStarted = true;

            GameObject[] lobbyWalls = GameObject.FindGameObjectsWithTag("LobbyWall");
            foreach (GameObject wall in lobbyWalls)
            {
                //Runner.Despawn(wall);
            }
        }
    }
}