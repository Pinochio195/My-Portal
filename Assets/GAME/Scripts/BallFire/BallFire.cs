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
        else if (collision.gameObject.CompareTag("Ground"))
        {
            LeanPool.Despawn(PlayerController.Instance._playerFire._ballFireGameObject);
        }
    }

    private void SpawnPortal(Collision2D collision, Collider2D wallCollider)
    {
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 contactPoint = contact.point;
        Vector2 contactNormal = contact.normal;

        #region lấy góc cho Cổng

        float angle = 0;
        if (collision.gameObject.TryGetComponent(out DirectionWall directionWall))
        {
            angle = (int)directionWall._direction;
        }

        #endregion
        LeanPool.Despawn(PlayerController.Instance._playerFire._ballFireGameObject);


        #region Check space để spawn portal

        if (_typeBall == TypeBall.Blue)
        {
            var portalRed = PortalManager.Instance._portalSpawn._portalRed;
            var redCollider = portalRed != null ? portalRed.GetComponent<Collider2D>() : null;
            var portalBounds = redCollider != null ? redCollider.bounds : default;

                if (portalRed == null || Mathf.Abs(contactPoint.y- portalRed.transform.position.y) <= .75f ||
                    Mathf.Abs(contactPoint.x- portalRed.transform.position.x) <=.75f)
            {
                if (angle == 90 || angle == -90)
                {
                    if (portalRed != null && contactPoint.y < portalRed.transform.position.y)
                    {
                        if (Mathf.Abs(portalBounds.min.y - wallCollider.bounds.min.y) < portalBounds.size.y)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(portalBounds.max.y - wallCollider.bounds.max.y) < portalBounds.size.y)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    if (portalRed != null && contactPoint.x < portalRed.transform.position.x)
                    {
                        if (Mathf.Abs(portalBounds.min.x - wallCollider.bounds.min.x) < portalBounds.size.x)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(portalBounds.max.x - wallCollider.bounds.max.x) < portalBounds.size.x)
                        {
                            return;
                        }
                    }
                }
            }
        }
        else if (_typeBall == TypeBall.Red)
        {
            var portalBlue = PortalManager.Instance._portalSpawn._portalBlue;
            var blueCollider = portalBlue != null ? portalBlue.GetComponent<Collider2D>() : null;
            var portalBounds = blueCollider != null ? blueCollider.bounds : default;
            if (portalBlue == null || Mathf.Abs(contactPoint.y-portalBlue.transform.position.y) <=.75f||
                Mathf.Abs(contactPoint.x-portalBlue.transform.position.x) <= .75f)
            {
                if (angle == 90 || angle == -90)
                {
                    if (portalBlue != null && contactPoint.y < portalBlue.transform.position.y)
                    {
                        if (Mathf.Abs(portalBounds.min.y - wallCollider.bounds.min.y) < portalBounds.size.y)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(portalBounds.max.y - wallCollider.bounds.max.y) < portalBounds.size.y)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    if (portalBlue != null && contactPoint.x < portalBlue.transform.position.x)
                    {
                        if (Mathf.Abs(portalBounds.min.x - wallCollider.bounds.min.x) < portalBounds.size.x)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (Mathf.Abs(portalBounds.max.x - wallCollider.bounds.max.x) < portalBounds.size.x)
                        {
                            return;
                        }
                    }
                }
            }
        }

        #endregion


        //Spawn cổng
        if (_typeBall == TypeBall.Blue)
        {
            #region Lấy wall của portal khi chạm để tắt đi khi player dịch chuyển qua cổng

            PortalManager.Instance._portalSpawn._wallTouch_PortalBlue = collision.gameObject.GetComponent<Collider2D>();

            #endregion

            #region Xóa portal cũ đang tồn tại

            if (PortalManager.Instance._portalSpawn._portalBlue != null)
            {
                LeanPool.Despawn(PortalManager.Instance._portalSpawn._portalBlue);
            }

            #endregion

            #region Sinh cổng mới

            PortalManager.Instance._portalSpawn._portalBlue = LeanPool.Spawn(
                PortalManager.Instance._portalSpawn._prefabsPortalBlue, collision.transform.position,
                Quaternion.Euler(0, 0, angle));

            #endregion

            #region Các biến nạp vào hàm

            Collider2D portalCollider = PortalManager.Instance._portalSpawn._portalBlue.GetComponent<Collider2D>();
            Vector2 wallCenter = wallCollider.bounds.center;
            Vector2 portalColliderSize = portalCollider.bounds.size;
            float wallWidthX = wallCollider.bounds.size.x;
            float wallWidthY = wallCollider.bounds.size.y;

            #endregion

            //đưa portal về đúng vị trí
            CheckUpDownPortal(wallCollider, contactPoint, angle, portalColliderSize, wallWidthX, wallWidthY, wallCenter,
                contactNormal, PortalManager.Instance._portalSpawn._portalBlue,
                PortalManager.Instance._portalSpawn._portalRed);
        }
        else if (_typeBall == TypeBall.Red)
        {
            #region Lấy wall của portal khi chạm để tắt đi khi player dịch chuyển qua cổng

            PortalManager.Instance._portalSpawn._wallTouch_PortalRed = collision.gameObject.GetComponent<Collider2D>();

            #endregion

            #region Xóa portal cũ đang tồng tại

            if (PortalManager.Instance._portalSpawn._portalRed != null)
            {
                LeanPool.Despawn(PortalManager.Instance._portalSpawn._portalRed);
            }

            #endregion

            #region Sinh cổng mới

            PortalManager.Instance._portalSpawn._portalRed = LeanPool.Spawn(
                PortalManager.Instance._portalSpawn._prefabsPortalRed, collision.transform.position,
                Quaternion.Euler(0, 0, angle));

            #endregion

            #region Các biến nạp vào hàm

            Collider2D portalCollider = PortalManager.Instance._portalSpawn._portalRed.GetComponent<Collider2D>();
            Vector2 wallCenter = wallCollider.bounds.center;
            Vector2 portalColliderSize = portalCollider.bounds.size;
            float wallWidth = wallCollider.bounds.size.x;
            float wallWidthY = wallCollider.bounds.size.y;

            #endregion

            //đưa portal về đúng vị trí
            CheckUpDownPortal(wallCollider, contactPoint, angle, portalColliderSize, wallWidth, wallWidthY, wallCenter,
                contactNormal, PortalManager.Instance._portalSpawn._portalRed,
                PortalManager.Instance._portalSpawn._portalBlue);
        }
    }

    private void CheckUpDownPortal(Collider2D wallCollider, Vector2 contactPoint, float angle,
        Vector2 portalColliderSize,
        float wallWidthX, float wallWidthY, Vector2 wallCenter, Vector2 contactNormal, GameObject portalSpawn,
        GameObject portalCurrent)
    {
        if (angle == 90 || angle == -90)
        {
            ChangePortal_90_Nega90(wallCollider, angle, portalColliderSize, wallWidthX, wallCenter, contactNormal,
                contactPoint,
                portalSpawn,
                portalCurrent);
        }
        else
        {
            ChangePortal_0_Nega180(wallCollider, angle, portalColliderSize, wallWidthY, wallCenter, contactNormal,
                contactPoint,
                portalSpawn,
                portalCurrent);
        }
    }

    #region Angle = 90 hoặc -90

    private void ChangePortal_90_Nega90(Collider2D wallCollider, float angle, Vector2 portalColliderSize,
        float wallWidth,
        Vector2 wallCenter, Vector2 contactNormal, Vector2 contactPoint, GameObject portalSpawn,
        GameObject portalCurrent)
    {
        Spawn_90_Nega90(wallCollider, portalColliderSize, wallCenter, contactNormal, contactPoint, portalSpawn,
            wallWidth);
        if (portalCurrent != null)
        {
            //đang cùng 1 cột
            if (Mathf.Abs(PortalManager.Instance._portalSpawn._portalRed.transform.position.x -
                          PortalManager.Instance._portalSpawn._portalBlue.transform.position.x) < 0.55f)
            {
                if (portalCurrent != null)
                {
                    float sizePortal = portalCurrent.GetComponent<Collider2D>().bounds.size.y / 2;
                    float minY = 0;
                    float maxY = 0;
                    if (Mathf.Abs(portalSpawn.transform.position.y - portalCurrent.transform.position.y) <
                        portalCurrent.GetComponent<Collider2D>().bounds.size.y)
                    {
                        if (contactPoint.y > portalCurrent.transform.position.y)
                        {
                            minY = portalCurrent.GetComponent<Collider2D>().bounds.max.y + sizePortal;
                            maxY = wallCollider.bounds.max.y - sizePortal;
                            Vector2 newPosition1 = (Vector2)portalCurrent.transform.position +
                                                   new Vector2(0,
                                                       portalCurrent.GetComponent<Collider2D>().bounds.size.y +
                                                       .3f);
                            newPosition1.y = Mathf.Clamp(newPosition1.y, minY, maxY);
                            portalSpawn.transform.position = newPosition1;
                        }
                        else
                        {
                            maxY = portalCurrent.GetComponent<Collider2D>().bounds.min.y - sizePortal;
                            minY = wallCollider.bounds.min.y + sizePortal;
                            Vector2 newPosition1 = (Vector2)portalCurrent.transform.position -
                                                   new Vector2(0,
                                                       portalCurrent.GetComponent<Collider2D>().bounds.size.y +
                                                       .3f);
                            newPosition1.y = Mathf.Clamp(newPosition1.y, minY, maxY);
                            portalSpawn.transform.position = newPosition1;
                        }
                    }
                }
            }
        }
    }

    private void Spawn_90_Nega90(Collider2D wallCollider, Vector2 portalColliderSize, Vector2 wallCenter,
        Vector2 contactNormal,
        Vector2 contactPoint, GameObject portalSpawn, float wallWidth)
    {
        #region Điều chỉnh vị trí của box collider của portal để dính sát vào box collider của wall

        float portalWidth = portalColliderSize.x;
        float distanceFromWallCenter = (wallWidth + portalWidth) * 0.5f;
        Vector2 newPosition = wallCenter + contactNormal * distanceFromWallCenter;
        newPosition.y = contactPoint.y;
        float newY = Mathf.Clamp(newPosition.y, wallCollider.bounds.min.y + portalColliderSize.y * 0.5f,
            wallCollider.bounds.max.y - portalColliderSize.y * 0.5f);
        newPosition.y = newY;
        portalSpawn.transform.position = newPosition;

        #endregion
    }

    #endregion

    #region Angle = 0 hoặc -180

    private void ChangePortal_0_Nega180(Collider2D wallCollider, float angle, Vector2 portalColliderSize,
        float wallWidth,
        Vector2 wallCenter, Vector2 contactNormal, Vector2 contactPoint, GameObject portalSpawn,
        GameObject portalCurrent)
    {
        Spawn_0_Nega180(wallCollider, portalColliderSize, wallCenter, contactNormal, contactPoint, portalSpawn,
            wallWidth);
        if (portalCurrent != null)
        {
            //đang cùng 1 cột
            if (Mathf.Abs(PortalManager.Instance._portalSpawn._portalRed.transform.position.y -
                          PortalManager.Instance._portalSpawn._portalBlue.transform.position.y) < 0.55f)
            {
                if (portalCurrent != null)
                {
                    float sizePortal = portalCurrent.GetComponent<Collider2D>().bounds.size.x / 2;
                    float minY = 0;
                    float maxY = 0;
                    if (Mathf.Abs(portalSpawn.transform.position.x - portalCurrent.transform.position.x) <
                        portalCurrent.GetComponent<Collider2D>().bounds.size.x)
                    {
                        if (contactPoint.x > portalCurrent.transform.position.x)
                        {
                            minY = portalCurrent.GetComponent<Collider2D>().bounds.max.x + sizePortal;
                            maxY = wallCollider.bounds.max.x - sizePortal;
                            Vector2 newPosition1 = (Vector2)portalCurrent.transform.position +
                                                   new Vector2(
                                                       portalCurrent.GetComponent<Collider2D>().bounds.size.x + .3f, 0);
                            newPosition1.x = Mathf.Clamp(newPosition1.x, minY, maxY);
                            portalSpawn.transform.position = newPosition1;
                        }
                        else
                        {
                            maxY = portalCurrent.GetComponent<Collider2D>().bounds.min.x - sizePortal;
                            minY = wallCollider.bounds.min.x + sizePortal;
                            Vector2 newPosition1 = (Vector2)portalCurrent.transform.position -
                                                   new Vector2(portalCurrent.GetComponent<Collider2D>().bounds.size.x +
                                                               .3f,
                                                       0);
                            newPosition1.x = Mathf.Clamp(newPosition1.x, minY, maxY);
                            portalSpawn.transform.position = newPosition1;
                        }
                    }
                }
            }
        }
    }

    private void Spawn_0_Nega180(Collider2D wallCollider, Vector2 portalColliderSize, Vector2 wallCenter,
        Vector2 contactNormal,
        Vector2 contactPoint, GameObject portalSpawn, float wallWidth)
    {
        #region Điều chỉnh vị trí của box collider của portal để dính sát vào box collider của wall

        float portalWidth = portalColliderSize.y - .35f;
        float distanceFromWallCenter = (wallWidth + portalWidth) * 0.5f;
        Vector2 newPosition = wallCenter + contactNormal * distanceFromWallCenter;
        newPosition.x = contactPoint.x;
        float newx = Mathf.Clamp(newPosition.x, wallCollider.bounds.min.x + portalColliderSize.x * 0.5f,
            wallCollider.bounds.max.x - portalColliderSize.x * 0.5f);
        newPosition.x = newx;
        portalSpawn.transform.position = newPosition;

        #endregion
    }

    #endregion

    public void OnCollisionExit2D(Collision2D collision)
    {
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Portal"))
        {
            if (_typeBall == TypeBall.Blue)
            {
                Debug.Log("Blue");
            }
            else
            {
                Debug.Log("Red");
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
    }
}