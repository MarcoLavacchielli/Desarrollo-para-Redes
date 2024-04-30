using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meta : NetworkBehaviour
{
    [SerializeField]
    private List<PlayerEndScreens> playerScreensList = new List<PlayerEndScreens>();
    [SerializeField]
    private PlayerEndScreens firstPlayerModel;

    [SerializeField] private NetworkBool UnoGano;
    [SerializeField] private NetworkBool DosGano;

    [SerializeField] GameObject victoryScreen, defeatScreen;

    [SerializeField] private NetworkBool termino = false;


    public void AddPlayerModel(PlayerEndScreens playerModel)
    {
        playerScreensList.Add(playerModel);
    }

    private void Update()
    {

        if (termino == false)
        {
            listManagement();
        }
        if (termino == true)
        {
            playerScreensList = null;
        }

    }

    void listManagement()
    {
        PlayerEndScreens[] playerModelsInScene = FindObjectsOfType<PlayerEndScreens>();
        foreach (PlayerEndScreens playerModel in playerModelsInScene)
        {
            // Chequear si el playerModel ya está en la lista antes de agregarlo
            if (!playerScreensList.Contains(playerModel))
            {
                AddPlayerModel(playerModel);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        firstPlayerModel = playerScreensList[0];

        if (other.GetComponent<PlayerEndScreens>() == firstPlayerModel)
        {
            UnoGano = true;
            DosGano = false;
            RpcDeclareVictory();
        }
        else if (other.GetComponent<PlayerEndScreens>() == playerScreensList[1])
        {
            UnoGano = false;
            DosGano = true;
            RpcDeclareVictory();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RpcDeclareVictory()
    {
        if (UnoGano)
        {
            //playerScreensList[0].defeatScreen.SetActive(true);
            //playerScreensList[1].victoryScreen.SetActive(true);
            playerScreensList[1].gano();
            playerScreensList[0].perdio();
            termino = true;
        }
        else if (DosGano)
        {
            //playerScreensList[1].defeatScreen.SetActive(true);
            //playerScreensList[0].victoryScreen.SetActive(true);
            playerScreensList[0].gano();
            playerScreensList[1].perdio();
            termino = true;
        }
    }

}