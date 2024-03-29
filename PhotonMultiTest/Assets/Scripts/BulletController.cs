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
        timer = 0.7f;
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
                removeBullet();
            }
        }
    }

    public void removeBullet()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}
