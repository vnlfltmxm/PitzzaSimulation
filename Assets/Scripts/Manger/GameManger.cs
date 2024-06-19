using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class GameManger : Singleton<GameManger>
{
    [SerializeField]
    private GameObject Prefab_PizzaHouse;
    [SerializeField]
    private GameObject Prefab_Player;

    private Transform playerSpawnPos;

    private void Awake()
    {
        GameObject map = Instantiate(Prefab_PizzaHouse);
        map.GetComponent<NavMeshSurface>().BuildNavMesh();
        SpawnPlayer();

    }


    private void SpawnPlayer()
    {
        playerSpawnPos = InteractionObjectManger.Instance.FindPrefabsParentTrasnform("playerSpawnPos");
        Instantiate(Prefab_Player, playerSpawnPos.position, playerSpawnPos.rotation);

    }

}
