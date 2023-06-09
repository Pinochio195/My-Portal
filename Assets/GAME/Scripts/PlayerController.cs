using System;
using System.Collections;
using System.Collections.Generic;
using Ring;
using UnityEngine;

public class PlayerController : BasePlayerController
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

}
