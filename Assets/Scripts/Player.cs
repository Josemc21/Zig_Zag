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
    private float speedForward;
    private float speedSide;
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

        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            speedForward = 7.0f;
            speedSide = 5.0f;
        } 
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            speedForward = 8.0f;
            speedSide = 5.7f;
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            speedForward = 10.0f;
            speedSide = 7.1f;
        }
    }

    void Update()
    {
        camera.transform.position = transform.position + offSet;

        // --------------------- Player Movement Control --------------------- // 
        if (!isGoingRight)
        {
            // Forward Path Active 
            transform.Translate(Vector3.forward * speedForward * Time.deltaTime, Space.Self);

            // Side Movement Control
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * Time.deltaTime * speedSide, Space.Self);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.left * Time.deltaTime * speedSide * -1, Space.Self);
            }
        } else {
            // Side Path Active
            transform.Translate(Vector3.left * speedForward * Time.deltaTime * -1, Space.Self);

            // Side Movement Control
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speedSide, Space.Self);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speedSide * -1, Space.Self);
            }

        }

        // Jump Input Control
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && playerGrounded)
        {   
            rb.AddForce(new Vector3(0, 6, 0), ForceMode.Impulse);
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

        // Player Rotation on Turn Fix
        if (transform.localRotation.x != 0 || transform.localRotation.y != 0 || transform.localRotation.z != 0)
        {
            this.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
            Debug.Log("Rotacion ajustada");
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
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            if (TotalLives == 0 && !GameWonScreen.gameObject.activeSelf) 
            { 
                GameOverScreen.SetActive(true);  
                Invoke("DelayedLevel1Restart", 5f);    
            }
        } 
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            if (TotalLives == 0 && !GameWonScreen.gameObject.activeSelf) 
            { 
                GameOverScreen.SetActive(true);  
                Invoke("DelayedLevel2Restart", 5f);    
            }
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            if (TotalLives == 0 && !GameWonScreen.gameObject.activeSelf) 
            { 
                GameOverScreen.SetActive(true);  
                Invoke("DelayedLevel3Restart", 5f);    
            }
        }

        // Game Won Screen
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            if (TotalScore >= 1500 && !GameOverScreen.gameObject.activeSelf) 
            { 
                GameWonScreen.SetActive(true);  
                Invoke("DelayedLevel2Loader", 1.0f);    
            }
        } 
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            if (TotalScore >= 2000 && !GameOverScreen.gameObject.activeSelf) 
            { 
                GameWonScreen.SetActive(true);  
                Invoke("DelayedLevel3Loader", 1.0f);    
            }
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            if (TotalScore >= 3000 && !GameOverScreen.gameObject.activeSelf) 
            { 
                GameWonScreen.SetActive(true);  
                Invoke("DelayedMenuLoader", 1.0f);    
            }
        }

        // Score Increaser
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            if (!GameOverScreen.activeSelf && TotalScore < 1500 && !GameWonScreen.activeSelf) 
            {
                TotalScore += 1.0f * Time.fixedDeltaTime;
            }
            Score.text = "" + ((int)TotalScore);
        } 
        else if (SceneManager.GetActiveScene().name == "Level 2")
        {
            if (!GameOverScreen.activeSelf && TotalScore < 2000 && !GameWonScreen.activeSelf) 
            {
                TotalScore += 1.0f * Time.fixedDeltaTime;
            }
            Score.text = "" + ((int)TotalScore);
        }
        else if (SceneManager.GetActiveScene().name == "Level 3")
        {
            if (!GameOverScreen.activeSelf && TotalScore < 3000 && !GameWonScreen.activeSelf) 
            {
                TotalScore += 1.0f * Time.fixedDeltaTime;
            }
            Score.text = "" + ((int)TotalScore);
        }  
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

    // Trigger Behaviours
    void OnTriggerEnter(Collider other) 
    { 
        if(other.gameObject.CompareTag("Moneda")) 
        { 
            TotalScore += 20; 
        }

        if (other.gameObject.CompareTag("Obstacle"))
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
    }

    void DelayedLevel2Loader() { SceneManager.LoadScene("Level 2"); }
    void DelayedLevel3Loader() { SceneManager.LoadScene("Level 3"); }
    void DelayedMenuLoader() { SceneManager.LoadScene("Main Menu"); }
    void DelayedLevel1Restart() { SceneManager.LoadScene("Level 1"); }
    void DelayedLevel2Restart() { SceneManager.LoadScene("Level 2"); }
    void DelayedLevel3Restart() { SceneManager.LoadScene("Level 3"); }
}
