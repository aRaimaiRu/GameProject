using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ChatWindowUI : MonoBehaviourPun
{
    [SerializeField] private ChatItemUI _chatItemPrefab;
    [SerializeField] private Transform _context;
    [SerializeField] private InputField _inputtext;
    public Playerinfo _playerInfo;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
        }
    }
    private void OnEnable()
    {
        _inputtext.text = string.Empty;
        _inputtext.ActivateInputField();

    }
    private void SendMessage()
    {
        // check empty message
        if (string.IsNullOrEmpty(_inputtext.text)) { return; }
        if (_playerInfo == null) { return; }
        photonView.RPC("ReceiveMessageRPC", RpcTarget.All, _inputtext.text, _playerInfo.CurrentColor.r, _playerInfo.CurrentColor.g, _playerInfo.CurrentColor.b);
        // InstantiateChatItem(_inputtext.text, Color.red);
        _inputtext.text = string.Empty;
        _inputtext.ActivateInputField();
    }
    public void OnSendButtonPressed()
    {
        SendMessage();
    }
    private void InstantiateChatItem(string text, Color color)
    {
        ChatItemUI newChatItem = Instantiate(_chatItemPrefab);
        newChatItem.transform.SetParent(_context);
        newChatItem.transform.position = Vector3.zero;
        newChatItem.transform.localScale = Vector3.one;

        newChatItem.Initialize(text, color);

    }
    [PunRPC]
    public void ReceiveMessageRPC(string text, float red, float green, float blue)
    {
        InstantiateChatItem(text, new Color(red, green, blue));
    }
}
