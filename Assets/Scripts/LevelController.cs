using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject basePrefab;
    [SerializeField] GameObject bonusPrefab;
    public static int gameMode = 0;

    Vector3 spawnPlayer1 = new Vector3(9, -24, 0);
    Vector3 spawnPlayer2 = new Vector3(17, -24, 0);

    Vector3[] spawnEnemy = { new Vector3(1, 0, 0), new Vector3(13, 0, 0), new Vector3(24, 0, 0) };

    public float enemySpawnInterval = 3.0f;
    private int _currentEnemySpawnIndex = 0;
    private float _enemySpawnCounter = 2.5f;
    // Start is called before the first frame update

    GameObject player1;
    GameObject player2;
    GameObject baseGO;
    GameObject gameOverTextGO;

    GameObject pauseTextGO;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap tilemapOver;
    [SerializeField] private TileBase tileBaseSteelWall;
    [SerializeField] private TileBase tileBaseBrickWall;
    [SerializeField] private TileBase tileBaseForest;
    [SerializeField] private TileBase tileBaseRiver;
    [SerializeField] private TileBase tileBaseIce;
    [SerializeField] private TileBase tileEnemyCounter;

    public static bool isGameOver = false;

    public static bool isPause = false;

    private float _timerGoToScoreScene;

    float _msekPauseCnt = 0;

    public const int maxEnemyCount = 20;
    public int enemySpawnCount = 0;

    void Awak()
    {
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }

    void Start()
    {
        switch (gameMode)
        {
            case 0:
                player1 = Instantiate(playerPrefab, spawnPlayer1, Quaternion.identity);
                player1.AddComponent<InputsPlayerOne>();
                player1.GetComponent<Player>().Init(0, spawnPlayer1);
                break;
            case 1:
                player1 = Instantiate(playerPrefab, spawnPlayer1, Quaternion.identity);
                player1.AddComponent<InputsPlayerOne>();
                player1.GetComponent<Player>().Init(0, spawnPlayer1);

                player2 = Instantiate(playerPrefab, spawnPlayer2, Quaternion.identity);
                player2.AddComponent<InputsPlayerTwo>();
                player2.GetComponent<Player>().Init(1, spawnPlayer2);
                break;
            case 2:
                break;
        }

        baseGO = Instantiate(basePrefab, new Vector3(13, -24, 0), Quaternion.identity);

        gameOverTextGO = GameObject.Find("GameOver");
        pauseTextGO = GameObject.Find("Pause");
        pauseTextGO.SetActive(false);

        isGameOver = false;
        _timerGoToScoreScene = 5.0f;
        enemySpawnCount = 0;

        Global.Reset();

        GenerateMap();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isGameOver && Input.GetKeyDown(KeyCode.Return))
        {
            isPause = !isPause;
        }

        if (isPause)
        {
            _msekPauseCnt += Time.deltaTime;
            if (_msekPauseCnt > 1.0)
                _msekPauseCnt = 0;
            pauseTextGO.SetActive(_msekPauseCnt > 0.5f);
        }
        else
            pauseTextGO.SetActive(false);

        if (isPause)
            return;

        if (enemySpawnCount < maxEnemyCount)
        {
            if (_enemySpawnCounter > enemySpawnInterval)
            {
                enemySpawnCount++;
                GameObject enemy = Instantiate(enemyPrefab, spawnEnemy[_currentEnemySpawnIndex], Quaternion.identity);
                _enemySpawnCounter = 0;
                _currentEnemySpawnIndex++;
                if (_currentEnemySpawnIndex > 2)
                    _currentEnemySpawnIndex = 0;

            }
            _enemySpawnCounter += Time.deltaTime;
        }

        int enemyTanksDestroyed = 0;

        for (int p = 0; p < Global.MaxPlayersCount; p++)
            for (int type = 0; type < (int)Tank.Type.MAX_TYPES; type++)
                enemyTanksDestroyed += Global.destroyedTankTypesCounter[p, type];


        if (enemyTanksDestroyed >= maxEnemyCount)
        {
            StartCoroutine(LoadAsyncScene("ScoreScene", true));
        }


        if (gameMode == 0)
        {
            if (player1 == null)
                isGameOver = true;
        }
        else if (gameMode == 1)
        {
            if (player1 == null && player2 == null)
                isGameOver = true;
        }

        if (baseGO.GetComponent<Base>().Health <= 0)
            isGameOver = true;

        if (isGameOver)
        {
            if (gameOverTextGO)
            {
                if (gameOverTextGO.transform.position.y < -12)
                    gameOverTextGO.transform.position = new Vector3(gameOverTextGO.transform.position.x, gameOverTextGO.transform.position.y + Time.deltaTime * 10, gameOverTextGO.transform.position.z);
            }


            if (_timerGoToScoreScene <= 0 || Input.GetButtonDown("Submit"))
            {
                StartCoroutine(LoadAsyncScene("ScoreScene"));
            }
            _timerGoToScoreScene -= Time.deltaTime;
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(LoadAsyncScene("StartupScene"));
            }
        }
    }


    public static void UpdateBattleInformation()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            string livesNameGO = (player.GetComponent<Player>().selfIndex + 1).ToString() + "PLives";
            GameObject txt = GameObject.Find(livesNameGO);
            if (txt != null)
                txt.GetComponent<UnityEngine.UI.Text>().text = player.GetComponent<Player>().Lives.ToString();
        }


        int total = 0;
        for (int i = 0; i < Global.MaxPlayersCount; i++)
        {
            total += Global.score[i];
        }
         
        LevelController levelController = GameObject.FindObjectOfType<LevelController>();  
        Vector3Int cellEnemyCounterWorldPosition;
      
        for (int p = 0; p < (int)Global.MaxPlayersCount; p++)
        for (int t = 0; t < (int)Tank.Type.MAX_TYPES; t++) 
        {
            int posByPlayerOffset = (p == 0) ? 0 : 35;
            int i = 0;
            int row = 0;
            for (int s = 0; s < Global.destroyedTankTypesCounter[p, t]; s++)
            {
                cellEnemyCounterWorldPosition = levelController.tilemap.WorldToCell(new Vector3(i-3 + posByPlayerOffset, row, -1));
                levelController.tilemap.SetTile(cellEnemyCounterWorldPosition, levelController.tileEnemyCounter); 
                if (i == 1) row--; 
                i = 1 - i;
            }
        } 
        
    }


    IEnumerator LoadAsyncScene(string name, bool waitBefore = false)
    {
        if (waitBefore)
            yield return new WaitForSeconds(3.0f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/" + name);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


    void GenerateMap()
    {

        Tilemap currTileMap = null;
        Tilemap anotherTileMap = null;
        TileBase currTileBase = null;
        
        for (float i = 0; i <= 25; i++)
        {
            for (float j = 0; j >= -25; j--)
            {
                if (i >= 11 && i <= 14 && j <= -23)
                    continue;

                Vector3Int cellWorldPosition = tilemap.WorldToCell(new Vector3(i, j, -1));
                tilemap.SetTile(cellWorldPosition, null);
                tilemapOver.SetTile(cellWorldPosition, null);

            }
        }


        int whatTileInsert = -1;
        int i_cut = 12;
        int j_cut = -12;
        for (int s = 0; s < 300; s++)
        {
            int dir = Random.Range(0, 4);
            switch (dir)
            {
                case 0: i_cut += 2; break;
                case 1: i_cut -= 2; break;
                case 2: j_cut += 2; break;
                case 3: j_cut -= 2; break;
            }

            if (i_cut < 0 || i_cut > 24 /*||j_cut > -1 || j_cut < -25*/)
            {
                i_cut = Random.Range(0, 24);
                s--;
                // j_cut = Random.Range(-25, -1);
            }

            if (/*i_cut < 0 || i_cut > 24 ||*/ j_cut < -25 || j_cut > -1)
            {
                // i_cut = Random.Range(0, 24);
                j_cut = Random.Range(-25, -1);
                s--;
            }

            if (i_cut <= 1 && j_cut >= -1)
                continue;

            if (i_cut >= 24 && j_cut >= -1)
                continue;

            if (i_cut >= 12 && i_cut <= 13 && j_cut >= -1)
                continue;

            if (i_cut >= 11 && i_cut <= 14 && j_cut <= -23)
                continue;


            if (i_cut >= 9 && i_cut <= 10 && j_cut <= -24)
                continue;


            if (i_cut >= 15 && i_cut <= 16 && j_cut <= -24)
                continue; 
             
            if (Random.Range(0, 100) < 20)
            {
                whatTileInsert = Random.Range(0, 5);
                switch (whatTileInsert)
                {
                    case 0: currTileBase = tileBaseBrickWall; break;
                    case 1: currTileBase = tileBaseForest; break;
                    case 2: if (Random.Range(0, 100) < 20) currTileBase = tileBaseRiver; break;
                    case 3: currTileBase = tileBaseIce; break;
                    case 4: if (Random.Range(0, 100) < 40) currTileBase = tileBaseSteelWall; break; 

                }
            }


            currTileMap = currTileBase == tileBaseForest ? tilemapOver : tilemap;
            anotherTileMap = currTileMap == tilemap ? tilemapOver : tilemap; 
            Vector3Int cellWorldPosition = currTileMap.WorldToCell(new Vector3(i_cut, j_cut, -1));
                 
            if(whatTileInsert != 5) { 
                
                    // if(tilemap.GetTile(cellWorldPosition) != null) {
                    //     s--;
                    //     continue;
                    // }
        

                    currTileMap.SetTile(cellWorldPosition, currTileBase);
                    currTileMap.SetTile(cellWorldPosition + new Vector3Int(1, 1, 0), currTileBase);
                    currTileMap.SetTile(cellWorldPosition + new Vector3Int(1, 0, 0), currTileBase);
                    currTileMap.SetTile(cellWorldPosition + new Vector3Int(0, 1, 0), currTileBase); 

                    anotherTileMap.SetTile(cellWorldPosition, null);
                    anotherTileMap.SetTile(cellWorldPosition + new Vector3Int(1, 1, 0), null);
                    anotherTileMap.SetTile(cellWorldPosition + new Vector3Int(1, 0, 0), null);
                    anotherTileMap.SetTile(cellWorldPosition + new Vector3Int(0, 1, 0), null); 

            }
            else {

                    currTileMap.SetTile(cellWorldPosition, null);
                    currTileMap.SetTile(cellWorldPosition + new Vector3Int(1, 1, 0), null);
                    currTileMap.SetTile(cellWorldPosition + new Vector3Int(1, 0, 0), null);
                    currTileMap.SetTile(cellWorldPosition + new Vector3Int(0, 1, 0), null); 

                    anotherTileMap.SetTile(cellWorldPosition, null);
                    anotherTileMap.SetTile(cellWorldPosition + new Vector3Int(1, 1, 0), null);
                    anotherTileMap.SetTile(cellWorldPosition + new Vector3Int(1, 0, 0), null);
                    anotherTileMap.SetTile(cellWorldPosition + new Vector3Int(0, 1, 0), null); 


            }


        }


        for (float i = 0; i <= 25; i++)
        {
            for (float j = 0; j >= -25; j--)
            {
                Vector3Int cellWorldPosition = tilemap.WorldToCell(new Vector3(i, j, -1));

                if ((i <= 1 && j >= -1) || (i >= 24 && j >= -1) || (i >= 12 && i <= 13 && j >= -1))
                {
                    tilemap.SetTile(cellWorldPosition, null);
                    tilemapOver.SetTile(cellWorldPosition, null);
                }

            }
        }



    }

}
