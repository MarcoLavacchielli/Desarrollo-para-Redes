using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerModel : NetworkBehaviour
{
    [SerializeField] private NetworkRigidbody _networkRgbd;
    [SerializeField] private NetworkMecanimAnimator _networkAnimator;

    [SerializeField] private Transform _childModel;

    public Camera camaraActivada;

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
    public float crouchYScale = 0.5f;
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

    [Header("Ataque")] // Deslizar
    [SerializeField] private float attackRadius = 1.5f;
    [SerializeField] private LayerMask objLayer;
    public int danio;
    private bool isAttacking = false;
    [SerializeField] private float cooldown = 0.5f;
    private float nextAttackTime = 0f;
    //

    /*[Header("Paneles de victoria y derrota")]//
    public GameObject victoryScreen;
    public GameObject defeatScreen;
    *///

    void Start()
    {
        transform.forward = Vector3.right;
        _remainingJumps = _maxJumps;
        startSpeed = _speed;

        //
        Rigidbody rb = _networkRgbd.Rigidbody;
        rb.interpolation = RigidbodyInterpolation.Extrapolate;
        //
    }

    public override void FixedUpdateNetwork()
    {
        _networkAnimator.Animator.SetBool("slowRun", false);
        _networkAnimator.Animator.SetBool("crouchIdle", false);
        _networkAnimator.Animator.SetBool("fastRun", false);
        _networkAnimator.Animator.SetBool("isAttack", false);

        if (_speed == crouchSpeed)
        {
            _networkAnimator.Animator.SetBool("crouchIdle", true);
        }

        if (_speed == sprintVelocity)
        {
            _networkAnimator.Animator.SetBool("fastRun", true);
        }

        if (isAttacking == true)
        {
            _networkAnimator.Animator.SetBool("isAttack", true);
        }

        if (GetInput(out _inputs))
        {
            if (_inputs.isJumpPressed)
            {
                Jump();
            }

            if (_inputs.isCrouchPressed)
            {
                Crouch();
            }

            if (_inputs.isStand)
            {
                Stand();
            }

            if (_inputs.isRunPressed)
            {
                Sprint();
            }

            if (_inputs.isSlidePressed && !_isSliding)
            {
                StartSliding();
            }

            if (_isSliding)
            {
                Slide();
            }

            if (_inputs.isAttackPressed && Time.time >= nextAttackTime && !isAttacking)
            {
                Attack();
            }

            Move(_inputs.xMovement, _inputs.yMovement);

            camaraActivada.gameObject.SetActive(true);
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
            _childModel.rotation = Quaternion.Slerp(_childModel.rotation, targetRotation, Time.deltaTime * _rotationSpeed); // Aplicar rotación al modelo hijo
            _networkAnimator.Animator.SetBool("slowRun", true);

            // Envía la rotación al servidor
            RpcUpdateRotation(_childModel.rotation);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RpcUpdateRotation(Quaternion newRotation)
    {
        _childModel.rotation = newRotation;
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
            transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
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
        if(transform.localScale == new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z))
        {
            _speed = crouchSpeed;
        }
        else
        {
            _speed = sprintVelocity;
        }
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

    void Attack()
    {
        if (!isAttacking && Time.time >= nextAttackTime)
        {
            isAttacking = true;
            nextAttackTime = Time.time + cooldown;

            // Llamar al RPC de ataque en el servidor
            RpcPerformAttack();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RpcPerformAttack()
    {
        StartCoroutine(ServerPerformAttack());
    }

    IEnumerator ServerPerformAttack()
    {
        Debug.Log("Attacking");

        // Retraso antes de realizar el ataque (opcional)
        yield return new WaitForSeconds(0.5f);

        // Lógica de ataque
        AttackDestroyer();

        // Notificar a todos los clientes que el ataque ha terminado
        RpcAttackFinished();
    }


    [Rpc(RpcSources.All, RpcTargets.All)]
    void RpcAttackFinished()
    {
        isAttacking = false;
    }

    void AttackDestroyer()
    {

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRadius, objLayer);

        foreach (Collider hitObject in hitEnemies)
        {
            Fusion.NetworkObject networkObject = hitObject.GetComponent<Fusion.NetworkObject>();

            if (networkObject != null)
            {
                Runner.Despawn(networkObject);
            }
            else
            {
                Debug.LogWarning("El objeto " + hitObject.name + " no tiene un componente NetworkObject adjunto.");
            }
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            _remainingJumps = _maxJumps; 
        }
    }

    [SerializeField] NetworkBool gano = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Meta")
        {
            Gano();
            gano = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public GameObject victoryScreen;
    public GameObject defeatScreen;

    public void Gano()
    {
        victoryScreen.SetActive(true);
        defeatScreen = null;
        //SceneManager.LoadScene("Victory");
        gano = true;
    }

    public void Perdio()
    {
        if (gano == false)
        {
            defeatScreen.SetActive(true);
            victoryScreen = null;
            //SceneManager.LoadScene("Defeat");
        }
        if (gano == true)
        {
            Gano();
        }
    }

}
