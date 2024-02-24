using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Global Variables & Objects
    private Vector3 offSet;
    public Camera camera;

    // Player Variables & Objects
    private float speedForward = 7f;
    private float speedSide = 7f;
    private float xValue, zValue;
    private bool isGoingRight = false;
    private bool playerGrounded = false;
    public Rigidbody rb; 

    //Floor Variables & Objects
    public GameObject Floor;
    
    void Start()
    {
        offSet = camera.transform.position;
        rb = GetComponent<Rigidbody>();
        InitialFloor();
    }

    void Update()
    {
        camera.transform.position = transform.position + offSet;

        // Player Movement Control
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
    }

    void InitialFloor()
    {
        for (int i = 0; i < 3; i++)
        {
            zValue += 5.0f;
            Instantiate(Floor, new Vector3(xValue, 0, zValue), Quaternion.identity);
        }
    }

    void Turn()
    {
        if (isGoingRight) { isGoingRight = false; } else if (!isGoingRight) { isGoingRight = true; }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            StartCoroutine(DestroyFloor(other.gameObject));
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            playerGrounded = true;
        }
    }

    IEnumerator DestroyFloor(GameObject Floor)
    {
        float random = Random.Range(0.0f, 1.0f);
        if (random > 0.5f)
        {
            xValue += 5.0f;
            yield return new WaitForSeconds(0.3f);
            Instantiate(Floor, new Vector3(xValue, 0, zValue), Quaternion.identity);
        } else {
            zValue += 5.0f;
            yield return new WaitForSeconds(0.3f);
            Instantiate(Floor, new Vector3(xValue, 0, zValue), Quaternion.identity);
        }
        
        yield return new WaitForSeconds(0.5f);
        Floor.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        Floor.gameObject.GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(2.5f);
        Destroy(Floor);
    }
}
