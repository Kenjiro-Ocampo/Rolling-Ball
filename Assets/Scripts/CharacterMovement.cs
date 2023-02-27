using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Init Values
    public float Speed = 1000f;
    public CharacterController Controller;
    public Transform CharacterCamera;
    private Rigidbody CharacterRB;
    void Start() {
        CharacterRB = GetComponent<Rigidbody>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {}

    // FixedUpdate called on physics 
    void FixedUpdate()
    {
        // Retrive the Axis of the character
        float HorizontalVector = Input.GetAxisRaw("Horizontal");
        float VerticalVector = Input.GetAxisRaw("Vertical");

        // Calculate the vector in which the character is facing. Normalized to have the vector a length of 1.
        Vector3 CharacterDirection = new Vector3(HorizontalVector, 0f, VerticalVector).normalized;
        
        // Info on quaternions: -> https://www.youtube.com/watch?v=zjMuIxRvygQ
        float CamCharacter = Mathf.Atan2(CharacterDirection.x, CharacterDirection.z) * Mathf.Rad2Deg + CharacterCamera.eulerAngles.y;
        Vector3 CharCameraDirection = Quaternion.Euler(0f, CamCharacter, 0f) * Vector3.forward;

        // Only apply the force when the a key is pressed down
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            // TODO: Unsure on why the vector's length isn't changed
            CharacterRB.AddForce(CharCameraDirection.normalized * Speed);
        }

        /*
        // Check is the length of the vector is positive and in a valiud direction.
        if (CharacterDirection.magnitude >= 0.1f)
        {
            
            Controller.Move(CharacterDirection * Speed);
        }
        */
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            other.gameObject.SetActive(false);
        }
    }
}