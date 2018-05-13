using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {

    public float speed;
    public float upDownRange;
    public float jumpSpeed;

    private float verticalRotation = 0;
    private CharacterController cc;
    private bool rotate;
    private float currentSpeed;
    private bool flying = false;

    private float verticalVelocity = 0;

	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();
        rotate = true;
        SetCursorState(CursorLockMode.Locked);
    }

    // Apply requested cursor state
    private void SetCursorState(CursorLockMode wantedMode)
    {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        float pushPower = currentSpeed * 0.5f;
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }

    // Update is called once per frame
    void Update () {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        currentSpeed = speed;

        if (rotate)
        {
            float rotX = Input.GetAxis("Mouse X");
            verticalRotation -= Input.GetAxis("Mouse Y");
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

            Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

            transform.Rotate(0, rotX, 0);
        }

        if (!flying)
        {
            if (cc.isGrounded) // Jumping
            {
                verticalVelocity = 0;
                if (Input.GetButton("Jump"))
                {
                    verticalVelocity = jumpSpeed;
                }
            }
            else
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }
        } else
        {
            //float a = Mathf.Clamp(cc.transform.position.y / 2000.0f, 0.2f, 10.0f);
            currentSpeed = speed * 300.0f;
            if (Input.GetKey("e"))
            {
                verticalVelocity = 1.0f;
            }
            else if (Input.GetKey("q"))
            {
                verticalVelocity = -1.0f;
            }
            else
            {
                verticalVelocity = 0.0f;
            }
        }

        if (Input.GetKey(KeyCode.LeftControl)) // Crouching
        {
            cc.height = 0.2f;
            currentSpeed *= 0.25f;
        }
        else
        {
            cc.height = 2;
        }
        
        if (Input.GetKey(KeyCode.LeftShift)) // Sprinting
        {
            currentSpeed *= 2.2f;
        }

        if (Input.GetKeyDown("f")) // Toggle Flying
        {
            flying = !flying;
        }

        Vector3 movement = transform.rotation * new Vector3(horizontal, verticalVelocity, vertical);
        cc.Move(movement * currentSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            rotate = !rotate;
            SetCursorState(rotate ? CursorLockMode.Locked : CursorLockMode.None);
        }
    }
}
