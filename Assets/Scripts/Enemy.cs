using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHp;
    [SerializeField] private LineRenderer HpBar;
    [HideInInspector] public bool isDead;
    private Animator _animator;
    private int Hp
    {
        get => _hp;
        set
        {
            _hp = value;
            var endPos = HpBar.GetPosition(1);
            endPos.x = value / (float)maxHp;
            HpBar.SetPosition(1, endPos);
            if (_hp <= 0)
                Death();
        }
            
    }

    private int _hp;
    private Collider _collider;
    private Rigidbody[] _rigidbodies;

    public void TakeDamage(int damage)
    {
        Hp -= damage;
    }
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Hp = maxHp;
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _collider = GetComponent<Collider>();
        foreach (var ragdollBone in _rigidbodies)
        {
            ragdollBone.isKinematic = true;
        }
    }
    [ContextMenu("Death")]
    private void Death()
    {
        _animator.enabled = false;
        foreach (var ragdollBone in _rigidbodies)
        {
            ragdollBone.isKinematic = false;
        }
        isDead = true;
        _collider.enabled = false;
        GameManager.instance.EnemyKilled();
    }
}