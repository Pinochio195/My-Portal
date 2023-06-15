using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderPortal : MonoBehaviour, ICollidable
{
    public enum TypePortal
    {
        Red,
        Blue
    }

    public TypePortal _typePortal;

    private Vector3 _transformPortal; //vị trí của portal kia khi chạm vào 1 trong 2 portal
    private Vector3 _rotationPortal; //góc của portal kia khi chạm vào 1 trong 2 portal

    private Quaternion portalBRotation;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        /*if (col.CompareTag("Player") && !PortalManager.Instance._portalSpawn.isCheckEnablePortal)
        {
            if (PortalManager.Instance._portalSpawn._portalRed != null)
            {
                GetForce(col);
                col.GetComponent<Rigidbody2D>().velocity =
                    (_typePortal == TypePortal.Red
                        ? PortalManager.Instance._portalSpawn._portalBlue.transform.up
                        : PortalManager.Instance._portalSpawn._portalRed.transform.up) *
                    PortalManager.Instance._portalSpawn._forcePlayer;
                PortalManager.Instance._portalSpawn.isCheckEnablePortal = true;

                #region Vô hiệu hóa di chuyển của player

                PlayerController.Instance.StopMovingLeft();
                PlayerController.Instance.StopMovingRight();
                PortalManager.Instance._portalSpawn.isCheckingMoveWhenTele = false;

                #endregion
            }
        }*/
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !PortalManager.Instance._portalSpawn.isCheckEnablePortal)
        {
            if (PortalManager.Instance._portalSpawn._portalRed != null && PortalManager.Instance._portalSpawn._portalBlue != null)
            {
                GetForce(col);
                col.GetComponent<Rigidbody2D>().velocity =
                    (_typePortal == TypePortal.Red
                        ? PortalManager.Instance._portalSpawn._portalBlue.transform.up
                        : PortalManager.Instance._portalSpawn._portalRed.transform.up) *
                    PortalManager.Instance._portalSpawn._forcePlayer;
                PortalManager.Instance._portalSpawn.isCheckEnablePortal = true;

                #region Vô hiệu hóa di chuyển của player

                PlayerController.Instance.StopMovingLeft();
                PlayerController.Instance.StopMovingRight();
                PortalManager.Instance._portalSpawn.isCheckingMoveWhenTele = false;

                #endregion
            }
        }
    }

    private void GetForce(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (PortalManager.Instance._portalSpawn._forcePlayer == 0)
            {
                PortalManager.Instance._portalSpawn._forcePlayer = Mathf.Abs(col.GetComponent<Rigidbody2D>().velocity.y) + 6f;
            }

            portalBRotation = _typePortal == TypePortal.Red
                ? PortalManager.Instance._portalSpawn._portalBlue.transform.rotation
                : PortalManager.Instance._portalSpawn._portalRed.transform.rotation;
            Debug.Log(portalBRotation.eulerAngles.z);
            if (portalBRotation.eulerAngles.z == 0f || portalBRotation.eulerAngles.z == 180f)
            {
                PortalManager.Instance._portalSpawn._forcePlayer = Mathf.Clamp(PortalManager.Instance._portalSpawn._forcePlayer, 8f, 25f);
                Debug.Log("Giới hạn 5");
            }
            else if (portalBRotation.eulerAngles.z == 90f || portalBRotation.eulerAngles.z == 270f)
            {
                Debug.Log("Giới hạn 2");
                PortalManager.Instance._portalSpawn._forcePlayer = Mathf.Clamp(PortalManager.Instance._portalSpawn._forcePlayer, 2f, 25f);
            }
        }
        CheckTypePortal(col,portalBRotation);
    }

    private void CheckTypePortal(Collider2D col,Quaternion portalBRotation)
    {
        
        col.gameObject.SetActive(false);
        // Xác định hướng lùi dựa trên giá trị góc
        int direction = Mathf.Approximately(portalBRotation.eulerAngles.z, 90f) ? -1 : 1;
        if (_typePortal == TypePortal.Blue)
        {
            _transformPortal = PortalManager.Instance._portalSpawn._portalRed.transform.position;
            PortalManager.Instance._portalSpawn._wallTouch_PortalRed.isTrigger = true;
        }
        else
        {
            _transformPortal = PortalManager.Instance._portalSpawn._portalBlue.transform.position;
            PortalManager.Instance._portalSpawn._wallTouch_PortalBlue.isTrigger = true;
        }
        Debug.Log(portalBRotation.eulerAngles.z);
        if (Mathf.Approximately(portalBRotation.eulerAngles.z, 90f) || Mathf.Approximately(portalBRotation.eulerAngles.z, 270f))
        {
            _transformPortal += new Vector3(direction * -0.5f, -0.5f, 0);
        }

        
        col.transform.position = _transformPortal;
        PlayerController.Instance._playerComponent._skeletonAnimation.skeleton.ScaleX =
            ((_typePortal == TypePortal.Red
                ? PortalManager.Instance._portalSpawn._portalBlue.transform.eulerAngles.z
                : PortalManager.Instance._portalSpawn._portalRed.transform.eulerAngles.z) == 90)
                ? 1f
                : -1f;
        col.gameObject.SetActive(true);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && PortalManager.Instance._portalSpawn.isCheckEnablePortal)
        {
            PortalManager.Instance._portalSpawn.isCheckEnablePortal = false;
            if (PortalManager.Instance._portalSpawn._wallTouch_PortalBlue.isTrigger)
            {
                PortalManager.Instance._portalSpawn._wallTouch_PortalBlue.isTrigger = false;
            }
            if(PortalManager.Instance._portalSpawn._wallTouch_PortalRed.isTrigger)
            {
                PortalManager.Instance._portalSpawn._wallTouch_PortalRed.isTrigger = false;
            }
        }
    }
}