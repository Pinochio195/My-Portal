using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Lean.Pool;
using Ring;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

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
    public Player_CheckGround playerCheckGround;

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Spine For Player")]
    public Player_Spine _playerSpine;

    [Space(10)] [HeaderTextColor(0.2f, 1, 1, headerText = "Fire For Player")]
    public Player_Fire _playerFire;

    private void Start()
    {
        _playerSpine._bone = _playerComponent._skeletonAnimation.Skeleton.FindBone(_playerSpine._boneName);
        _playerComponent._skeletonAnimation.AnimationState.Event += FireBall;
    }

    private void Update()
    {
        if (CheckUIReturn()) return;
        DirectionFire();

        if (Input.GetMouseButtonDown(1))
        {
            _playerSpine._bone.SetToSetupPose();
        }
        Debug.DrawLine((Vector2)_playerFire._firePosition.position, Input.mousePosition, Color.red);
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private static bool CheckUIReturn()
    {
        #region Kiểm tra xem có nhấn va UI nào không , nếu không thì return

#if UNITY_EDITOR || UNITY_STANDALONE
            if (EventSystem.current.IsPointerOverGameObject())
            {
                GameObject selectedObj = EventSystem.current.currentSelectedGameObject;
                if (selectedObj != null)
                {
                    return true;
                }
            }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                GameObject selectedObj = EventSystem.current.currentSelectedGameObject;
                if (selectedObj != null)
                {
                    return true;
                }
            }
        }
#endif

        #endregion

        return false;
    }

    private void DirectionFire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            DirectionFace(mousePosition);
        }
    }

    private void DirectionFace(Vector2 mousePosition)
    {
        _playerFire._positionBone = _playerComponent._skeletonAnimation.transform.InverseTransformPoint(mousePosition);
        if ((_playerFire._positionBone.x < 0 && _playerComponent._skeletonAnimation.skeleton.ScaleX == -1f) ||
            (_playerFire._positionBone.x > 0 && _playerComponent._skeletonAnimation.skeleton.ScaleX == 1f))
        {
            _playerComponent._skeletonAnimation.skeleton.ScaleX *= -1f;
            _playerFire._positionBone.x *= -1f;
        }
        else
        {
            _playerSpine._bone.SetToSetupPose();
        }

        _playerSpine._bone.SetPositionSkeletonSpace(_playerFire._positionBone.normalized);
        //_playerComponent._skeletonAnimation.state.SetAnimation(0, "Shoot", false);
        Vector2 fireDirection = ((Vector2)_playerFire._firePosition.position - (Vector2)mousePosition).normalized;
        _playerFire._ballFireGameObject = LeanPool.Spawn(_playerFire._prefabs_Blue_BallFire,
            _playerFire._firePosition.position, Quaternion.identity);
        _playerFire._ballFireGameObject.GetComponent<Rigidbody2D>().velocity =
            fireDirection * _playerFire._speedBall;
    }

    private void FireBall(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == _playerFire._fireEvent)
        {
            Fire();
        }
    }

    private void Fire()
    {
        Vector2 fireDirection = (_playerFire._positionBone - (Vector2)_playerFire._firePosition.transform.position).normalized;
        _playerFire._ballFireGameObject = LeanPool.Spawn(_playerFire._prefabs_Blue_BallFire,
            _playerFire._firePosition.position, Quaternion.identity);
        _playerFire._ballFireGameObject.GetComponent<Rigidbody2D>().velocity =
            fireDirection * _playerFire._speedBall;
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
    }

    public void OnTriggerExit2D(Collider2D other)
    {
    }
}