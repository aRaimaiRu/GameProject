using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MasterClient : MonoBehaviourPun
{
    [SerializeField] private GameObject _impostorWindow;
    [SerializeField] private Text _impostorText;
    public enum Role
    {
        Process,
        Scanner,
        Deleter,
        Worm,
        Spyware,
        Imposter
    }
    public static List<Role> VirusRoleList = new List<Role>() {
        Role.Worm,
        Role.Spyware
        };
    public static List<Role> AntiVirusRoleList = new List<Role>() { Role.Scanner, Role.Deleter };
    private GameObject[] players;
    private int _impostorCount;
    private int _antiVirusCount;
    public int ImpostorCount
    {
        get { return _impostorCount; }
        set { _impostorCount = value; }
    }
    public int AntiVirusCount
    {
        get { return _antiVirusCount; }
        set { _antiVirusCount = value; }
    }
    // we want more control that who is Initialize so use custom Initialize instead Awake
    public void Initialize()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        Debug.Log("Current player Count =" + PhotonNetwork.CurrentRoom.PlayerCount);
        _impostorCount = PhotonNetwork.CurrentRoom.PlayerCount < 3 ? 1 : (int)PhotonNetwork.CurrentRoom.CustomProperties["VirusNumber"];
        _antiVirusCount = PhotonNetwork.CurrentRoom.PlayerCount - _impostorCount;
        StartCoroutine(PickImpostor());

    }

    private IEnumerator PickImpostor()
    {

        List<int> playerIndex = new List<int>();
        int tries = 0;
        int impostorNumber = _impostorCount;
        // Get all the playerss in the game
        do
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            tries++;
            yield return new WaitForSeconds(0.25f);
        } while (players.Length < PhotonNetwork.CurrentRoom.PlayerCount);
        // init player index list
        for (int i = 0; i < players.Length; i++) { playerIndex.Add(i); }

        // Assign the imposter
        while (impostorNumber > 0)
        {
            // pick index
            int pickedImpostorIndex = playerIndex[Random.Range(0, playerIndex.Count)];
            // pick
            Role pickedRole = VirusRoleList[Random.Range(0, VirusRoleList.Count)];

            // set impostor
            PhotonView pv = players[pickedImpostorIndex].GetComponent<PhotonView>();
            pv.RPC("SetRole", RpcTarget.All, pickedRole);

            // remove item that already pick
            playerIndex.Remove(pickedImpostorIndex);
            VirusRoleList.Remove(pickedRole);
            impostorNumber--;


        }
        // give process A Role
        for (int i = 0; i < playerIndex.Count; i++)
        {

            PhotonView pv = players[playerIndex[i]].GetComponent<PhotonView>();
            pv.RPC("SetRole", RpcTarget.All, Role.Process);

        }


        // photonView.RPC("ImpostorPicked", RpcTarget.All, ImpostorCount);

    }
    [PunRPC]
    public void ImpostorPicked(int impostorNumber)
    {
        StartCoroutine(ShowImpostorAnimation(impostorNumber));
    }
    private IEnumerator ShowImpostorAnimation(int impostorNumber)
    {
        _impostorWindow.SetActive(true);
        _impostorText.gameObject.SetActive(true);
        _impostorText.text = "There " + impostorNumber + (impostorNumber < 2 ? "is " : "are ") + "impostor" + (impostorNumber > 1 ? "s" : string.Empty) + "among us";
        yield return new WaitForSeconds(1);
        _impostorWindow.gameObject.SetActive(false);

    }



}
