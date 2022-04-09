using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputData inputDataRef;

    bool isActive = false;

    // Movement Variables
    [SerializeField] private float playerSpeed;
    [SerializeField] private Vector2 moveVector;
    [SerializeField] private Vector3 playerVelocity;
    private Rigidbody rb;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        inputDataRef = InputManager.playerInputActions;

        isActive = true;
        inputDataRef.Player.Move.performed += OnMove;
        inputDataRef.Player.Move.canceled += OnMove;
    }

    private void OnEnable()
    {
        if (isActive)
        {
            inputDataRef.Player.Move.performed += OnMove;
            inputDataRef.Player.Move.canceled += OnMove;
        }
    }

    private void OnDisable()
    {
        inputDataRef.Player.Move.performed -= OnMove;
        inputDataRef.Player.Move.canceled -= OnMove;
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(rb.position + playerVelocity * playerSpeed * Time.deltaTime);
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        moveVector = obj.ReadValue<Vector2>();
        Debug.Log(moveVector);
        playerVelocity = new Vector3(moveVector.x, 0, moveVector.y);
    }

    private void StartMinigame()
    {
        playerVelocity = Vector3.zero;
        InputManager.ToggleActionMap(inputDataRef.Minigame);
    }
}
