using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rgbd;
    void Start()
    {
        _rgbd.AddForce(transform.forward * 10f, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerModel enemy))
        {
            enemy.TakeDamage(25f);
        }
        
        Destroy(gameObject);
    }
}
