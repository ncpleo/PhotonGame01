using UnityEngine;
using Photon.Pun;
using System;

public class PlayerConcroller : MonoBehaviour
{
    private Transform _transform;
    private PhotonView _view;
    private Rigidbody2D _rb;
    public float speed = 3f;
    public float jumpForce = 7f;
    public float bulletForce = 100;
    public int hp = 100;
    
    void Start()
    {
        _transform = this.transform;
        _view = this.gameObject.GetComponent<PhotonView>();
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        hp = 100;

        if (!_view.IsMine)
        {
            Destroy(_rb);  //prevent duplicated calculation on the rigidbody force
        }
        
    }

    void Update()
    {
        if (_view.IsMine)  //one control for one local player
        {
            Control();
        }
    }

    void Control()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            _transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Vector3 offset = new Vector3(0.3f, 0, 0);  //prevent hitting player
            GameObject bulletObject =  PhotonNetwork.Instantiate("Bullet", _transform.position + offset, Quaternion.identity);
            Rigidbody2D bulletRb = bulletObject.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(new Vector2(bulletForce, 0));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //prevent multiple triggers on one bullet
        if(_view.IsMine)
        {
            if(other.gameObject.tag == "Bullet")
            {
                BulletController bullet = other.gameObject.GetComponent<BulletController>();
                if (!bullet.view.IsMine)
                {
                    hp -= 10;
                    if(hp<=0)
                    {
                        PhotonNetwork.Destroy(this.gameObject);
                    }
                }
            }
        }
    }
}
