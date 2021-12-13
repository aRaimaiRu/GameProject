using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerUIItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text PlayerName;
    [SerializeField] private Color PlayerColor;
    [SerializeField] private Button leftArrowBtn;
    [SerializeField] private Button rightArrowBtn;
    [SerializeField] private Image showColorImg;
    ExitGames.Client.Photon.Hashtable playerproperties = new ExitGames.Client.Photon.Hashtable();
    public void SetName(string _name)
    {
        PlayerName.text = _name;
    }
    public void SetColor(Color _color)
    {
        showColorImg.color = _color;
        // PlayerColor = _color;
        // playerproperties["color"] = _color;
        // playerproperties["testColor"] = _color.ToString();

        // PhotonNetwork.SetPlayerCustomProperties(playerproperties);


    }
    private void OnLeftArrowPressed()
    {
        // PhotonNetwork.SetPlayerCustomProperties(playerproperties);
        Debug.Log(playerproperties["ColorIndex"]);
    }
}
