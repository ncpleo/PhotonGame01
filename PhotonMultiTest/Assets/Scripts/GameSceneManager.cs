using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    
    void Start()
    {
        if(PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            InitGame();
        }
    }

    public void InitGame()
    {
        float spwanPointX = Random.Range(-3, 3);
        float spwanPointY = 2;
        PhotonNetwork.Instantiate("Player", new Vector3(spwanPointX, spwanPointY, 0), Quaternion.identity);
    }

    void Update()
    {
        
    }
}
