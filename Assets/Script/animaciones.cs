using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animaciones : MonoBehaviour
{
    public Animator animator;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float turnSpeed = 200f;

    private float moveInput;
    private float turnInput;
    private bool isRunning;

    void Update()
    {
        // Obtener la entrada del teclado
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // Calcular la velocidad basada en si está corriendo o caminando
        float speed = isRunning ? runSpeed : walkSpeed;
        transform.Translate(Vector3.forward * moveInput * speed * Time.deltaTime);

        // Girar al personaje
        transform.Rotate(Vector3.up, turnInput * turnSpeed * Time.deltaTime);

        // Llamar a las animaciones según el estado del personaje
        if (moveInput != 0)
        {
            if (isRunning)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
            }
            else
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }

        // Girar a la izquierda o derecha
        if (turnInput != 0)
        {
            animator.SetBool("isTurning", true);
        }
        else
        {
            animator.SetBool("isTurning", false);
        }

        // Estado idle
        if (moveInput == 0 && turnInput == 0)
        {
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }
    }
}