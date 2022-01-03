using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Killable : Photon.Pun.MonoBehaviourPun
{
    public bool IsImpostor = false;
    [SerializeField] private float _range = 10.0f;
    private LineRenderer _lineRenderer;
    private Killable _target;
    // Start is called before the first frame update
    private void Awake()
    {
        if (!photonView.IsMine) { return; }
        _lineRenderer = GetComponent<LineRenderer>();
        // StartCoroutine(SearchForKillable());
    }
    private void Start()
    {
        if (!photonView.IsMine) { return; }
        // UIControl.Instance.CurrentPlayer = this;

    }
    private void Update()
    {
        // if (!photonView.IsMine) { return; }
        // if (_target != null && IsImpostor)
        // {
        //     _lineRenderer.SetPosition(0, transform.position);
        //     _lineRenderer.SetPosition(1, _target.transform.position);
        // }
        // else
        // {
        //     _lineRenderer.SetPosition(0, Vector3.zero);
        //     _lineRenderer.SetPosition(1, Vector3.zero);
        // }

    }
    // private IEnumerator SearchForKillable()
    // {
    //     while (true)
    //     {
    //         Killable newTarget = null;
    //         float minDist = Mathf.Infinity;
    //         Killable[] killList = FindObjectsOfType<Killable>();

    //         foreach (Killable kill in killList)
    //         {
    //             if (kill == this) { continue; }
    //             float distance = Vector3.Distance(transform.position, kill.transform.position);
    //             if (distance > _range) { continue; }
    //             if (distance < minDist)
    //             {

    //                 newTarget = kill;
    //                 minDist = distance;

    //                 // kill
    //                 UIControl.Instance.HasTarget = _target != null;
    //                 // break;
    //             }



    //         }
    //         if (minDist > _range)
    //         {
    //             UIControl.Instance.HasTarget = false;

    //         }
    //         _target = newTarget;
    //         yield return new WaitForSeconds(0.25f);
    //     }
    // }
    public void Kill()
    {
        // if (_target == null) { return; }
        // PhotonView pv = _target.GetComponent<PhotonView>();
        // pv.RPC("KillRPC", RpcTarget.Others, pv.ViewID);
        photonView.RPC("KillRPC", RpcTarget.All);

    }

    [PunRPC]
    public void KillRPC()
    {
        // // if (photonView.ViewID != ViewID) { return; }
        if (!photonView.IsMine) { return; }
        PlayerDeadBody playerBody = PhotonNetwork.Instantiate("PlayerBody", this.transform.position, Quaternion.identity).GetComponent<PlayerDeadBody>();
        Playerinfo playerinfo = GetComponent<Playerinfo>();
        // 
        playerBody.SetColor(playerinfo._allPlayerColors[playerinfo.colorIndex]);

        // // respawn for test
        // // transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
        // PhotonNetwork.Destroy(this.gameObject);
        // // PhotonNetwork.Destroy(photonView);
        // // PhotonNetwork.Disconnect();
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Network>().DestroyPlayer();

        UIControl.Instance.OnThisPlayerKilled();
    }
    // [PunRPC]
    // public void SetImpostor()
    // {
    //     IsImpostor = true;
    //     Debug.Log("impostor here");

    // }
}
