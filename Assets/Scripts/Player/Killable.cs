using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Killable : Photon.Pun.MonoBehaviourPun
{
    public void Kill()
    {
        photonView.RPC("KillRPC", RpcTarget.All); //  เรียกคำสั่ง KillRPC ในทุกเครื่อง
    }

    [PunRPC]
    public void KillRPC()
    {
        if (!photonView.IsMine) { return; } // ถ้าไม่ใช่เจ้าของ Object จะไม่ทำงาน
        PlayerDeadBody playerBody = PhotonNetwork.Instantiate("PlayerBody", this.transform.position, Quaternion.identity).GetComponent<PlayerDeadBody>();
        //สร้าง Object PlayerBody
        Playerinfo playerinfo = GetComponent<Playerinfo>(); // เข้าถึง Script PlayerInfo
        playerBody.SetColor(playerinfo._allPlayerColors[playerinfo.colorIndex]); // นำสีจาก Script Playerinfo ใส่ให้ PlayerBody
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Network>().DestroyPlayer(); //ทำลาย Player นี้
        UIControl.Instance.OnThisPlayerKilled(); //แสดง UI ว่าโดนทำลาย
    }
}
