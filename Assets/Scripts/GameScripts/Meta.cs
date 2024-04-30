using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meta : NetworkBehaviour
{
    [SerializeField]
    private List<PlayerModel> playerScreensList = new List<PlayerModel>();
    [SerializeField]
    private PlayerModel firstPlayerModel;

    [SerializeField] private NetworkBool UnoGano;
    [SerializeField] private NetworkBool DosGano;

    //[SerializeField] GameObject victoryScreen, defeatScreen;

    [SerializeField] private NetworkBool termino = false;


    public void AddPlayerModel(PlayerModel playerModel)
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

    private void OnTriggerEnter(Collider other)
    {
        //firstPlayerModel = playerScreensList[0];

        if (other.GetComponent<PlayerModel>() == playerScreensList[0])
        {
            UnoGano = true;
            DosGano = false;
            RpcDeclareVictory();
        }
        else if (other.GetComponent<PlayerModel>() == playerScreensList[1])
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
            playerScreensList[0].Gano();
            playerScreensList[1].Perdio();
            termino = true;
        }
        else if (DosGano)
        {
            playerScreensList[1].Gano();
            playerScreensList[0].Perdio();
            termino = true;
        }
    }

}