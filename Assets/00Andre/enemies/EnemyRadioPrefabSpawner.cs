using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadioPrefabSpawner : MonoBehaviour
{
    public static EnemyRadioPrefabSpawner instance;
    
    public GameObject EnemyParent;
    public GameObject EnemyRadioPrefab;
    private Vector3 spawnPosition;
    
    // list of enemies
    public List<EnemyRadio> enemies = new List<EnemyRadio>();
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        spawnPosition = transform.position;
    }
    
    public void SpawnEnemyRadio()
    {
        var obj = Instantiate(EnemyRadioPrefab, spawnPosition, Quaternion.identity, EnemyParent.transform);
        enemies.Add(obj.GetComponent<EnemyRadio>());
    }
    
    public void DeactivateEnemyRadio()
    {
        try
        {
            foreach (var enemy in enemies)
            {
                // remove and destroy
                enemies.Remove(enemy);
                Destroy(enemy.gameObject);
            }
        }
        catch (Exception)
        {
        }
    }
}
