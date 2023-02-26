using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterMotor : MonoBehaviour
{
    private PlayerController playerController;
    public AnimatorHandler animatorHandler;
    public Vector3 input;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void TickInput(Vector2 movementInput)
    {
        HandleInput(movementInput);
        HandleRotation();
    }

    public void HandleInput(Vector2 inpt)
    {
        input = new Vector3(inpt.x, 0, inpt.y);
    }

    public void HandleRotation()
    {
        Vector3 relativePos = MouseController.WorldMousePosition - transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);

        rotation.Normalize();
        rotation.x = 0;
        rotation.z = 0;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, playerController.turnSpeed * Time.deltaTime);
    }

    public void Move()
    {
        float speed = playerController.speed;

        if (input != Vector3.zero)
        {
            if (input == Vector3.forward)
                playerController.rb.MovePosition(transform.position + transform.forward * input.normalized.magnitude * speed * Time.deltaTime);
            if (input == Vector3.right)
                playerController.rb.MovePosition(transform.position + transform.right * input.normalized.magnitude * speed * 0.8f * Time.deltaTime);
            if (input == Vector3.left)
                playerController.rb.MovePosition(transform.position - transform.right * input.normalized.magnitude * speed * 0.8f * Time.deltaTime);
            if (input == Vector3.back)
                playerController.rb.MovePosition(transform.position - transform.forward * input.normalized.magnitude * speed * 0.7f * Time.deltaTime);

            animatorHandler.UpdateAnimatorValues(1, 0);
        }
        else
        {
            animatorHandler.UpdateAnimatorValues(0, 0);
        }
    }
}
