using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SharedMode
{
    public class NetworkCharacterController : MonoBehaviour
    {
        NetworkInputData _networkInputs;

        [SerializeField] private bool _isJumpPressed;
        [SerializeField] private bool _isCrouchPressed;
        [SerializeField] private bool _isStand;
        [SerializeField] private bool _isRunPressed;
        [SerializeField] private bool _isSlidePressed;
        [SerializeField] private bool _isAttackPressed;

        void Start()
        {
            _networkInputs = new NetworkInputData();
        }

        void Update()
        {
            _networkInputs.xMovement = Input.GetAxis("Horizontal");
            _networkInputs.yMovement = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isJumpPressed = true;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                _isCrouchPressed = true;
            }
            if (Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftControl))
            {
                _isStand = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _isRunPressed = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                _isSlidePressed = true;
            }
            if (Input.GetMouseButtonDown(0))
            {
                _isAttackPressed = true;
            }
        }

        public NetworkInputData GetLocalInputs()
        {
            _networkInputs.isJumpPressed = _isJumpPressed;
            _isJumpPressed = false;

            _networkInputs.isCrouchPressed = _isCrouchPressed;
            _isCrouchPressed = false;

            _networkInputs.isStand = _isStand;
            _isStand = false;

            _networkInputs.isRunPressed = _isRunPressed;
            _isRunPressed = false;

            _networkInputs.isSlidePressed = _isSlidePressed;
            _isSlidePressed = false;

            _networkInputs.isAttackPressed = _isAttackPressed;
            _isAttackPressed = false;

            return _networkInputs;  
        }
    }
}

