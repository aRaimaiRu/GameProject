using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class VotePlayerItem : MonoBehaviour
{
    [SerializeField] private Text _playerNameText;
    [SerializeField] private Text _statusText;
    [SerializeField] private Image _Symbol;

    private int _actorNumber;
    public int ActorNumber
    {
        get { return _actorNumber; }

    }

    [SerializeField] private Button _voteButton;
    private VotingManager _votingManager;
    [SerializeField] private Text CountVoteText;

    private void Awake()
    {
        // _voteButton = GetComponentInChildren<Button>();
        _voteButton.onClick.AddListener(OnVotePressed);
    }

    private void OnVotePressed()
    {
        _votingManager.CastVote(_actorNumber);
    }
    public void Initialize(Player player, VotingManager votingManager)
    {
        _actorNumber = player.ActorNumber;
        _playerNameText.text = player.NickName;
        _statusText.text = "Not Decided";
        _votingManager = votingManager;

    }
    public void UpdateStatus(string status)
    {
        _statusText.text = status;

    }
    public void ToggleButton(bool isInteractible)
    {
        _voteButton.interactable = isInteractible;
    }
    public void SetCountVoteText(int _votecount)
    {

        CountVoteText.text = (_votecount == 0) ? "" : _votecount.ToString();

    }
    public void ShowDead()
    {
        _playerNameText.text = _playerNameText.text + "(Dead)";
    }
    public void ShowSymbol(Sprite _image)
    {
        _Symbol.sprite = _image;
        _Symbol.gameObject.SetActive(true);

    }
}
