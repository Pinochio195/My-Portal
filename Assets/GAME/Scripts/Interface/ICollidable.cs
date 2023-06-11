using UnityEngine;

public interface ICollidable
{
    void OnCollisionEnter2D(Collision2D collision);
    void OnCollisionExit2D(Collision2D collision);
    void OnTriggerEnter2D(Collider2D other);
    void OnTriggerExit2D(Collider2D other);
}