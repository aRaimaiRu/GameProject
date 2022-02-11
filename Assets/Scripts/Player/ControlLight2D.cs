using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ControlLight2D : MonoBehaviourPun
{
    public float BeginOutLightRadius;
    public float OutLightRadius = 3.0f;
    public float delay = 5.0f;
    private Light2D _light2D;
    void Start()
    {
        TaskManager.Instance.onLightSabotage += DecreaseLightRadius;
        this._light2D = GetComponent<Light2D>();
        BeginOutLightRadius = _light2D.pointLightOuterRadius;
    }

    private void DecreaseLightRadius()
    {
        photonView.RPC("DecreaseLightRadiusRPC", RpcTarget.All);
    }
    [PunRPC]
    private void DecreaseLightRadiusRPC()
    {
        if (VotingManager.Instance.CheckIfPlayerIsImpostor(PhotonNetwork.LocalPlayer.ActorNumber)) { return; }
        StartCoroutine(TempDecreaseLightRadius(delay));
    }
    IEnumerator TempDecreaseLightRadius(float sec)
    {
        _light2D.pointLightOuterRadius = OutLightRadius;
        yield return new WaitForSeconds(sec);
        _light2D.pointLightOuterRadius = BeginOutLightRadius;

    }
    private void OnDestroy()
    {
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.onLightSabotage -= DecreaseLightRadius;
        }

    }
}
