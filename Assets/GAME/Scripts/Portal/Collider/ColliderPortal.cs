using System.Collections;
using System.Collections.Generic;
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
        if (col.CompareTag("Player") && !PortalManager.Instance.isCheckEnablePortal)
        {
            GetForce(col);
            CheckTypePortal(col);
            col.GetComponent<Rigidbody2D>().velocity =
                (_typePortal == TypePortal.Red
                    ? PortalManager.Instance._portalBlue.transform.up
                    : PortalManager.Instance._portalRed.transform.up) * PortalManager.Instance._forcePlayer;
            PortalManager.Instance.isCheckEnablePortal = true;
        }
    }

    private void GetForce(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (PortalManager.Instance._forcePlayer == 0)
            {
                PortalManager.Instance._forcePlayer = Mathf.Abs(col.GetComponent<Rigidbody2D>().velocity.y) + 6f;
            }

            Quaternion portalBRotation = _typePortal == TypePortal.Red
                ? PortalManager.Instance._portalBlue.transform.rotation
                : PortalManager.Instance._portalRed.transform.rotation;

            if (portalBRotation.eulerAngles.z == 0f || portalBRotation.eulerAngles.z == 180f)
            {
                PortalManager.Instance._forcePlayer = Mathf.Clamp(PortalManager.Instance._forcePlayer, 5f, 25f);
                Debug.Log("Giới hạn 5");
            }
            else if (portalBRotation.eulerAngles.z == 90f || portalBRotation.eulerAngles.z == -90f)
            {
                Debug.Log("Giới hạn 2");
                PortalManager.Instance._forcePlayer = Mathf.Clamp(PortalManager.Instance._forcePlayer, 2f, 25f);
            }
        }
    }

    private void CheckTypePortal(Collider2D col)
    {
        col.gameObject.SetActive(false);
        if (_typePortal == TypePortal.Blue)
        {
            _transformPortal = PortalManager.Instance._portalRed.transform.position;
        }
        else
        {
            _transformPortal = PortalManager.Instance._portalBlue.transform.position;
        }

        col.transform.position = _transformPortal;
        PlayerController.Instance._playerComponent._skeletonAnimation.skeleton.ScaleX =
            ((_typePortal == TypePortal.Red
                ? PortalManager.Instance._portalBlue.transform.eulerAngles.z
                : PortalManager.Instance._portalRed.transform.eulerAngles.z) == 90)
                ? 1f
                : -1f;
        col.gameObject.SetActive(true);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && PortalManager.Instance.isCheckEnablePortal)
        {
            PortalManager.Instance.isCheckEnablePortal = false;
        }
    }
}