using System.Collections;
using UnityEngine;

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
    private bool playerGrounded = false;
    public Rigidbody rb;

    //Floor Variables & Objects
    public GameObject Floor;
    private float lastJumpObstacleGenerationTime = 0.0f;
    private float lastObstacleGenerationTime = 0.0f;
    
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
            GeneratePotentialJumpObstacle(random);   // Potencial obstaculo de salto
            GenerateRandomWalls(Instantiate(Floor, new Vector3(xValue, 0, zValue), Quaternion.identity), false); // Generar paredes aleatorias
        }
        else
        {
            zValue += 5.0f;
            GeneratePotentialJumpObstacle(random);   // Potencial obstaculo de salto
            GenerateRandomWalls(Instantiate(Floor, new Vector3(xValue, 0, zValue), Quaternion.identity), true); // Generar paredes aleatorias
        }
        
        yield return new WaitForSeconds(0.5f);
        Floor.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        Floor.gameObject.GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForSeconds(2.5f);
        Destroy(Floor);
    }


    void GeneratePotentialJumpObstacle(float random)
    {
        // Verifica si ha pasado al menos 3 segundos desde la última generación de obstáculos
        if (Time.time - lastJumpObstacleGenerationTime >= 3.0f)
        {
            if (random >= 0.8f) // 20% de generar un hueco
            {
                xValue += 3.0f;
            }
            else if (random <= 0.2f) // 20% de generar un hueco
            {
                zValue += 3.0f;
            }

            // Actualiza el tiempo de la última generación de obstáculos
            lastJumpObstacleGenerationTime = Time.time;
        }
    }

    void GenerateRandomWalls(GameObject floor, bool Turn)
    {
        // Desactivar todas las paredes primero
        DeactivateAllWalls(floor);

        // Probabilidad de activar una pared
        float wallActivationChance = Random.Range(0.0f, 1.0f);

        // Verifica si ha pasado al menos 1 segundos desde la última generación de obstáculos
        if (Time.time - lastObstacleGenerationTime >= 1.0f)
        {
            // Si el número aleatorio es menor que la probabilidad de activación de una sola pared
            if (wallActivationChance <= 0.3f && Turn) // 30% de generar una pared normal
            {
                // Activar solo una pared
                ActivateRandomWall(floor);
            }
            else if (wallActivationChance <= 0.3f && !Turn) // 30% de generar una pared girada
            {
                // Activar las paredes izquierda y derecha giradas
                ActivateTurnRandomWall(floor);
            }
            else if (wallActivationChance >= 0.7f && Turn) // 30% de generar dos paredes normales
            {
                // Activar las paredes izquierda y derecha normales
                ActivateLeftAndRightWalls(floor);
            }
            else if (wallActivationChance >= 0.7f && !Turn) // 30% de generar tres paredes giradas
            {
                // Activar las paredes izquierda y derecha giradas
                ActivateTurnLeftAndRightWalls(floor);
            }

            // Actualiza el tiempo de la última generación de obstáculos
            lastObstacleGenerationTime = Time.time;
        }
    }


    void ActivateRandomWall(GameObject floor)
    {
        // Generar un número aleatorio entre 0 y 3 para seleccionar una pared al azar
        int randomIndex = Mathf.RoundToInt(Random.Range(0, 3f));

        // Activar la pared seleccionada
        switch (randomIndex)
        {
            case 0:
                floor.transform.Find("WallLeft").gameObject.SetActive(true);
                break;
            case 1:
                floor.transform.Find("WallCenter").gameObject.SetActive(true);
                break;
            case 2:
                floor.transform.Find("WallRight").gameObject.SetActive(true);
                break;
        }
    }

    void ActivateTurnRandomWall(GameObject floor)
    {
        // Generar un número aleatorio entre 0 y 3 para seleccionar una pared al azar
        int randomIndex = Mathf.RoundToInt(Random.Range(0, 3f));

        // Activar la pared seleccionada
        switch (randomIndex)
        {
            case 0:
                floor.transform.Find("TurnWallLeft").gameObject.SetActive(true);
                break;
            case 1:
                floor.transform.Find("TurnWallCenter").gameObject.SetActive(true);
                break;
            case 2:
                floor.transform.Find("TurnWallRight").gameObject.SetActive(true);
                break;
        }
    }

    void ActivateLeftAndRightWalls(GameObject floor)
    {
        // Activar las paredes izquierda y derecha
        floor.transform.Find("WallLeft").gameObject.SetActive(true);
        floor.transform.Find("WallRight").gameObject.SetActive(true);
    }

    void ActivateTurnLeftAndRightWalls(GameObject floor)
    {
        // Activar las paredes izquierda y derecha
        floor.transform.Find("TurnWallLeft").gameObject.SetActive(true);
        floor.transform.Find("TurnWallRight").gameObject.SetActive(true);
    }

    void DeactivateAllWalls(GameObject floor)
    {
        // Desactivar todas las paredes
        floor.transform.Find("WallLeft").gameObject.SetActive(false);
        floor.transform.Find("WallCenter").gameObject.SetActive(false);
        floor.transform.Find("WallRight").gameObject.SetActive(false);
        floor.transform.Find("TurnWallLeft").gameObject.SetActive(false);
        floor.transform.Find("TurnWallCenter").gameObject.SetActive(false);
        floor.transform.Find("TurnWallRight").gameObject.SetActive(false);
    }
}