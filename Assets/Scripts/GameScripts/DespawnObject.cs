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
    private int playersInside = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside++;

            if (playersInside >= 2)
            {
                GameObject[] lobbyWalls = GameObject.FindGameObjectsWithTag("LobbyWall");
                foreach (GameObject wall in lobbyWalls)
                {
                    Runner.Despawn(wallObj);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside--;
        }
    }
}