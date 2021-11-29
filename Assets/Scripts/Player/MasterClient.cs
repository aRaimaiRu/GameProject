using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MasterClient : MonoBehaviour
{
    [SerializeField] private GameObject _impostorWindow;
    [SerializeField] private GameObject _impostorText;
    // we want more control that who is Initialize so use custom Initialize instead Awake
    public void Initialize()
    {
        StartCoroutine(PickImpostor());
    }

    private IEnumerator PickImpostor()
    {
        GameObject[] players;
        List<int> playerIndex = new List<int>();
        int tries = 0;
        int impostorNumber = 0;
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
        // Assign the imposter
        while (impostorNumber > 0)
        {
            int pickedImpostorIndex = playerIndex[Random.Range(0, playerIndex.Count)];
            playerIndex.Remove(pickedImpostorIndex);
            PhotonView pv = players[pickedImpostorIndex].GetComponent<PhotonView>();
            pv.RPC("SetImpostor", RpcTarget.All);
            impostorNumber--;
        }

        yield return null;

    }
}
