

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Script generador del terreno
*/

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int minDistanceFromPlayer;
    [SerializeField] private int maxTerrainCount; // maximo numero de terrenos a la vez
    [SerializeField] private List<TerrainData> terrainDatas = new List<TerrainData>(); // lista de objetos terrain data (grass, road, water)
    [SerializeField] private Transform terrainHolder; // contenedor para los terrenos
    [SerializeField] private GameObject initialSpawnTerrain; // GameObject que contiene los terrenos del spawn inicial
    [SerializeField] private GameObject metaPrefab; // Prefab Meta

    private List<GameObject> currentTerrains = new List<GameObject>(); // lista de los terrenos actuales existentes en el juego
    [HideInInspector] public Vector3 currentPosition = new Vector3(0,0,0); // posicion donde generar el proximo terreno
    private int lastIndex = -1;

    private bool metaSpawned = false; // bandera para indicar si el prefab Meta ha sido generado
    
    private void Start() // se llama al iniciar
    {
        GenerateInitialSpawnZone(); // llama a esta funcion para el spawn del mapa

        for (int i = 0; i < maxTerrainCount; i++) // hasta tener el numero limite de terreno
        {
            // spawnea terrenos
            SpawnTerrain(true, new Vector3(0, 0, 0));
        }
        // actualiza el valor
        maxTerrainCount = currentTerrains.Count;
    }

    private void GenerateInitialSpawnZone()
    {
        // recorre los terrenos del spawn
        foreach (Transform child in initialSpawnTerrain.transform)
        {
            // instancia el terrano del spawn en la posicion
            GameObject terrain = Instantiate(child.gameObject, currentPosition, Quaternion.identity, terrainHolder);
            // lo añade a la lista de terrenos actuales
            currentTerrains.Add(terrain);
            // se aumenta la posicion
            currentPosition.x++;
        }
    }

    // genera nuevos terrenos si la posicion actual esta dentro de la distancia minima o es el inicio.
    public void SpawnTerrain(bool isStart, Vector3 playerPos)
    {
        if ((currentPosition.x - playerPos.x < minDistanceFromPlayer) || (isStart))
        {
            int terrainIndex;
            do
            {
                // seleciona un terreno aleatorio diferente al ultimo
                terrainIndex = Random.Range(0, terrainDatas.Count);
            } 
            while (terrainIndex == lastIndex); 

            // se guarda el valor del que va a ser el ultimo terreno
            lastIndex = terrainIndex;
            // se obtiene un TerrainData (conjunto de un terreno)
            TerrainData selectedTerrainData = terrainDatas[terrainIndex];
            // se genera un num aleatorio entre los limites establecidos de terreno
            int terrainInSuccession = Random.Range(selectedTerrainData.minInSuccesion, selectedTerrainData.maxInSuccesion);

            // genera el numero de terreno recien establecido
            for (int i = 0; i < terrainInSuccession; i++)
            {
                // crea un nuevo de la lista de terrenos posibles en selectedTerrainData
                GameObject terrain = Instantiate(selectedTerrainData.possibleTerrain[Random.Range(0, selectedTerrainData.possibleTerrain.Count)], currentPosition, Quaternion.identity, terrainHolder);
                // se le añade a la lista de terreno
                currentTerrains.Add(terrain);

                if (!isStart) // si no estamos generando el spawn
                {
                    if (currentTerrains.Count > maxTerrainCount) // si hay mas terrenos que el limite permitido
                    {
                        Destroy(currentTerrains[0]); // se elimina el primer terreno
                        currentTerrains.RemoveAt(0); // se le elimina de la lista
                    }
                }
                currentPosition.x++; // se actualiza la posicion
            }
        }

        // Verificar si el jugador ha avanzado 5 unidades en X
        if (!metaSpawned && playerPos.x >= 5)
        {
            SpawnMeta();
        }
    }

    private void SpawnMeta()
    {
        // Instanciar el prefab Meta en la posición actual
        Instantiate(metaPrefab, currentPosition, Quaternion.identity, terrainHolder);
        metaSpawned = true; // Marcar que el prefab Meta ha sido generado
        currentPosition.x++;
    }
}
