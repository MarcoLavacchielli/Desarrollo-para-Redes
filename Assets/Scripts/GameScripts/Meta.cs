using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meta : NetworkBehaviour
{
    [SerializeField] GameObject victoryScreen, defeatScreen;
    private bool victoryDeclared = false;

    private void OnTriggerEnter(Collider other)
    {
        if (victoryDeclared || !Object.HasStateAuthority)
            return;

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerModel>().enabled = false;

            if (!victoryDeclared)
            {
                victoryScreen.SetActive(true);
                victoryDeclared = true;
            }
            else
            {
                defeatScreen.SetActive(true);
            }
        }
    }
}
