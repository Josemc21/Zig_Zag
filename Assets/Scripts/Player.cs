using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Global Variables & Objects
    private Vector3 offSet;
    public new Camera camera;
    //private int TotalMonedas = 0;

    // Player Variables & Objects
    private float speedForward = 7f;
    private float speedSide = 7f;
    private float xValue, zValue;
    private bool isGoingRight = false;
    private bool playerGrounded = false;
    public Rigidbody rb;
    public AudioClip sonidoMoneda; // Sonido de la moneda
    private AudioSource audioSource; //Componente Audio Source adjunto al objeto de la moneda

    //Floor Variables & Objects
    public GameObject Floor;
    private float lastJumpObstacleGenerationTime = 0.0f;
    private float lastObstacleGenerationTime = 0.0f;
    
    void Start()
    {
        offSet = camera.transform.position;
        rb = GetComponent<Rigidbody>();
        InitialFloor();

        // Obtener la referencia al componente Audio Source
        audioSource = GetComponent<AudioSource>();

        // Asignar el clip de sonido al Audio Source
        audioSource.clip = sonidoMoneda;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Moneda"))
        {
            //TotalMonedas++;
            //Contador.text = "Monedas = " + TotalMonedas";

            // Reproducir el sonido de la moneda
            audioSource.Play();
            other.gameObject.SetActive(false);
            /*if (TotalMonedas == 11)
            {
               
            }*/
        }
    }

    IEnumerator DestroyFloor(GameObject Floor)
    {
        float random = Random.Range(0.0f, 1.0f);
        if (random > 0.5f)
        {
            xValue += 5.0f;
            GeneratePotentialJumpObstacle(random);   // Potencial obstaculo de salto
            GeneratePotentialRandomWalls(Instantiate(Floor, new Vector3(xValue, 0, zValue), Quaternion.identity), false); // Generar paredes aleatorias
        }
        else
        {
            zValue += 5.0f;
            GeneratePotentialJumpObstacle(random);   // Potencial obstaculo de salto
            GeneratePotentialRandomWalls(Instantiate(Floor, new Vector3(xValue, 0, zValue), Quaternion.identity), true); // Generar paredes aleatorias
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

    void GeneratePotentialRandomWalls(GameObject floor, bool Turn)
    {
        // Desactivar todas las paredes primero
        if (floor != null)
        {
            DeactivateAllWalls(floor);
            DeactivateAllCoins(floor);
        }

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
            else 
            {
                if(Turn) ActivatePotentialCoin(floor); // Activar moneda
                else ActivateTurnPotentialCoin(floor); // Activar moneda en suelo yendo a dcha
            }

            // Actualiza el tiempo de la última generación de obstáculos
            lastObstacleGenerationTime = Time.time;
        }
    }


    void ActivateRandomWall(GameObject floor)
    {
        // Generar un número aleatorio entre 0 y 3 para seleccionar una pared al azar
        int randomIndex = Mathf.RoundToInt(Random.Range(0, 7f));

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
            case 3:
                floor.transform.Find("LongWall").gameObject.SetActive(true);
                break;
            case 4:
                floor.transform.Find("TallWallLeft").gameObject.SetActive(true);
                break;
            case 5:
                floor.transform.Find("TallWallCenter").gameObject.SetActive(true);
                break;
            case 6:
                floor.transform.Find("TallWallRight").gameObject.SetActive(true);
                break;
        }
    }

    void ActivateTurnRandomWall(GameObject floor)
    {
        // Generar un número aleatorio entre 0 y 3 para seleccionar una pared al azar
        int randomIndex = Mathf.RoundToInt(Random.Range(0, 7f));

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
            case 3:
                floor.transform.Find("TurnLongWall").gameObject.SetActive(true);
                break;
            case 4:
                floor.transform.Find("TurnTallWallLeft").gameObject.SetActive(true);
                break;
            case 5:
                floor.transform.Find("TurnTallWallCenter").gameObject.SetActive(true);
                break;
            case 6:
                floor.transform.Find("TurnTallWallRight").gameObject.SetActive(true);
                break;
        }
    }

    void ActivateLeftAndRightWalls(GameObject floor)
    {
        // Probabilidad de activar una pared
        float chance = Random.Range(0.0f, 1.0f);

        if (chance <= 0.5f) // 50% de generar paredes normales
        {
            // Activar las paredes izquierda y derecha
            floor.transform.Find("WallLeft").gameObject.SetActive(true);
            floor.transform.Find("WallRight").gameObject.SetActive(true);
        }
        else // 50% de generar paredes altas
        {
            // Activar las paredes izquierda y derecha altas
            floor.transform.Find("TallWallLeft").gameObject.SetActive(true);
            floor.transform.Find("TallWallRight").gameObject.SetActive(true);
        }
    }

        

    void ActivateTurnLeftAndRightWalls(GameObject floor)
    {
        // Probabilidad de activar una pared
        float chance = Random.Range(0.0f, 1.0f);

        if (chance <= 0.5f) // 50% de generar paredes normales giradas
        {
            // Activar las paredes izquierda y derecha
            floor.transform.Find("TurnWallLeft").gameObject.SetActive(true);
            floor.transform.Find("TurnWallRight").gameObject.SetActive(true);
        }
        else // 50% de generar paredes altas giradas
        {
            // Activar las paredes izquierda y derecha altas
            floor.transform.Find("TurnTallWallLeft").gameObject.SetActive(true);
            floor.transform.Find("TurnTallWallRight").gameObject.SetActive(true);
        }
    }

    void ActivatePotentialCoin(GameObject floor)
    {
        // Generar un número aleatorio entre 0 y 3 para seleccionar una pared al azar
        int randomIndex = Mathf.RoundToInt(Random.Range(0, 3f));

        // Activar la pared seleccionada
        switch (randomIndex)
        {
            case 0:
                floor.transform.Find("CoinLeft").gameObject.SetActive(true);
                break;
            case 1:
                floor.transform.Find("CoinCenter").gameObject.SetActive(true);
                break;
            case 2:
                floor.transform.Find("CoinRight").gameObject.SetActive(true);
                break;
        }
    }

    void ActivateTurnPotentialCoin(GameObject floor)
    {
        // Generar un número aleatorio entre 0 y 3 para seleccionar una pared al azar
        int randomIndex = Mathf.RoundToInt(Random.Range(0, 3f));

        // Activar la pared seleccionada
        switch (randomIndex)
        {
            case 0:
                floor.transform.Find("TurnCoinLeft").gameObject.SetActive(true);
                break;
            case 1:
                floor.transform.Find("TurnCoinCenter").gameObject.SetActive(true);
                break;
            case 2:
                floor.transform.Find("TurnCoinRight").gameObject.SetActive(true);
                break;
        }
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
        floor.transform.Find("LongWall").gameObject.SetActive(false);
        floor.transform.Find("TurnLongWall").gameObject.SetActive(false);
        floor.transform.Find("TallWallLeft").gameObject.SetActive(false);
        floor.transform.Find("TallWallRight").gameObject.SetActive(false);
        floor.transform.Find("TallWallCenter").gameObject.SetActive(false);
        floor.transform.Find("TurnTallWallLeft").gameObject.SetActive(false);
        floor.transform.Find("TurnTallWallRight").gameObject.SetActive(false);
        floor.transform.Find("TurnTallWallCenter").gameObject.SetActive(false);
    }

    void DeactivateAllCoins(GameObject floor)
    {
        // Desactivar todas las paredes
        floor.transform.Find("CoinLeft").gameObject.SetActive(false);
        floor.transform.Find("CoinCenter").gameObject.SetActive(false);
        floor.transform.Find("CoinRight").gameObject.SetActive(false);
        floor.transform.Find("TurnCoinLeft").gameObject.SetActive(false);
        floor.transform.Find("TurnCoinCenter").gameObject.SetActive(false);
        floor.transform.Find("TurnCoinRight").gameObject.SetActive(false);
    }
}