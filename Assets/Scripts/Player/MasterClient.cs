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
        Spyware
    }
    private List<Role> VirusRoleList = new List<Role>() { Role.Worm, Role.Spyware };
    private List<Role> AntiVirusRoleList = new List<Role>() { Role.Scanner, Role.Deleter };

    // we want more control that who is Initialize so use custom Initialize instead Awake
    public void Initialize()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(PickImpostor());

        }
    }

    private IEnumerator PickImpostor()
    {
        GameObject[] players;
        List<int> playerIndex = new List<int>();
        int tries = 0;
        int impostorNumber = 0;
        int impostorNumberFinal = 0;
        // Get all the playerss in the game
        do
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            tries++;
            yield return new WaitForSeconds(0.25f);
        } while (players.Length < PhotonNetwork.CurrentRoom.PlayerCount);
        // init player index list
        for (int i = 0; i < players.Length; i++) { playerIndex.Add(i); }
        // decide imposter number
        impostorNumber = players.Length < 5 ? 1 : 2;
        impostorNumberFinal = impostorNumber;
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


        // photonView.RPC("ImpostorPicked", RpcTarget.All, impostorNumberFinal);

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
