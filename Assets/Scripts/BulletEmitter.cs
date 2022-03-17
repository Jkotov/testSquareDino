using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int defaultBullets;
    private Camera _camera;
    private List<Bullet> _bullets;
    private void Awake()
    {
        _camera = Camera.main;
        _bullets = new List<Bullet>(defaultBullets);
        for (int i = 0; i < defaultBullets; i++)
        {
            CreateBullet();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Shot(Input.mousePosition);
        else if (Input.touchCount > 0)
        {
            var touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
                Shot(touch.position);
        }

    }

    private void Shot(Vector3 onScreenPos)
    {
        if (GameManager.instance.isGameStarted == false)
            return ;
        var pos = transform.position;
        Bullet bullet = null;
        for (int i = 0; i < _bullets.Count; i++)
        {
            if (_bullets[i].gameObject.activeInHierarchy == false)
            {
                bullet = _bullets[i];
                break;
            }
        }
        if (bullet == null)
            bullet = CreateBullet();
        bullet.transform.position = pos;
        var ray = _camera.ScreenPointToRay(onScreenPos);
        Vector3 direction = ray.direction;
        if (Physics.Raycast(ray, out var hit))
            direction = hit.point - pos;
        bullet.gameObject.SetActive(true);
        bullet.Shot(direction);
    }

    private Bullet CreateBullet()
    {
        var tmp = Instantiate(bulletPrefab, transform.position, Quaternion.identity, transform);
        var bullet = tmp.GetComponent<Bullet>();
        _bullets.Add(bullet);
        tmp.SetActive(false);
        return bullet;
    }
}
