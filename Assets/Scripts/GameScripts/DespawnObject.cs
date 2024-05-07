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
    private NetworkBool countingDown = false;
    private float countdownTimer = 3f;
    public TMP_Text countdownText; // Referencia al objeto de texto TMP

    private NetworkBool objDestroyed = false;

    private void Update()
    {
        if (countingDown && !objDestroyed)
        {
            countdownTimer -= Time.deltaTime;
            int secondsLeft = Mathf.CeilToInt(countdownTimer);
            countdownText.text = secondsLeft.ToString(); // Actualiza el texto con el valor del contador

            if (countdownTimer <= 0)
            {
                countdownText.text = ""; // Vacía el texto cuando el contador llega a cero
                countingDown = false;
                DespawnWalls();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside++;

            if (playersInside >= 2)
            {
                countingDown = true; // Comienza la cuenta regresiva cuando hay al menos 2 jugadores dentro
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside--;

            if (playersInside < 2)
            {
                countdownTimer = 3f;
                countingDown = false; // Detiene la cuenta regresiva si uno o ambos jugadores salen del área
                countdownText.text = ""; // Asegúrate de que el texto se vacíe si los jugadores no están dentro
            }
        }
    }

    private void DespawnWalls()
    {
        GameObject[] lobbyWalls = GameObject.FindGameObjectsWithTag("LobbyWall");
        foreach (GameObject wall in lobbyWalls)
        {
            objDestroyed = true;
            Runner.Despawn(wallObj);
        }
    }
}
