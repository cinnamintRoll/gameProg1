using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _speed = 1000f;
    private bool isRight;
    private Vector2 TargetDirection => isRight ? Vector2.right : Vector2.left;

    public void SetDirection(bool isRight)
    {
        this.isRight = isRight;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rb.AddForce(Vector2.right * _speed);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Platform":
            case "Player":
            case "Death":
            case "Enemy":
                break;
            default:
                Destroy(col.gameObject);
                break;
        }

    }
}
