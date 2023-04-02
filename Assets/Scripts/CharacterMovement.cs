using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Better not copy me because I will come to your house and take all your chicken tenders from your fridge.
// Also I have to split this script into seperate files because spaghetti code looks gross.

public class CharacterMovement : MonoBehaviour
{
    // Init Values
    public CharacterController Controller;
    public Transform CharacterCamera;

    //public Text PointText;
    //public Text WinText;
    //public Text LoseText;
    //public Text TimerText;

    public TextMeshProUGUI TMPointText;
    public TextMeshProUGUI TMWinText;
    public TextMeshProUGUI TMLoseText;
    public TextMeshProUGUI TMTimerText;

    [SerializeField] public MeshFilter CharacterMF;
    [SerializeField] public Mesh CharacterMesh;

    private Rigidbody CharacterRB;

    [SerializeField] private int CharacterPoints;
    private float HorizontalVector;
    private float VerticalVector;

    [SerializeField] public bool bIsGrounded = false;
    [SerializeField] public float Speed = 1000f;
    [SerializeField] public float JumpMagnitude = 5.0f;

    private bool bTimer = true;
    private bool bWon = false;

    [SerializeField] private float TimeRemain;

    [SerializeField] private bool bIsGroundPound = false;

    // [SerializeField] private bool CameraLock = true;
    // private Cinemachine.CinemachineFreeLook CinemachineCamera;

    void Start() {
        CharacterRB = GetComponent<Rigidbody>();
        Controller = GetComponent<CharacterController>();
        CharacterMF = GetComponent<MeshFilter>();
        //CinemachineCamera = GetComponent<Cinemachine.CinemachineFreeLook>();
        
        // Timer
        bTimer = true;

            // Set Active Super Scuffed lol
        //LoseText.gameObject.SetActive(false);
        TMLoseText.gameObject.SetActive(false);

        // Hide Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        CharacterPoints = 0;
        TMWinText.SetText("");
        //WinText.text = "";
        SetPointText();
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            bIsGrounded = false;

            CharacterRB.AddForce(Vector3.up.normalized * JumpMagnitude, ForceMode.Impulse);
        }
        else if (!bIsGrounded && Input.GetKeyDown(KeyCode.Space)) /*  Ground Pound  */
        {
            CharacterRB.AddForce(Vector3.down.normalized * JumpMagnitude * 2, ForceMode.Impulse);
            bIsGroundPound = true;
        }

        //      Lock Camera Work In Progress
/*
        if (Input.GetKeyDown(KeyCode.Tab) && CameraLock == true)
        {
            CinemachineCamera.m_YAxis.m_InputAxisValue = 0;
            CinemachineCamera.m_XAxis.m_InputAxisValue = 0;
            CameraLock = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && CameraLock == false)
        {
            CinemachineCamera.m_YAxis.m_InputAxisValue = 6;
            CinemachineCamera.m_XAxis.m_InputAxisValue = 325;
            CameraLock = true;
        }
*/
        if (bTimer)
        {
            if (TimeRemain > 0)
            {
                TimeRemain -= Time.deltaTime;
            }
            else
            {
                bTimer = false;
                //LoseText.text = "You Lose!";
                TMLoseText.SetText("You Lose!");
                TimeRemain = 0;

                // Set Active Super Scuffed lol
                if (bWon != true)
                {
                    //LoseText.gameObject.SetActive(true);
                    TMLoseText.gameObject.SetActive(true);  
                }
            }
        }
        //TimerText.text = "Time Left: "+ Mathf.FloorToInt(TimeRemain % 60).ToString();
        TMTimerText.text = "Time Left: "+ Mathf.FloorToInt(TimeRemain).ToString();
    }

    // FixedUpdate called on physics 
    void FixedUpdate()
    {
        // Retrive the Axis of the character
        HorizontalVector = Input.GetAxisRaw("Horizontal");
        VerticalVector = Input.GetAxisRaw("Vertical");

        // Calculate the vector in which the character is facing. Normalized to have the vector a length of 1.
        Vector3 CharacterDirection = new Vector3(HorizontalVector, 0f, VerticalVector).normalized;
        Vector3 CharacterVerticalDirection = new Vector3(0f, 1f, 0f).normalized;

        // Info on quaternions: -> https://www.youtube.com/watch?v=zjMuIxRvygQ
        float CamCharacter = Mathf.Atan2(CharacterDirection.x, CharacterDirection.z) * Mathf.Rad2Deg + CharacterCamera.eulerAngles.y;
        Vector3 CharCameraDirection = Quaternion.Euler(0f, CamCharacter, 0f) * Vector3.forward;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            // TODO: Unsure on why the vector's length isn't changed
            CharacterRB.AddForce(CharCameraDirection.normalized * Speed, ForceMode.Force);
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
        //PointText.text = "Points: " + CharacterPoints.ToString();
        TMPointText.text = "Points: " + CharacterPoints.ToString();
        if (CharacterPoints >= 12 && (bTimer == true))
        {
            bWon = true;
            //WinText.text = "You Win!";
            TMWinText.text = "You Win!";
            CharacterMF.mesh = CharacterMesh;
        }
    }

    // Reset Jump & Ground Pound Logic
    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.CompareTag("Breakable") && bIsGroundPound)
        {
            other.gameObject.SetActive(false);
        }

        if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Breakable"))
        {
            bIsGrounded = true;
            bIsGroundPound = false;
        }

    }
}