using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
TerrainData es un scriptable object lo que permite ser editado y creado en 
el inspector de unity
*/

[CreateAssetMenu(fileName = "Terrain Data", menuName = "Terrain Data")]
// Permite que se puedan crear instancias de TerrainData desde el menu de Assets
public class TerrainData : ScriptableObject
{
    public List<GameObject> possibleTerrain; // lista de los posibles terreno a generar
    public int maxInSuccesion; // maximos seguidos que puede generar
    public int minInSuccesion; // minimo de los que puede generar
    
}
