using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Playerinfo : Photon.Pun.MonoBehaviourPun,IPunObservable
{
    public int colorIndex;
    public SpriteRenderer playerBody;
    public List<Color> _allPlayerColors = new List<Color>();
    private void Awake() {
        if(photonView.IsMine){
            colorIndex = Random.Range(0,_allPlayerColors.Count-1);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info){
        if(stream.IsWriting){
            stream.SendNext(colorIndex);
            Debug.Log("Writing"+colorIndex);
        }else{
            colorIndex = (int)stream.ReceiveNext();
            Debug.Log("Not Writing"+colorIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerBody.color = _allPlayerColors[colorIndex];
    }
}
