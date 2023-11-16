using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //main refs
    private PlayerAnimation _playerAnim;
    private CharacterController _controller;

    //camera
    private Transform _camera;

    //movement
    private Vector3 _playerVelocity;
    private float _moveSpeed = 2.8f;

    void Start()
    {
        //main refs
        _playerAnim = GetComponent<PlayerAnimation>();
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;

        StartCoroutine(_playerAnim.InitSpritePerspectiveOnFirstFrame(_camera));
    }

    void Update()
    {
        PlayerMovement();
        CalculateAnimations();
        _playerAnim.UpdateSpritePerspective(_camera);
    }

    private void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        //apply lateral movement + rotation when receiving input
        if (inputDirection.magnitude >= 0.1f)
        {
            float moveTargetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, moveTargetAngle, 0);

            Vector3 moveDirection = Quaternion.Euler(0, moveTargetAngle, 0) * Vector3.forward;
            _playerVelocity = (moveDirection * _moveSpeed * Time.deltaTime);
        }
        else
        {
            _playerVelocity = new Vector3(0, _playerVelocity.y, 0); //controller velocity doesn't work without explicitly settings values to 0
        }

        _controller.Move(_playerVelocity);
    }

    private void CalculateAnimations()
    {
        Vector3 normalizedVelocity = transform.InverseTransformDirection(_controller.velocity.normalized);
        float playerForwardVelocity = normalizedVelocity.z;
        _playerAnim.PlayWalkAnimation(playerForwardVelocity);
    }
}
