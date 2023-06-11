using System;
using System.Collections;
using System.Collections.Generic;
using Ring;
using UnityEngine;

public class PlayerController : BasePlayerController, ICollidable
{
    #region Singleton Pattern

    private static PlayerController instance;

    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerController>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "BasePlayerController";
                    instance = obj.AddComponent<PlayerController>();
                    Debug.Log("Khởi tạo mới instance");
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    #endregion

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Jump For Player")]
    public Player_Jump _playerJump;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    #region Move Player

    protected override void PlayerMove()
    {
        base.PlayerMove();
    }

    public override void StartMovingLeft()
    {
        base.StartMovingLeft();
    }

    public override void StopMovingLeft()
    {
        base.StopMovingLeft();
    }

    public override void StartMovingRight()
    {
        base.StartMovingRight();
    }

    public override void StopMovingRight()
    {
        base.StopMovingRight();
    }

    #endregion

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (_playerComponent._rigidbody.velocity.y == 0)
            {
                Debug.Log(222);
                PortalManager.Instance._forcePlayer = 0;
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal") &&  PortalManager.Instance._forcePlayer ==0)
        {
            PortalManager.Instance._forcePlayer = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) + 10f;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
    }
}