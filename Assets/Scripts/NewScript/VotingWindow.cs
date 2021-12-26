using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class VotingWindow : MonoBehaviourPunCallbacks
{
    bool startTimer = false;
    double timerIncrementValue;
    double startTime;
    [SerializeField] double timer = 30;
    ExitGames.Client.Photon.Hashtable CustomeValue;
    public Slider timeSlider;
    public VotingManager _votingManager;

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.Time;
            startTimer = true;
            CustomeValue.Add("StartTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
        }
        // else
        // {
        //     startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
        //     startTimer = true;
        // }
    }

    void Update()
    {
        if (!startTimer) return;
        timerIncrementValue = PhotonNetwork.Time - startTime;
        timeSlider.value = (float)((timer - timerIncrementValue) / timer);
        if (timerIncrementValue >= timer)
        {
            //Timer Completed
            //Do What Ever You What to Do Here
            StartCoroutine(conclude());

        }
    }
    public IEnumerator conclude()
    {
        StartCoroutine(_votingManager.ConcludeVote());
        PlayerDeadBody[] _playerDeadBodys = FindObjectsOfType<PlayerDeadBody>();
        foreach (PlayerDeadBody deadbody in _playerDeadBodys)
        {
            Destroy(deadbody);

        }
        yield return new WaitForSeconds(3.0f);
        this.gameObject.SetActive(false);


    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("StartTime"))
        {
            if (startTimer == false)
            {
                // bool.Parse(propertiesThatChanged["StartTime"].ToString());
                startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
                startTimer = true;

            }
        }
    }
}
