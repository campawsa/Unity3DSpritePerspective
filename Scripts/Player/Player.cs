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
    private Vector3 _playerMovement;
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
            _playerMovement = (moveDirection * _moveSpeed);
        }
        else _playerMovement = Vector3.zero;

        _controller.Move(_playerMovement * Time.deltaTime);
    }

    private void CalculateAnimations()
    {
        Vector3 normalizedVelocity = transform.InverseTransformDirection(_controller.velocity.normalized);
        float playerForwardVelocity = normalizedVelocity.z;
        _playerAnim.PlayWalkAnimation(playerForwardVelocity);
    }
}
