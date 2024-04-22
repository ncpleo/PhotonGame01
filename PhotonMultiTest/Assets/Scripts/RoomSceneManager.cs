using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using Photon.Realtime;
using TMPro;

public class RoomSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    TMP_Text textRoomName;
    [SerializeField]
    TMP_Text textPlayerList;
    [SerializeField]
    Button buttonStartGame;

    void Start()
    {
        if(PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            textRoomName.text += PhotonNetwork.CurrentRoom.Name;
            UpdatePlayerList();
        }
        //start button can only pressed by the room owner
        buttonStartGame.interactable = PhotonNetwork.IsMasterClient;
    }

    //check if the player is the room holder
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        buttonStartGame.interactable = PhotonNetwork.IsMasterClient;
    }

    public void UpdatePlayerList()
    {
        StringBuilder sb = new StringBuilder();
        foreach(var kvp in PhotonNetwork.CurrentRoom.Players)
        {
            sb.AppendLine(">> "+kvp.Value.NickName);
        }
        textPlayerList.text = sb.ToString();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public void OnClickStartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
