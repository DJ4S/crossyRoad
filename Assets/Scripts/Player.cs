
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
Script asociado al player
Uso de patron Command para gestionar movimiento
Uso de patron Facade para las actualizaciones de puntuacion y finalizacion del juego
Sitema de inputs para gestionar los movimientos del jugador con las teclas
*/
/*
// interfaz command para ejecutar comando
public interface ICommand
{
    void Execute();
}

// interfaz strategy para obtener un vector de direction
public interface IMoveStrategy
{
    Vector3 GetDirection();
}

public class Player : MonoBehaviour
{
    [SerializeField] private TerrainGenerator terrainGenerator; // refrencia al objeto con el script de generador de terreno
    [SerializeField] private TextMeshProUGUI scoreText; // refrencia al texto del juego con el score
    [SerializeField] private float raycastDistance = 1f; // distancia a verificar por el raycast
    [SerializeField] private LayerMask obstacleLayer; // definicion del tag de los obstaculos
    [SerializeField] private LayerMask waterLayer; // definicion del tag del agua
    [SerializeField] private EndGameManager endGameManager; // referencia al manejadior de fin de juego
    
    public float boundaryZ = 19.5f; // distancia de los limites del mapa
    private Animator animator; // elemento que gestionara las animaciones
    private bool isHopping; // para saber cuando esta haciendo la animacion
    private int score; // valor del score

    private IMoveStrategy moveStrategy;
    private ICommand moveCommand;
    private GameFacade gameFacade;

    private void Start() // al iniciar el juego
    {
        animator = GetComponent<Animator>(); // se obtiene el animator adjunto al player
        gameFacade = new GameFacade(scoreText, endGameManager); // inicializa la facade con el texto referenciado y el canvas de fin de juego
    }

    private void OnCollisionEnter(Collision collision) // se ejecuta en colision
    {
        if (collision.collider.GetComponent<Vehicle>() != null) // si el objeto colisionado tiene componente Vehiculo (tablones y coches)
        {
            if (collision.collider.GetComponent<Vehicle>().isLog) // si es un tablon de mandera
            {
                transform.parent = collision.collider.transform; // el player se convierte en hijo del tablon para seguir su flujo
            }
            else // si es un coche
            {
                gameFacade.EndGame(score); // llamada a final de juego
            }
        }
        else if (collision.collider.CompareTag("Agua")) // si no es un vehiculo pero es agua
        {
            gameFacade.EndGame(score); // tambien se llama a final del juego
        }
        else
        {
            transform.parent = null; // si no, el jugador se desasocia del padre (deja de estar en log)
        }
    }

    private void Update() // durante el juego
    {
        gameFacade.UpdateScore(score); // actualizamos el score de la pantalla 

        if (Input.GetKeyDown(KeyCode.W) && !isHopping) // si no esta saltando y se detecta tecla
        {
            Move(new MoveForward()); // llamamos a la funcion para movernos
        }
        else if (Input.GetKeyDown(KeyCode.A) && !isHopping)
        {
            Move(new MoveLeft());
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isHopping)
        {
            Move(new MoveRight());
        }
        if (Mathf.Abs(transform.position.z) > boundaryZ) // si el player supera el limite
        {
            // se destruye y llama a fin de juego
            Destroy(gameObject);
            gameFacade.EndGame(score);
        }
    }

    private void Move(IMoveStrategy strategy) 
    {
        moveStrategy = strategy; // guardamos el movimiento recibido
        // crea nueva instancia de MoveCommand que es la que aplica el movimiento
        moveCommand = new MoveCommand(this, moveStrategy.GetDirection());
        moveCommand.Execute(); // se ejecuta la instancia nueva
    }

    public void TryMove(Vector3 direction)
    {
        if (CanMove(direction)) // verifica que puede moverse
        {
            MoveCharacter(direction);
            if (direction == Vector3.right) score++;
        }
    }

    private bool CanMove(Vector3 direction) // verifica si hay obstaculo o puede moverse
    {
        Ray ray = new Ray(transform.position, direction);  // crea un nuevo raycast en posicion del jugador y la direccion dle moviemiento
        RaycastHit hit; // variable para almacenar que se encuentra el rayo
        // verifica si el rayo se encuentra en la distancia establecida un objeto del layer obstaculo
        if (Physics.Raycast(ray, out hit, raycastDistance, obstacleLayer))
        {
            // si hay obstaculo, no puede moverse
            return false;
        }
        // si no hay obstaculo, se puede mover
        return true;
    }

    private void MoveCharacter(Vector3 direction)
    {
        animator.SetTrigger("hop"); // activa la animacion del jugador al moverse
        isHopping = true; // especifica que esta saltando
        // inicia corrutina para manejar movimiento
        StartCoroutine(MoveCoroutine(direction));
    }

    private IEnumerator MoveCoroutine(Vector3 direction)
    {
        Vector3 startPosition = transform.position; // posicion del jugador
        Vector3 endPosition = startPosition + direction; // posicion donde debe acabar el jugador 
        float moveDuration = 0.2f; // establece lo que tarde el jugador en completar el movimiento
        float elapsedTime = 0f; // variable que acumula el tiempo transcurrido

        while (elapsedTime < moveDuration) // hasta que haya pasado el tiempo indicado
        {
            // movemos el personaje
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / moveDuration));
            // actualizamos el tiempo transcurrido
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // la posicion del jugador recibe endPosition 
        transform.position = endPosition;
        // se llama a la generacion de terreno en funcion de la posicion actual del jugador
        terrainGenerator.SpawnTerrain(false, transform.position);
        // ya no esta haciendo la animacion
        isHopping = false;
    }

    public void FinishHop()
    {
        isHopping = false;
    }
}

// Clase para ejecutar el movimiento del jugador
public class MoveCommand : ICommand
{
    private Player player;
    private Vector3 direction;
        
    // constructor asigna valores a varibales
    public MoveCommand(Player player, Vector3 direction)
    {
        this.player = player;
        this.direction = direction;
    }

    // llama a la siguiente funcion de movimiento del jugador
    public void Execute()
    {
        player.TryMove(direction);
    }
}

// Clases para definir los tres movimientos (player tine rotacion de Y:90 por visual)
public class MoveForward : IMoveStrategy 
{
    public Vector3 GetDirection()
    {
        return Vector3.right;
    }
}

public class MoveLeft : IMoveStrategy
{
    public Vector3 GetDirection()
    {
        return Vector3.forward;
    }
}

public class MoveRight : IMoveStrategy
{
    public Vector3 GetDirection()
    {
        return Vector3.back;
    }
}

// Clase para facilitar la interaccion con el usuario y gestion del fin de juego
public class GameFacade
{
    private TextMeshProUGUI scoreText;
    private EndGameManager endGameManager;

    public GameFacade(TextMeshProUGUI scoreText, EndGameManager endGameManager)
    {
        this.scoreText = scoreText; // referencia al texto para mostrar la puntuacion
        this.endGameManager = endGameManager; // referencia al gestor de fin de juego
    }

    public void UpdateScore(int score) // actualiza el texto del score con el nuevo valor
    {
        scoreText.text = "Score: " + score;
    }

    public void EndGame(int score) // en caso de que termine  se llama al gestor de fin del juego pasando un score
    {
        endGameManager.ShowEndGameScreen(score);
    }
}
*/

