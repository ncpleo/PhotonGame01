using UnityEngine;
using Photon.Pun;
using System;
using HashTable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerConcroller : MonoBehaviourPunCallbacks
{
    private Transform _transform;
    private PhotonView _view;
    private Rigidbody2D _rb;
    private SpriteRenderer _spr;

    public float speed = 4f;
    public float jumpForce = 7.5f;
    public float bulletForce = 300;
    public int hp;
    public int jumpAbility;

    [SerializeField]
    private Image hpImage;
    [SerializeField]
    private Text nameText;

    GameSceneManager _gm;

    void Start()
    {
        _transform = this.transform;
        _view = this.gameObject.GetComponent<PhotonView>();
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        _spr = this.gameObject.GetComponent<SpriteRenderer>();
        _gm = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();     //search all gameobject to find gm

        hp = 100;
        jumpAbility = 2;

        if (!_view.IsMine)
        {
            Destroy(_rb);  //prevent duplicated calculation on the rigidbody force
        }
        nameText.text = _view.Owner.NickName;
    }

    void Update()
    {
        if (_view.IsMine)  //one control for one local player
        {
            Control();
            if(_transform.position.y < -10)
            {
                Dead();
            }
        }
    }

    void Control()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _transform.position += Vector3.left * speed * Time.deltaTime;
            _spr.flipX = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _transform.position += Vector3.right * speed * Time.deltaTime;
            _spr.flipX = false;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if(jumpAbility > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                jumpAbility--;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float force = _spr.flipX ? -bulletForce : bulletForce;  //check player facing direction
            float _offset = _spr.flipX ? -0.2f : 0.2f;
            Vector3 offset = new Vector3(_offset, 0, 0);  //prevent hitting player
            GameObject bulletObject = PhotonNetwork.Instantiate("Bullet", _transform.position + offset, Quaternion.identity);
            Rigidbody2D bulletRb = bulletObject.GetComponent<Rigidbody2D>();
            SpriteRenderer spr = bulletObject.GetComponent<SpriteRenderer>();
            spr.flipX = _offset < 0;    //change the facing direction of the bullet
            bulletRb.AddForce(new Vector2(force, 0));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //prevent multiple triggers on one bullet
        if (_view != null && _view.IsMine)
        {
            if (other.gameObject.tag == "Bullet")
            {
                BulletController bullet = other.gameObject.GetComponent<BulletController>();
                if (!bullet.view.IsMine)
                {
                    HashTable table = new HashTable();
                    hp -= 10;
                    table.Add("hp", hp);
                    _gm.CallRpcSendMessageToAll(bullet.view.Owner.NickName + " shot " + _view.Owner.NickName);
                    //bullet.removeBullet();
                    PhotonNetwork.LocalPlayer.SetCustomProperties(table);
                    if (hp <= 0)
                    {
                        Dead();
                    }
                }
            }
            if(other.gameObject.tag == "Ground")
            {
                jumpAbility = 2;
            }
        }
    }

    public void Dead()
    {
        PhotonNetwork.Destroy(this.gameObject);
        //RPC call masterClient that player is killed
        _gm.CallRpcPlayerDead();
    }

    public void UpdateHpBar()
    {
        float percent = (float)hp / 100;
        hpImage.transform.localScale = new Vector3(percent, hpImage.transform.localScale.y, hpImage.transform.localScale.z);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, HashTable changedProps)
    {
        if(targetPlayer == _view.Owner)
        {
            hp = (int)changedProps["hp"];
            print(targetPlayer.NickName + ":" + hp.ToString());
            UpdateHpBar();
        }
    }
}
