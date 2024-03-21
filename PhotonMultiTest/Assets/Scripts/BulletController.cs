using UnityEngine;
using Photon.Pun;
using System.Threading;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float timer;
    public PhotonView view;

    void Start()
    {
        timer = 1;
        view = this.gameObject.GetComponent<PhotonView>();
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        if(!view.IsMine)
        {
            Destroy(_rb);
        }
    }
    void Update()
    {
        if(view.IsMine)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}
