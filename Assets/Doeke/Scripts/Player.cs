using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    float sensitivity = 200f;
    float x_rotation = 0f;

    Camera cam;

    public CharacterController controller;

    float speed = 12f;

    float gravity = -9.81f; 

    Vector3 velocity;

    public Transform ground_check;
    float ground_distance = 0.4f;
    public LayerMask ground_mask;
    bool is_grounded;

    
    void Start() {
        cam = this.gameObject.transform.GetChild(0).GetComponent<Camera>();
        
        Cursor.lockState = CursorLockMode.Locked;

    }


    void Update() {

        is_grounded = Physics.CheckSphere(ground_check.position, ground_distance, ground_mask);

        if (is_grounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        //
        // rotation
        //
        float h = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float v = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;
        
        x_rotation -= v;
        x_rotation = Mathf.Clamp(x_rotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(x_rotation, 0f, 0f);
        transform.Rotate(Vector3.up * h);

        //
        // movement
        //
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

    }


    
}
