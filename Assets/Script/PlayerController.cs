using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private MochiGameManager mochiGameManager;

    private Vector2 mousePosition = default;
    private Vector3 mousePos;
    void Start()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
        GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        mochiGameManager = obj.GetComponent<MochiGameManager>();
    }

    void Update()
    {
        

    }

    private void FixedUpdate()
    {
        if (mochiGameManager.gameMode == MochiGameManager.GameMode.Play)
        {
            mousePosition = playerInput.actions["Mouse"].ReadValue<Vector2>();
            mousePos = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePos.z = 0f;
            transform.position = mousePos;
        }
    }
}
