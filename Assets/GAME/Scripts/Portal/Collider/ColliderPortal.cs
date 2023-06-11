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
        if (col.CompareTag("Player")&&!PortalManager.Instance.isCheckEnablePortal)
        {
            CheckTypePortal(col);
            col.GetComponent<Rigidbody2D>().velocity = transform.up * PortalManager.Instance._forcePlayer;
            PortalManager.Instance.isCheckEnablePortal = true;
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
        col.gameObject.SetActive(true);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")&&PortalManager.Instance.isCheckEnablePortal)
        {
            PortalManager.Instance.isCheckEnablePortal = false;
        }
    }
}