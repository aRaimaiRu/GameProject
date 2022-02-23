using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Door : MonoBehaviourPun
{
    private float SourceX;
    private float SourceY;
    public float DestinationX;
    public float DestinationY;
    public int id;
    public float delayClose = 5.0f;
    private void Start()
    {
        SourceX = transform.localPosition.x;
        SourceY = transform.localPosition.y;
        this.gameObject.SetActive(false);
        TaskManager.Instance.onDoorSabotage += ReceiveDoorSabotage;
    }

    private void ReceiveDoorSabotage(int _id)
    {
        if (id != _id) { return; }
        photonView.RPC("ClosingDoorRPC", RpcTarget.All);


    }

    [PunRPC]
    public void ClosingDoorRPC()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(ClosingDoor());
    }
    IEnumerator ClosingDoor()
    {

        if (DestinationX != 0)
        {
            LeanTween.moveLocalX(gameObject, DestinationX, 1f);
            yield return new WaitForSeconds(delayClose);
            LeanTween.moveLocalX(gameObject, SourceX, 1f);


        }
        else if (DestinationY != 0)
        {
            LeanTween.moveLocalY(gameObject, DestinationY, 1f);
            yield return new WaitForSeconds(delayClose);
            LeanTween.moveLocalY(gameObject, SourceY, 1f);


        }
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);


    }
    private void OnDestroy()
    {
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.onDoorSabotage -= ReceiveDoorSabotage;
        }

    }
}
