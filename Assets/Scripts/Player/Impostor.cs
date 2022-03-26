using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;




public class Impostor : Role
{
    [SerializeField] private float _range = 10.0f; // ระยะในการกำจัดผู้เล่น
    protected Killable _target; // เป้าหมาย
    public float KillCoolDown = 10f; // Cooldown
    public override void Start()
    {
        base.Start();
        if (!photonView.IsMine) { return; } // ทำงานเฉพาะเครื่องที่เป็นเจ้าของ
        UIControl.Instance.IsImpostor = true; //ตั้ง UI เป็นของ Impostor
        UIControl.Instance.ImpostorBtn(); //ตั้ง UI เป็นของ Impostor
        StartCoroutine(SearchForKillable()); //เรียกใช้งาน ฟังก์ชั่น SearchForKillable
    }
    private IEnumerator SearchForKillable()
    {
        while (true)
        {
            Killable newTarget = null; // เป้าหมายใหม่
            float minDist = Mathf.Infinity; // infinity
            Killable[] killList = FindObjectsOfType<Killable>(); // list of all Killable

            foreach (Killable kill in killList) // loop all Killable
            {
                if (kill == this.GetComponent<Killable>()) { continue; } // check if killable == self
                float distance = Vector3.Distance(transform.position, kill.transform.position);
                // คำนวณระยะทางของตัวละครกับเป้าหมาย Killable
                if (distance > _range) { continue; } // Skip ถ้า ระยะทางเกิน ระยะที่กำหนด
                if (distance < minDist) // check if ระยะทาง น้อยกว่า ระยะทางที่น้อยที่สุด
                {
                    newTarget = kill; //ตั้งเป้าหมายใหม่
                    minDist = distance; // แทนที่ระยะทางที่น้อยที่สุด
                    UIControl.Instance.HasTarget = _target != null;
                    //บอก UI ว่ามีเป้าหมาย
                }

            }
            if (minDist > _range || minDist == Mathf.Infinity) // check if ระยะทางของเป้าหมายเกิน ระยะที่กำหนด หรือ Infinity
            {
                UIControl.Instance.HasTarget = false; //บอก UI ว่าไม่มีเป้าหมาย

            }
            _target = newTarget; ////ตั้งเป้าหมาย
            yield return new WaitForSeconds(0.25f); // รอ 0.25 วินาที
        }
    }

    public override void GamePlayAction()
    {
        UIControl.Instance._killBtn.onClick.AddListener(delegate
        {
            photonView.RPC("TeleportRPC", RpcTarget.All, this._target.gameObject.transform.position);
            //teleport ตัวไปยังตำแหน่งของเป้าหมาย
            this._target.Kill(); // สั่งให้เป้าหมายทำลายตัวเอง
        }); // ใส่ function เมื่อกดปุ่ม ฆ่า 
    }
    [PunRPC]
    public void TeleportRPC(Vector3 _position)
    {
        this.gameObject.transform.position = _position;//teleport ตัวไปยังตำแหน่งของเป้าหมาย
    }
}



