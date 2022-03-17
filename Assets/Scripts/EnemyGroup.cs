using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{
    private Enemy[] _enemies;

    private void Awake()
    {
        _enemies = GetComponentsInChildren<Enemy>();
    }

    public bool IsEnemiesActive()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.isDead == false)
                return true;
        }
        return false;
    }
}
