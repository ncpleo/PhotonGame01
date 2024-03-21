using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Realtime;
using System.Text;

public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    InputField inputRoomName;
    [SerializeField]
    InputField inputPlayerName;
    [SerializeField]
    Text textRoomList;
    

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("StartScene");
        }
        else
        {
            if(PhotonNetwork.CurrentLobby == null)
            {
                PhotonNetwork.JoinLobby();
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("Joined");
    }

    public string GetRoomName()
    {
        string roomName = inputRoomName.text;
        return roomName.Trim();
    }
        public string GetPlayerName()
    {
        string playerName = inputPlayerName.text;
        return playerName.Trim();
    }

    public void OnClickCreateRoom()
    {
        string roomName = GetRoomName();
        string playerName = GetPlayerName();
        if (roomName.Length > 0 && playerName.Length > 0)
        {
            PhotonNetwork.CreateRoom(roomName);
            PhotonNetwork.LocalPlayer.NickName = playerName;
        }
        else
        {
            print("Invalid Room Name or Player Name!");
        }
    }

    public void OnClickJoinRoom()
    {
        string roomName = GetRoomName();
        string playerName = GetPlayerName();
        if (roomName.Length > 0 && playerName.Length > 0)
        {
            PhotonNetwork.JoinRoom(roomName);
            PhotonNetwork.LocalPlayer.NickName = playerName;
        }
        else
        {
            print("Invalid Room Name or Player Name!");
        }
    }

    public override void OnJoinedRoom()
    {
        print("Room Joined");
        SceneManager.LoadScene("RoomScene");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("update");
        StringBuilder sb = new StringBuilder();
        foreach(RoomInfo roomInfo in roomList)
        {
            print(roomInfo);
            if(roomInfo.PlayerCount > 0)
            {
                sb.AppendLine(">> " + roomInfo.Name);
            }
            
        }
        textRoomList.text = sb.ToString();
    }
}
