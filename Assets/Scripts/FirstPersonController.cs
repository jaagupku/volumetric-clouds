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

    private float verticalVelocity = 0;

	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();
        rotate = true;
        Cursor.lockState = CursorLockMode.Locked;
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

        if (rotate)
        {
            float rotX = Input.GetAxis("Mouse X");
            verticalRotation -= Input.GetAxis("Mouse Y");
            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);

            Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

            transform.Rotate(0, rotX, 0);
        }

        if (cc.isGrounded)
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

        if (Input.GetKey("c"))
        {
            cc.height = 0.2f;
        }
        else
        {
            cc.height = 2;
        }

        currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = 2.2f * speed;
            //Camera.main.fieldOfView = 100;
        }
        else
        {
            //Camera.main.fieldOfView = 80;
        }

        Vector3 movement = transform.rotation * new Vector3(horizontal, verticalVelocity, vertical);
        cc.Move(movement * currentSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = rotate ? CursorLockMode.None : CursorLockMode.Locked;
            rotate = !rotate;
        }
    }
}
