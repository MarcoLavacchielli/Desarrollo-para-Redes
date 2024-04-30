using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    /*//[SerializeField] TextMeshProUGUI _timerText, _authoriryText;

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
    }*/

    [SerializeField]
    public List<PlayerModel> playerScreensList = new List<PlayerModel>();
    //NetworkLinkedList<PlayerModel> playerScreensList = new NetworkLinkedList<PlayerModel>();

    public void AddPlayerModel(PlayerModel playerModel)
    {
        if (Object.HasStateAuthority)
        {
            playerScreensList.Add(playerModel);
        }
    }

    private void Update()
    {
         listManagement();
    }

    void listManagement()
    {
        PlayerModel[] playerModelsInScene = FindObjectsOfType<PlayerModel>();
        foreach (PlayerModel playerModel in playerModelsInScene)
        {
            // Chequear si el playerModel ya está en la lista antes de agregarlo
            if (!playerScreensList.Contains(playerModel))
            {
                AddPlayerModel(playerModel);
            }
        }
    }

}