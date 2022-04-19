using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class VotingWindow : MonoBehaviourPunCallbacks
{
    [SerializeField] private Image VotingWindowRoleSymbol;
    bool startTimer = false;
    double timerIncrementValue;
    double startTime;
    [SerializeField] double timer = 30;
    ExitGames.Client.Photon.Hashtable CustomeValue;
    public Slider timeSlider;
    public VotingManager _votingManager;
    public Text timeText;


    public override void OnEnable()
    {
        base.OnEnable();
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.Time;
            CustomeValue.Add("StartTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
        }
        ShowRoleSymbol();
        // else
        // {
        //     startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
        //     startTimer = true;
        // }
    }

    private void ShowRoleSymbol()
    {
        VotingWindowRoleSymbol.sprite = VotingManager.Instance.RoleSymbolImg.sprite;

    }

    void Update()
    {
        if (!startTimer) return;
        timerIncrementValue = PhotonNetwork.Time - startTime;
        timeSlider.value = (float)((timer - timerIncrementValue) / timer);
        timeText.text = "Time: " + ((int)(timer - timerIncrementValue)).ToString();
        if (timerIncrementValue >= timer)
        {
            //Timer Completed
            //Do What Ever You What to Do Here
            StartCoroutine(conclude());

        }
    }
    public IEnumerator conclude()
    {
        startTimer = false;

        StartCoroutine(_votingManager.ConcludeVote());
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
    public void everyoneVoted()
    {
        // timer - (PhotonNetwork.Time - startTime) = 5
        // timer - PhotonNetwork.Time + startTime = 5
        startTime = 5 - timer + PhotonNetwork.Time;
    }
}
