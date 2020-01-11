using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractPopUp : Readable
{

    new protected void Start()
    {
        base.Start();

        this.OpenSound = (AudioClip)Resources.Load("Audio/contract_open");
        this.CloseSound = (AudioClip)Resources.Load("Audio/contract_close");
    }
}
