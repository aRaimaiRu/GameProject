using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerDeadBodyReport : MonoBehaviourPun
{
    private UIControl _uiControl;
    private VotingManager _votingManager;
    public void Initialize(UIControl uIControl, VotingManager votingManager)
    {
        _uiControl = uIControl;
        _votingManager = votingManager;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_votingManager == null) { return; }
        if (collision.gameObject.GetComponent<PlayerDeadBody>() == null) { return; }
        if (_votingManager.WasBodyReported(photonView.OwnerActorNr) == false)
        {
            Debug.Log("trigger has dead body");
            _uiControl.HasDeadBodyInRange = true;
            _votingManager.DeadBodyInProximity = collision.gameObject.GetComponent<PhotonView>();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_votingManager == null) { return; }
        if (_votingManager.DeadBodyInProximity == collision.gameObject.GetComponent<PhotonView>())
        {
            Debug.Log("trigger has no dead body");

            _uiControl.HasDeadBodyInRange = false;
            _votingManager.DeadBodyInProximity = null;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_votingManager == null) { return; }
        PhotonView targetPhotonView = collision.gameObject.GetComponent<PhotonView>();
        if (targetPhotonView == null) { return; }
        if (_votingManager.WasBodyReported(targetPhotonView.OwnerActorNr))
        {
            if (_votingManager.DeadBodyInProximity == targetPhotonView)
            {
                _uiControl.HasDeadBodyInRange = false;
            }

        }


    }
}
