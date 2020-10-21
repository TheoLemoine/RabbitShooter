using System;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Vector2 mouseSensibility = new Vector2(1f, 1f);
        [SerializeField] private float playerSpeed = 1f;

        private Vector2 _playerInputMovement = new Vector3(0f, 0f);
        private Vector2 _playerCameraMovement = new Vector3(0f, 0f);

        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform camTransform;
        [SerializeField] private Transform playerTransform;

        private void Start()
        {
            // locking cursor so it does not go out of window.
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // get inputs from update and store them
            _playerInputMovement = new Vector2(-Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical")) * playerSpeed;
            _playerCameraMovement = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            
            // apply camera movement here
            camTransform.Rotate(mouseSensibility.y * _playerCameraMovement.y * Time.deltaTime, 0, 0);
            playerTransform.Rotate(0, mouseSensibility.x * _playerCameraMovement.x * Time.deltaTime, 0);
        }

        // apply inputs related to physics
        private void FixedUpdate()
        {
            rb.velocity = transform.rotation * new Vector3(
                _playerInputMovement.y,
                rb.velocity.y,
                _playerInputMovement.x
            );
        }
    }
}