using System.Collections;
using UnityEngine;

/*
Script asociado al agua y carretera
*/

public class MovingObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnObject; // se asigna el prefab a spawnear (coche o tablon)
    [SerializeField] private Transform spawnPos; // se define el spawn point de donde salen
    [SerializeField] private float minSeparationTime; // tiempo minimo entre spawns
    [SerializeField] private float maxSeparationTime; // tiempo maximo entre spawns
    [SerializeField] private bool isRightSide; // se define para saber hacia que lado tienen que ir

    void Start()
    {
        StartCoroutine(SpawnVehicle());
    }

    private IEnumerator SpawnVehicle()
    {
        while (true)
        {
            // espera un tiempo aleatorio entre el minimo y maximo definidos para seguir
            yield return new WaitForSeconds(Random.Range(minSeparationTime, maxSeparationTime));
            // instancia un nuevo objeto en la poscion spawnPos con la rotacion determianda
            GameObject go = Instantiate(spawnObject, spawnPos.position, Quaternion.identity);
            // Agrega el script DestroyOutOfBonds al objeto para que se destruya una vez salga del mapa
            DestroyOutOfBounds destroyScript = go.AddComponent<DestroyOutOfBounds>();

            if (!isRightSide) // define la direccion del objeto
            {
                // si es falso lo gira de 180 para que vaya en la direccion correcta
                go.transform.Rotate(new Vector3(0, 180, 0));
            }
        }
    }
}

