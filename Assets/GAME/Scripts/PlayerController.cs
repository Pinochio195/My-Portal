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

    [Space(5)] [HeaderTextColor(0.2f, 1, 1, headerText = "Jump For Player")]
    public Player_CheckGround playerCheckGround;

    [Space(5)] [HeaderTextColor(0.2f, 1, 1, headerText = "Spine For Player")]
    public Player_Spine _playerSpine;

    [Space(5)] [HeaderTextColor(0.2f, 1, 1, headerText = "Fire For Player")]
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
            _playerComponent._skeletonAnimation.Skeleton.SetToSetupPose();
        }
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
        if (Input.GetMouseButtonDown(0) && (Time.time - _playerFire.lastFireTime) >= _playerFire.fireCooldown)
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            DirectionFace(mousePosition);
            // Ghi nhận thời gian bắn cuối cùng
            _playerFire.lastFireTime = Time.time;
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
        _playerComponent._skeletonAnimation.state.SetAnimation(0, "Shoot", false);
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
        Vector2 fireDirection = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) -
                                 (Vector2)_playerFire._firePosition.position).normalized;
        //spam ball
        _playerFire._ballFireGameObject = LeanPool.Spawn(
            (_playerFire._typeBall == Player_Fire.TypeBall.Blue
                ? _playerFire._prefabs_Blue_BallFire
                : _playerFire._prefabs_Red_BallFire),
            _playerFire._firePosition.position, Quaternion.identity);
        //fire ball
        _playerFire._ballFireGameObject.GetComponent<Rigidbody2D>().velocity =
            fireDirection * _playerFire._speedBall;
        //change skin , ball
        _playerFire._typeBall = (_playerFire._typeBall == Player_Fire.TypeBall.Blue)
            ? Player_Fire.TypeBall.Red
            : Player_Fire.TypeBall.Blue;
        _playerFire._skinPlayer = _playerFire._typeBall.ToString();
        _playerComponent._skeletonAnimation.Skeleton.SetSkin(_playerFire._skinPlayer);
        _playerComponent._skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        // Đăng ký callback khi animation bắn kết thúc
        _playerComponent._skeletonAnimation.AnimationState.Complete += OnFireAnimationComplete;
    }

    private void OnFireAnimationComplete(TrackEntry trackEntry)
    {
        // Hủy đăng ký callback và chuyển sang animation Idle
        _playerComponent._skeletonAnimation.AnimationState.Complete -= OnFireAnimationComplete;
        _playerComponent._skeletonAnimation.AnimationState.SetAnimation(0, "Idle_1", true);
        _playerComponent._skeletonAnimation.Skeleton.SetToSetupPose();
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
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("WallPortal"))
        {
            Debug.Log(1);
            Debug.Log(PortalManager.Instance._portalSpawn._forcePlayer);
            float velocityThreshold = 0.0001f;

            if (Mathf.Abs(_playerComponent._rigidbody.velocity.y) < velocityThreshold)
            {
                Debug.Log("Get it");
                PortalManager.Instance._portalSpawn._forcePlayer = 0;
                PortalManager.Instance._portalSpawn.isCheckingMoveWhenTele = true;
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            Debug.Log(2);
            _playerComponent._collider.isTrigger = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            Debug.Log(3);
            _playerComponent._collider.isTrigger = false;
        }
    }
}