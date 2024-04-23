using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : NetworkBehaviour
{
    [SerializeField] private NetworkRigidbody _networkRgbd;
   // [SerializeField] private NetworkTransform _networkTransform;
    [SerializeField] private NetworkMecanimAnimator _networkAnimator;
    
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private ParticleSystem _shootParticle;
    [SerializeField] private Transform _shootPosition;

    [SerializeField] private float _life;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private int _currentSign, _previousSign;

    [Networked(OnChanged = nameof(OnFiringChanged))]
    bool _isFiring { get; set; }

    private float _lastFiringTime;

    private NetworkInputData _inputs;    

    
    void Start()
    {
        transform.forward = Vector3.right;
    }

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out _inputs))
        {
            if (_inputs.isFirePressed) Shoot();
            if (_inputs.isJumpPressed) Jump();

            Move(_inputs.xMovement);
        }
    }

    void Move(float xAxi)
    {
        if (xAxi != 0)
        {
            _networkRgbd.Rigidbody.MovePosition(transform.position + Vector3.right * (xAxi * _speed * Time.fixedDeltaTime));

            _currentSign = (int)Mathf.Sign(xAxi);

            if (_currentSign != _previousSign)
            {
                _previousSign = _currentSign;

                transform.rotation = Quaternion.Euler(Vector3.up * (90 * _currentSign));
            }
            
            _networkAnimator.Animator.SetFloat("MovementValue", Mathf.Abs(xAxi));
        }
        else if (_currentSign != 0)
        {
            _currentSign = 0;
            _networkAnimator.Animator.SetFloat("MovementValue", 0);
        }
    }
    
    void Jump()
    {
        _networkRgbd.Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }

    //Aca llegamos
    void Shoot()
    {
        if (Time.time - _lastFiringTime < 0.15f) return;

        _lastFiringTime = Time.time;

        Runner.Spawn(_bulletPrefab, _shootPosition.position, transform.rotation);

        StartCoroutine(FiringCooldown());
    }

    IEnumerator FiringCooldown()
    {
        _isFiring = true;

        yield return new WaitForSeconds(0.15f);

        _isFiring = false;
    }

    static void OnFiringChanged(Changed<PlayerModel> changed)
    {
        var updatedFiring = changed.Behaviour._isFiring;
        changed.LoadOld();
        var oldFiring = changed.Behaviour._isFiring;

        if (!oldFiring && updatedFiring)
        {
            changed.Behaviour._shootParticle.Play();
        }
    }

    public void TakeDamage(float dmg)
    {
        RPC_TakeDamage(dmg);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_TakeDamage(float dmg)
    {
        _life -= dmg;

        if (_life <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        Runner.Shutdown();
    }
}