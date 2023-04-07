using System.Linq;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float _gunRange;
    [SerializeField] private Transform _leftDepth;
    [SerializeField] private Transform _rightDepth;
    [SerializeField] private Transform _floorDepth;
    [SerializeField] private GameObject _bulletPrefab;

    [SerializeField] private float _depthRange = 1f;
    [SerializeField] private float _gravity = 2f;
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _projectileCooldown = 3f;
    /// <summary>
    /// refers to left or right, right if true , left if false
    /// </summary>
    private bool _direction = false;
    private float _directionInt => _direction ? 1 : -1;

    private float _lastFireTime;

    private void Update()
    {
        Gravity();

        if (!HasFloor(_rightDepth))
        {
            _direction = false;
        }

        if (!HasFloor(_leftDepth))
        {
            _direction = true;
        }

        MoveObject();

        if (HasDetectedPlayer())
        {
            FireProjectile();
        }
    }


    private bool HasFloor(Transform depth)
    {
        var rays = Physics2D.RaycastAll(depth.position, Vector2.down, _depthRange);
        return rays.Any(ray => ray.collider);
    }

    private void MoveObject()
    {

        var speedMath = _directionInt * Time.deltaTime * _speed;
        var transform1 = this.transform;
        var currentPos = transform1.position;
        transform1.position = new Vector3(currentPos.x + speedMath, currentPos.y, currentPos.z);
    }

    private void Gravity()
    {
        if (HasFloor(_floorDepth)) return;

        var transform1 = this.transform;
        var currentPos = transform1.position;
        transform1.position = new Vector3(currentPos.x, currentPos.y - _gravity * Time.deltaTime, currentPos.z);
    }

    private bool HasDetectedPlayer()
    {
        var rays = Physics2D.RaycastAll(this.transform.position, Vector2.right * _directionInt, _gunRange);
        return rays.Any(ray => ray.collider && ray.collider.gameObject.CompareTag("Player"));
    }

    private void FireProjectile()
    {
        if (!(Time.time - _lastFireTime > _projectileCooldown)) return;
        _lastFireTime = Time.time;

        var position = this.transform.position;

        var bullet = Instantiate(_bulletPrefab, new Vector3(position.x + (2 * _directionInt), position.y, position.z), Quaternion.identity);
        bullet.SetActive(false);
        var bulletComp = bullet.GetComponent<EnemyBullet>();
        bulletComp.SetDirection(!_direction);
        bullet.SetActive(true);
    }





}
