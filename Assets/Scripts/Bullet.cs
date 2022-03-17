using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private int bulletDamage;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    private Coroutine _lifetimeCoroutine;

    public void Shot(Vector3 direction)
    {
        _lifetimeCoroutine = StartCoroutine(Lifetime());
        _rb.velocity = direction.normalized * speed;
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var damageable = collision.collider.GetComponent<IDamageable>();
        damageable?.TakeDamage(bulletDamage);
        StopCoroutine(_lifetimeCoroutine);
        gameObject.SetActive(false);
    }
}