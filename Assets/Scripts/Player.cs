using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Global Variables & Objects
    private Vector3 offSet;
    public Camera camera;

    // Player Variables & Objects
    public float speed;
    private float xValue, zValue;
    private Vector3 Direction;
    private bool isGoingRight = false;
    public GameObject PlayerPositionLeft;
    public GameObject PlayerPositionRight;
    public GameObject PlayerPositionUp;
    public GameObject PlayerPositionDown;
    public GameObject PlayerPositionCenter;

    //Floor Variables & Objects
    public GameObject Floor;
    
    void Start()
    {
        offSet = camera.transform.position;
        InitialFloor();
        Direction = Vector3.forward;
    }

    void Update()
    {
        camera.transform.position = transform.position + offSet;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Turn();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            MovePlayerLane("Left");
        }
        if (Input.GetKeyUp(KeyCode.RightArrow)) {
            MovePlayerLane("Right");
        }
        if (Input.GetKeyUp(KeyCode.UpArrow)) {
            //Jump();
        }

        transform.Translate(Direction * speed * Time.deltaTime);
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
        if (Direction == Vector3.forward)
        {
            Direction = Vector3.right;
            isGoingRight = true;
        }
        else 
        {
            Direction = Vector3.forward;
            isGoingRight = false;
        }
    }

    void MovePlayerLane(string move)
    {
        if (move == "Left")
        {
            
        } else {
            //Comportamiento yendo a la derecha
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            StartCoroutine(DestroyFloor(other.gameObject));
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