/*
Script asociado al player
Uso de patron Command para gestionar movimiento
Uso de patron Facade para las actualizaciones de puntuacion y finalizacion del juego
Sistema de inputs para gestionar los movimientos del jugador con las teclas
*/

public class Player : MonoBehaviour
{
    [SerializeField] private TerrainGenerator terrainGenerator;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float raycastDistance = 1f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private EndGameManager endGameManager;
    [SerializeField] private NextGameManager nextGameManager; // referencia al NextGameManager

    public float boundaryZ = 19.5f;
    private Animator animator;
    private bool isHopping;
    private int score;

    private IMoveStrategy moveStrategy;
    private ICommand moveCommand;
    private GameFacade gameFacade;

    private void Start()
    {
        animator = GetComponent<Animator>();
        gameFacade = new GameFacade(scoreText, endGameManager);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Vehicle>() != null)
        {
            if (collision.collider.GetComponent<Vehicle>().isLog)
            {
                transform.parent = collision.collider.transform;
            }
            else
            {
                gameFacade.EndGame(score);
            }
        }
        else if (collision.collider.CompareTag("Agua"))
        {
            gameFacade.EndGame(score);
        }
        else if (collision.collider.CompareTag("Meta")) // Detectar colisión con Meta
        {
            ShowNextGameScreen(); // Mostrar el canvas de siguiente juego
        }
        else
        {
            transform.parent = null;
        }
    }

    private void Update()
    {
        gameFacade.UpdateScore(score);

        if (Input.GetKeyDown(KeyCode.W) && !isHopping)
        {
            Move(new MoveForward());
        }
        else if (Input.GetKeyDown(KeyCode.A) && !isHopping)
        {
            Move(new MoveLeft());
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isHopping)
        {
            Move(new MoveRight());
        }

        if (Mathf.Abs(transform.position.z) > boundaryZ)
        {
            Destroy(gameObject);
            gameFacade.EndGame(score);
        }
    }

    private void Move(IMoveStrategy strategy)
    {
        moveStrategy = strategy;
        moveCommand = new MoveCommand(this, moveStrategy.GetDirection());
        moveCommand.Execute();
    }

    public void TryMove(Vector3 direction)
    {
        if (CanMove(direction))
        {
            MoveCharacter(direction);
            if (direction == Vector3.right) score++;
        }
    }

    private bool CanMove(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, raycastDistance, obstacleLayer))
        {
            return false;
        }
        return true;
    }

    private void MoveCharacter(Vector3 direction)
    {
        animator.SetTrigger("hop");
        isHopping = true;
        StartCoroutine(MoveCoroutine(direction));
    }

    private IEnumerator MoveCoroutine(Vector3 direction)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + direction;
        float moveDuration = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        terrainGenerator.SpawnTerrain(false, transform.position);
        isHopping = false;
    }

    public void FinishHop()
    {
        isHopping = false;
    }

    private void ShowNextGameScreen()
    {
        nextGameManager.ShowNextGameScreen(); // Mostrar el canvas de siguiente juego
    }
}

public class MoveCommand : ICommand
{
    private Player player;
    private Vector3 direction;
        
    public MoveCommand(Player player, Vector3 direction)
    {
        this.player = player;
        this.direction = direction;
    }

    public void Execute()
    {
        player.TryMove(direction);
    }
}

public interface ICommand
{
    void Execute();
}

public interface IMoveStrategy
{
    Vector3 GetDirection();
}

public class MoveForward : IMoveStrategy 
{
    public Vector3 GetDirection()
    {
        return Vector3.right;
    }
}

public class MoveLeft : IMoveStrategy
{
    public Vector3 GetDirection()
    {
        return Vector3.forward;
    }
}

public class MoveRight : IMoveStrategy
{
    public Vector3 GetDirection()
    {
        return Vector3.back;
    }
}

public class GameFacade
{
    private TextMeshProUGUI scoreText;
    private EndGameManager endGameManager;

    public GameFacade(TextMeshProUGUI scoreText, EndGameManager endGameManager)
    {
        this.scoreText = scoreText;
        this.endGameManager = endGameManager;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void EndGame(int score)
    {
        endGameManager.ShowEndGameScreen(score);
    }
}
