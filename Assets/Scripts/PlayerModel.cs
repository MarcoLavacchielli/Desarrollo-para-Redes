using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : NetworkBehaviour
{
    [SerializeField] private NetworkRigidbody _networkRgbd;
    [SerializeField] private NetworkMecanimAnimator _networkAnimator;
   // [SerializeField] private NetworkTransform _networkTransform;

    //[SerializeField] private Bullet _bulletPrefab;
    //[SerializeField] private ParticleSystem _shootParticle;
    //[SerializeField] private Transform _shootPosition;
    //bool _isFiring { get; set; }

    //private float _lastFiringTime;

    //private int _currentSign, _previousSign;

    //[Networked(OnChanged = nameof(OnFiringChanged))]

    private NetworkInputData _inputs;

    [Header("Movimiento")]
    [SerializeField] private float _speed;
    private float startSpeed;
    [SerializeField] private float _rotationSpeed = 10f;

    [Header("Vida")]
    [SerializeField] private float _life;

    [Header("Salto")] //salto
    [SerializeField] private int _maxJumps = 1;
    private int _remainingJumps;
    [SerializeField] private float _jumpForce;
    //

    [Header("Agacharse")] // crouch
    public float crouchSpeed;
    public float crouchYScale;
    //

    [Header("Correr")] // sprint
    public float sprintVelocity;
    //

    [Header("Deslizar")] // Deslizar
    public float maxSlideTime;
    public float slideSpeed;
    private bool _isSliding = false;
    private float _slideTimer = 0f;
    //


    void Start()
    {
        transform.forward = Vector3.right;
        _remainingJumps = _maxJumps;

        startSpeed = _speed;
    }

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out _inputs))
        {
            //if (_inputs.isFirePressed) Shoot();
            if (_inputs.isJumpPressed) Jump();

            if (_inputs.isCrouchPressed) Crouch();

            if (_inputs.isStand) Stand();

            if (_inputs.isRunPressed) Sprint();

            if (_inputs.isSlidePressed && !_isSliding)
            {
                StartSliding();
            }

            if (_isSliding)
            {
                Slide();
            }


            Move(_inputs.xMovement, _inputs.yMovement);
        }

        /*if (!_inputs.isCrouchPressed && transform.localScale.y != 1)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        }*/
    }

    void Move(float xAxi, float yAxi)
    {
        Vector3 movement = new Vector3(xAxi, 0, yAxi).normalized;
        if (movement != Vector3.zero)
        {
            _networkRgbd.Rigidbody.MovePosition(transform.position + movement * _speed * Time.fixedDeltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed); // Ajuste de velocidad de rotación aquí

            _networkAnimator.Animator.SetFloat("MovementValue", movement.magnitude);
        }
        else
        {
            _networkAnimator.Animator.SetFloat("MovementValue", 0);
        }
    }
    
    void Jump()
    {
        if (_remainingJumps > 0)
        {
            _networkRgbd.Rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
            _remainingJumps--;
        }
    }

    void Crouch()
    {
        if (_inputs.isCrouchPressed)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            _networkRgbd.Rigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            _speed = crouchSpeed;
        }
    }

    void Stand()
    {
        transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        _speed = startSpeed;
    }

    void Sprint()
    {
        _speed = sprintVelocity;
    }

    void StartSliding()
    {
        _isSliding = true;
        _slideTimer = 0f;
        _speed = slideSpeed;
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        _networkRgbd.Rigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    void Slide()
    {
        _slideTimer += Time.fixedDeltaTime;

        if (_slideTimer >= maxSlideTime)
        {
            EndSlide();
        }
    }

    void EndSlide()
    {
        _isSliding = false;
        _slideTimer = 0f;
        _speed = startSpeed;
        transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
    }

    //Aca llegamos
    /*void Shoot()
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
    }*/

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            _remainingJumps = _maxJumps; // Restaurar los saltos disponibles al tocar el suelo
        }
    }
}
