using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class WorldGenerator : MonoBehaviour
{
    // Gameplay
    private float _chunkSpawnZ;
    private Queue<Chunk> activeChunks = new Queue<Chunk>();
    private List<Chunk> chunkPool = new List<Chunk>();
    
    // Configs
    [SerializeField] private int firstChunkSpawnPosition = -10;
    [SerializeField] private int chunkOnScreen = 5;
    [SerializeField] private float disableDistance = 5f;
    [SerializeField] private List<GameObject> chunkPrefab;
    [SerializeField] private Transform cameraTransform;
    
    #region TO DELETE 
    private void Awake()
    {
        ResetWorld();
    }
    #endregion

    private void Start()
    {
        // Check if we have an empty chunkPrefab list
        if (chunkPrefab.Count != 0) return;
        Debug.LogError("No chunk prefab found on the world generator, assign some chunks.");

        // Try to assign the camera
        if (cameraTransform) return;
        cameraTransform = Camera.main.transform;
        Debug.LogError("We've assigned camera transform automatically.");
    }

    private void Update()
    {
        ScanPosition();
    }
    
    public void ResetWorld()
    {
        // Reset the ChunkSpawnZ
        _chunkSpawnZ = firstChunkSpawnPosition;

        for (int i = activeChunks.Count; i < 0; i--)
        {
            DisableLastChunk();
        }

        for (int i = 0; i < chunkOnScreen; i++)
        {
            SpawnNewChunk();
        }
    }

    private void ScanPosition()
    {

        var cameraZ = cameraTransform.transform.position.z;
        var lastChunk = activeChunks.Peek();

        if (!(cameraZ >= lastChunk.transform.position.z + lastChunk.chunkLength + disableDistance)) return;
        SpawnNewChunk();
        DisableLastChunk();
    }

    private void SpawnNewChunk()
    {
        // Get a random index for which prefab to spawn
        // TODO
        var randomIndex = UnityEngine.Random.Range(0, chunkPrefab.Count);


        // Does it already exist within our pool?
        // TODO
        var chunk = chunkPool.Find(x => !x.gameObject.activeSelf && x.name == (chunkPrefab[randomIndex].name) + "(Clone)");
        
        // Create a chunk if we were not able to find one to reuse
        if (!chunk)
        {
            var go = Instantiate(chunkPrefab[randomIndex], transform);
            chunk = go.GetComponent<Chunk>();
        }

        // Place the object and show it
        chunk.transform.position = new Vector3(0, 0, _chunkSpawnZ);
        _chunkSpawnZ += chunk.chunkLength;
        
        // Store value to pool
        activeChunks.Enqueue(chunk);
        chunk.ShowChunk();
    }

    private void DisableLastChunk()
    {
        var chunk = activeChunks.Dequeue();
        chunk.HideChunk();
        chunkPool.Add(chunk);
    }
}
