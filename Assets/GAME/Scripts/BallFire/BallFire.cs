using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class BallFire : MonoBehaviour, ICollidable
{
    public enum TypeBall
    {
        Red,
        Blue
    }

    public TypeBall _typeBall;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WallPortal") && collision.contactCount > 0)
        {
            Collider2D wallCollider = collision.collider.GetComponent<Collider2D>();
            if (wallCollider != null)
            {
                SpawnPortal(collision, wallCollider);
                Debug.Log("Done!");
            }
        }
    }

    private void SpawnPortal(Collision2D collision, Collider2D wallCollider)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 contactPoint = contact.point;
        Vector2 contactNormal = contact.normal;
        LeanPool.Despawn(PlayerController.Instance._playerFire._ballFireGameObject);

        #region lấy góc cho Cổng

        float angle = 0;
        if (collision.gameObject.TryGetComponent(out DirectionWall directionWall))
        {
            angle = (int)directionWall._direction;
        }

        #endregion

        //Spawn cổng
        if (_typeBall == TypeBall.Blue)
        {
            if (PortalManager.Instance._portalSpawn._portalBlue!= null)
            {
                LeanPool.Despawn(PortalManager.Instance._portalSpawn._portalBlue);
            }
            PortalManager.Instance._portalSpawn._portalBlue = LeanPool.Spawn(
                PortalManager.Instance._portalSpawn._prefabsPortalBlue, collision.transform.position,
                Quaternion.Euler(0, 0, angle));
            //lấy collider của cổng
            Collider2D portalCollider = PortalManager.Instance._portalSpawn._portalBlue.GetComponent<Collider2D>();

            #region Điều chỉnh vị trí của box collider của portal để dính sát vào box collider của wall

            /*Vector2 portalColliderSize = portalCollider.bounds.size;
            Vector2 wallCenter = wallCollider.bounds.center;
            float wallWidth = wallCollider.bounds.size.x;
            float portalWidth = portalColliderSize.x;
            float distanceFromWallCenter = (wallWidth + portalWidth) * 0.5f;
            Vector2 newPosition = wallCenter + contactNormal * distanceFromWallCenter;
            newPosition.y = contactPoint.y;
            float newY = Mathf.Clamp(newPosition.y, wallCollider.bounds.min.y + portalColliderSize.y * 0.5f,
                wallCollider.bounds.max.y - portalColliderSize.y * 0.5f);
            newPosition.y = newY;
            PortalManager.Instance._portalSpawn._portalBlue.transform.position = newPosition;*/

            #endregion
            #region Điều chỉnh vị trí của box collider của portal để dính sát vào box collider của wall

            Vector2 portalColliderSize = portalCollider.bounds.size;
            Vector2 wallCenter = wallCollider.bounds.center;
            float wallHeight = wallCollider.bounds.size.y;
            float portalHeight = portalColliderSize.y;
            float distanceFromWallCenter = (wallHeight + portalHeight) * 0.5f;
            Vector2 newPosition = wallCenter + contactNormal * distanceFromWallCenter;
            newPosition.x = contactPoint.x;
            float newX = Mathf.Clamp(newPosition.x, wallCollider.bounds.min.x + portalColliderSize.x * 0.5f,
                wallCollider.bounds.max.x - portalColliderSize.x * 0.5f);
            newPosition.x = newX;
            PortalManager.Instance._portalSpawn._portalBlue.transform.position = newPosition;

            #endregion

        }
        else if (_typeBall == TypeBall.Red)
        {
            if (PortalManager.Instance._portalSpawn._portalRed!= null)
            {
                LeanPool.Despawn(PortalManager.Instance._portalSpawn._portalRed);
            }
            PortalManager.Instance._portalSpawn._portalRed = LeanPool.Spawn(PortalManager.Instance._portalSpawn._prefabsPortalRed, collision.transform.position, Quaternion.Euler(0, 0, angle));
            //lấy collider của cổng
            Collider2D portalCollider = PortalManager.Instance._portalSpawn._portalRed.GetComponent<Collider2D>();

            #region Điều chỉnh vị trí của box collider của portal để dính sát vào box collider của wall

            Vector2 portalColliderSize = portalCollider.bounds.size;
            Vector2 wallCenter = wallCollider.bounds.center;
            float wallWidth = wallCollider.bounds.size.x;
            float portalWidth = portalColliderSize.x;
            float distanceFromWallCenter = (wallWidth + portalWidth) * 0.5f;
            Vector2 newPosition = wallCenter + contactNormal * distanceFromWallCenter;
            newPosition.y = contactPoint.y;
            float newY = Mathf.Clamp(newPosition.y, wallCollider.bounds.min.y + portalColliderSize.y * 0.5f,
                wallCollider.bounds.max.y - portalColliderSize.y * 0.5f);
            newPosition.y = newY;
            PortalManager.Instance._portalSpawn._portalRed.transform.position = newPosition;

            #endregion
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