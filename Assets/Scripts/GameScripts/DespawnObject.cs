using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using UnityEngine.SceneManagement;

public class DespawnObject : NetworkBehaviour
{

    public NetworkObject wallObj;

    public override void FixedUpdateNetwork()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length >= 2)
        {

            GameObject[] lobbyWalls = GameObject.FindGameObjectsWithTag("LobbyWall");
            foreach (GameObject wall in lobbyWalls)
            {
                Runner.Despawn(wallObj);
            }
        }
    }
}
