using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerEndScreens : NetworkBehaviour
{


    [Header("Canvas")]//
    public GameObject victoryScreen;
    public GameObject defeatScreen;
    //

    public void gano()
    {
        victoryScreen.SetActive(true);
        Destroy(defeatScreen);
        //Runner.Despawn(defeatScreen);
    }

    public void perdio()
    {
        defeatScreen.SetActive(true);
        Destroy(victoryScreen);
        //Runner.Despawn(victoryScreen);
    }


}
