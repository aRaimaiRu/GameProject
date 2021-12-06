using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class VotingManager : MonoBehaviourPun
{
    public static VotingManager Instance;
    [HideInInspector] public PhotonView DeadBodyInProximity;
    private List<int> _reportedDeadBodiesList = new List<int>();

    private void Awake()
    {
        Instance = this;
    }

    public bool WasBodyReported(int actorNumber)
    {
        return _reportedDeadBodiesList.Contains(actorNumber);

    }
    public void ReportDeadBody()
    {
        if (DeadBodyInProximity == null) { return; }
        if (_reportedDeadBodiesList.Contains(DeadBodyInProximity.OwnerActorNr))
        {
            // the body was already reported do nothing
            return;
        }
        Debug.Log("DeadBodyInProximity.OwnerActorNr =" + DeadBodyInProximity.OwnerActorNr);
        photonView.RPC("ReportDeadBodyRPC", RpcTarget.All, DeadBodyInProximity.OwnerActorNr);

    }
    [PunRPC]
    public void ReportDeadBodyRPC(int actorNumber)
    {
        _reportedDeadBodiesList.Add(actorNumber);
    }


}
