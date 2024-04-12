using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviourPunCallbacks
{
    public void OnClickStart()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        print("Started");
    }

    public override void OnConnectedToMaster()
    {
        print("Connected");
        SceneManager.LoadScene("LobbyScene");
    }

    public void OnClickOption()
    {
        
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}
