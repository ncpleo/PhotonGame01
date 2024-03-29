using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Realtime;
using System.Collections;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    List<string> messageList;
    [SerializeField]
    Text messageText;

    PhotonView _pv;

    public Dictionary<Player, bool> alivePlayerMap = new Dictionary<Player, bool>();   //record current game state

    void Start()
    {
        _pv = this.gameObject.GetComponent<PhotonView>();
        if(PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            StartCoroutine(DelayInit(1));            //delay at init, prevent sync problem
        }
    }

    IEnumerator DelayInit(float sec)
    {
        yield return new WaitForSeconds(sec);
        InitGame();
    }

    public void InitGame()
    {
        foreach(var kvp in PhotonNetwork.CurrentRoom.Players)        //store all player to the dict map
        {
            alivePlayerMap[kvp.Value] = true;
        }

        float spwanPointX = Random.Range(-3, 3);
        float spwanPointY = 2;
        PhotonNetwork.Instantiate("Player", new Vector3(spwanPointX, spwanPointY, 0), Quaternion.identity);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        alivePlayerMap[newPlayer] = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (alivePlayerMap.ContainsKey(otherPlayer))          //check player existence
        {
            alivePlayerMap.Remove(otherPlayer);
        }
        if(PhotonNetwork.CurrentRoom.PlayerCount <= 1)        //kick player when <=1 player in room
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    void Update()
    {
        
    }

    public void CallRpcPlayerDead()
    {
        _pv.RPC("RpcPlayerDead", RpcTarget.All);      //rpc info sync
    }

    [PunRPC]
    void RpcPlayerDead(PhotonMessageInfo info)
    {
        if (alivePlayerMap.ContainsKey(info.Sender))
        {
            alivePlayerMap[info.Sender] = false;
        }

        if(PhotonNetwork.IsMasterClient && CheckGameOver())
        {
            //check player status, result display/scene change
            //SceneManager.LoadScene("GameScene 2");
            CallRpcReloadGame();
        }
    }

    bool CheckGameOver()
    {
        int aliveCount = 0;
        foreach(var kvp in alivePlayerMap)        
        {
            if(kvp.Value) aliveCount++;            //count each exist players
        }
        return aliveCount <= 1;                    //true: end game
    }

    void CallRpcReloadGame()
    {
        _pv.RPC("ReloadGame", RpcTarget.All);
    }

    [PunRPC]
    void ReloadGame(PhotonMessageInfo info)
    {
        //SceneManager.LoadScene("GameScene");
        PhotonNetwork.LoadLevel("GameScene");
    }

    public void CallRpcSendMessageToAll(string message)
    {
        _pv.RPC("RpcSendMessage", RpcTarget.All, message);   //send message to everyone
    }

    [PunRPC]
    void RpcSendMessage(string message, PhotonMessageInfo info) //PhotonMessageInfo: telling who send the message
    {
        if(messageList.Count >= 10)       //check the message list size
        {
            messageList.RemoveAt(0);     //remove the oldest message to keep 10 messages only on the message box
        }
        messageList.Add(message);
        UpdateMessage();                //output to the canvas
    }

    void UpdateMessage()
    {
        messageText.text = string.Join("\n", messageList);
    }

    
}
