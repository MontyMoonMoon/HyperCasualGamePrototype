using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpwner : MonoBehaviour
   {


    //timing is awkward needs tweaking
       
    public Transform player;
    public GameObject boxPrefab;
    public float spawnInterval = 2f;
    public float spawnHeightOffset = 10f;
    public float spawnRangeX = 8f;

    private float timer = 0f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
     
         if (player != null)
        {
            Vector2 newPosition = new Vector2(player.position.x, player.position.y + spawnHeightOffset);
            transform.position = newPosition;
        }

        
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnBox();
        }
    }

    void SpawnBox()

       //supposed to lock spawner relative to player at a given height
    {
     Vector2 spawnPosition = new Vector2(transform.position.x + Random.Range(-spawnRangeX, spawnRangeX),transform.position.y + Random.Range(0f, 2f));

    Instantiate(boxPrefab, spawnPosition, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
    }
}
