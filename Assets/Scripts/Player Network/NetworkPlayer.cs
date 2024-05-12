using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }

    public override void Spawned()
    {
        if (Object.HasInputAuthority) Local = this;
    }
}
