using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Global Variables & Objects
    private Vector3 offSet;
    public new Camera camera;

    // Player Variables & Objects
    private float speedForward = 7f;
    private float speedSide = 7f;
    private float xValue, zValue;
    private bool isGoingRight = false;
    private bool playerGrounded = true;
    public Rigidbody rb; 

    // Player UI Values & Objects
    private int TotalLives = 10;
    private float TotalScore = 0;
    public Text Lives;
    public Text Score;
    public GameObject GameOverScreen;
    public GameObject GameWonScreen;

    // Floor Variables & Objects
    public GameObject Floor;
    public GameObject LastFloor;
    public GameObject CurrentFloor;
    
    
    void Start()
    {
        offSet = camera.transform.position;
        rb = GetComponent<Rigidbody>();
        InitialFloor();
    }

    void Update()
    {
        camera.transform.position = transform.position + offSet;

        // --------------------- Player Movement Control --------------------- // 
        if (!isGoingRight)
        {
            // Forward Path Active 
            transform.Translate(Vector3.forward * speedForward * Time.deltaTime);

            // Side Movement Control
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * Time.deltaTime * speedSide);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.left * Time.deltaTime * speedSide * -1);
            }
        } else {
            // Side Path Active
            transform.Translate(Vector3.left * speedForward * Time.deltaTime * -1);

            // Side Movement Control
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speedSide);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speedSide * -1);
            }
        }

        // Jump Input Control
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && playerGrounded)
        {   
            rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
            playerGrounded = false;
        }
        
        // Turn Input Control
        if (Input.GetKeyUp(KeyCode.Space)) { Turn(); }

        // Player Height on Turn Fix
        if (transform.position.y > 0.515f && playerGrounded) 
        { 
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            Debug.Log("Y fixed"); 
        }

        // --------------------- Player UI and Lives System --------------------- //
        // Player Restart On Death
        if (this.transform.position.y < -2) 
        {
            this.transform.position = new Vector3(LastFloor.transform.position.x, 0.515f, LastFloor.transform.position.z);
            if (TotalLives > 0) 
            { 
                TotalLives--;
                Lives.text = "LIVES = " + TotalLives;
            } 
            else { Lives.text = "LIVES = 💀"; }
            if (TotalScore - 100 < 0) { TotalScore = 0; } else { TotalScore -= 100; }
            
        }

        // GameOver Screen
        if (TotalLives == 0 && !GameWonScreen.gameObject.activeSelf) 
        { 
            GameOverScreen.SetActive(true);  
            Invoke("DelayedLevelRestart", 5f);    
        }

        // Game Won Screen
        if (TotalScore >= 1500 && !GameOverScreen.gameObject.activeSelf) 
        { 
            GameWonScreen.SetActive(true);  
            Invoke("DelayedLevelLoader", 5f);    
        }

        // Score Increaser
        if (!GameOverScreen.activeSelf || TotalScore < 300) 
        {
            TotalScore += 1.0f * Time.fixedDeltaTime;
        } 
        Score.text = "" + ((int)TotalScore);
    }

    void InitialFloor()
    {
        for (int i = 0; i < 5; i++)
        {
            zValue += 5.0f;
            Instantiate(Floor, new Vector3(xValue, 0, zValue), Quaternion.identity);
        }
    }

    void Turn()
    {
        if (isGoingRight) { isGoingRight = false; } else if (!isGoingRight) { isGoingRight = true; }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "ObstacleFloor")
        {
            playerGrounded = true;
        }
        else if (other.gameObject.tag == "Floor")
        {
            LastFloor = CurrentFloor.gameObject;
            CurrentFloor = other.gameObject;
            playerGrounded = true;
        }
    }

    // Score Up when touch Coin
    void OnTriggerEnter(Collider other) { if(other.gameObject.tag == "Moneda") { TotalScore += 20; } }
    void DelayedLevelLoader() { SceneManager.LoadScene("Main Menu"); }
    void DelayedLevelRestart() { SceneManager.LoadScene("Level 1"); }
}
