using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Better not copy me because I will come to your house and take all your chicken tenders from your fridge.
// Also I have to split this script into seperate files because spaghetti code looks gross.

public class CharacterMovement : MonoBehaviour
{
    // Init Values
    public CharacterController Controller;
    public Transform CharacterCamera;
    public Text PointText;
    public Text WinText;
    public Text LoseText;
    public Text TimerText;

    [SerializeField]
    public MeshFilter CharacterMF;
    [SerializeField]
    public Mesh CharacterMesh;

    private Rigidbody CharacterRB;

    [SerializeField]
    private int CharacterPoints;
    float HorizontalVector;
    float VerticalVector;

    [SerializeField]
    public bool bIsGrounded = true;
    [SerializeField]
    public float Speed = 1000f;
    [SerializeField]
    public float JumpMagnitude = 0.0001f;

    public bool bJumping;

    // Jumping disabled b/c feature unfinished 
    public bool bJumpToggle = false;
    private bool bTimer = true;
    private bool bWon = false;
    [SerializeField]
    private float TimeRemain;

    void Start() {
        CharacterRB = GetComponent<Rigidbody>();
        Controller = GetComponent<CharacterController>();
        CharacterMF = GetComponent<MeshFilter>();

        // Timer
        bTimer = true;

            // Set Active Super Scuffed lol
        LoseText.gameObject.SetActive(false);

        // Hide Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        CharacterPoints = 0;
        WinText.text = "";
        SetPointText();
    }

    // Update is called once per frame
    void Update()
    {
        if (bTimer)
        {
            if (TimeRemain > 0)
            {
                TimeRemain -= Time.deltaTime;
            }
            else
            {
                bTimer = false;
                LoseText.text = "You Lose!";
                TimeRemain = 0;
                // Set Active Super Scuffed lol
                if (bWon != true)
                {
                    LoseText.gameObject.SetActive(true);    
                }
            }
        }
        TimerText.text = "Time Left: "+ Mathf.FloorToInt(TimeRemain % 60).ToString();
    }

    // FixedUpdate called on physics 
    void FixedUpdate()
    {
        // Retrive the Axis of the character
        HorizontalVector = Input.GetAxisRaw("Horizontal");
        VerticalVector = Input.GetAxisRaw("Vertical");

        bJumping = Input.GetButton("Jump");

        // Calculate the vector in which the character is facing. Normalized to have the vector a length of 1.
        Vector3 CharacterDirection = new Vector3(HorizontalVector, 0f, VerticalVector).normalized;
        Vector3 CharacterVerticalDirection = new Vector3(0f, 1f, 0f).normalized;

        // Info on quaternions: -> https://www.youtube.com/watch?v=zjMuIxRvygQ
        float CamCharacter = Mathf.Atan2(CharacterDirection.x, CharacterDirection.z) * Mathf.Rad2Deg + CharacterCamera.eulerAngles.y;
        Vector3 CharCameraDirection = Quaternion.Euler(0f, CamCharacter, 0f) * Vector3.forward;

        // Only apply the force when the a key is pressed down
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            // TODO: Unsure on why the vector's length isn't changed
            CharacterRB.AddForce(CharCameraDirection.normalized * Speed, ForceMode.Force);
        }
        else if (bJumping)
        {
            // TODO: Fix this jump mechanic...
            Jump();
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            other.gameObject.SetActive(false);
            CharacterPoints += 1;
            SetPointText();

            // Check if 4 points.
            if (CharacterPoints == 4)
            {
                GameObject Door = GameObject.FindGameObjectWithTag("Door");
                Door.SetActive(false);
            }
        }
    }

    void SetPointText()
    {
        PointText.text = "Points: " + CharacterPoints.ToString();
        if (CharacterPoints >= 12 && (bTimer == true))
        {
            bWon = true;
            WinText.text = "You Win!";
            CharacterMF.mesh = CharacterMesh;
        }
    }

    private void Jump()
    {
        if (bJumpToggle) 
        {
            bJumpToggle = false;

            //Add jump forces
            CharacterRB.AddForce(Vector2.up.normalized * JumpMagnitude);
            CharacterRB.AddForce(Vector3.up.normalized * JumpMagnitude);
            
            ResetJump();
        }
    }

    // Reset Jump
    private void ResetJump() 
    {
        bJumpToggle = true;
    }
}

/* I am not fixing jump because I'm lazy and want to do other projects :) */