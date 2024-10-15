using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    [SerializeField] private float moveSpeed;

    private void Update()
    {
        float inputAD;
        float inputWS;
        Vector2 inputWASD;
        inputAD = Input.GetAxis("Horizontal");
        inputWS = Input.GetAxis("Vertical");
        inputWASD = new Vector2(inputAD, inputWS);

        controller.Move(inputWASD * moveSpeed * Time.deltaTime);
    }
}

