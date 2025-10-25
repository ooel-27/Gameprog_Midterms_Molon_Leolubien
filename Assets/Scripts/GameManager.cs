using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public PlayerController player;
    public GameObject enemyPrefab;
    public GameObject gameOverCanvas;   

    [Header("Spawning")]
    public float spawnRadius = 12f;
    public float spawnInterval = 2.5f;
    public int maxEnemies = 20;

    [Header("Enemy Colors")]
    public Color[] enemyPalette = new Color[] { Color.white, Color.blue, Color.green };

    private float spawnTimer;
    private readonly List<Enemy> aliveEnemies = new List<Enemy>();
    private bool isGameOver;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        
        if (gameOverCanvas) gameOverCanvas.SetActive(false);

        
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameOver) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            TrySpawnEnemy();
        }
    }

    void TrySpawnEnemy()
    {
        if (!enemyPrefab || !player) return;

        aliveEnemies.RemoveAll(e => e == null);
        if (aliveEnemies.Count >= maxEnemies) return;

       
        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * spawnRadius;
        Vector3 pos = player.transform.position + offset;

        GameObject eObj = Instantiate(enemyPrefab, pos, Quaternion.identity);
        Enemy enemy = eObj.GetComponent<Enemy>();
        enemy.SetColor(enemyPalette[Random.Range(0, enemyPalette.Length)]);
        enemy.player = player.transform;
        aliveEnemies.Add(enemy);
    }

    public void GameOver()
    {
        isGameOver = true;
        if (player) player.isGameOver = true;

        
        foreach (var e in aliveEnemies)
            if (e) e.enabled = false;

        
        if (gameOverCanvas) gameOverCanvas.SetActive(true);

        
        Time.timeScale = 0f;

        Debug.Log("Game Over triggered by GameManager.");
    }

    
    public Enemy GetNearestEnemy(Vector3 fromPos)
    {
        Enemy nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var e in aliveEnemies)
        {
            if (e == null) continue;
            float dist = Vector3.Distance(fromPos, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = e;
            }
        }
        return nearest;
    }
}