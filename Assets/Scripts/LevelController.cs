using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public static bool isGameOver = false;

    public static bool isPause = false;

    private float _timerGoToScoreScene;
 
    float _msekPauseCnt = 0;
    void Start()
    {
        switch (gameMode)
        {
            case 0:
                player1 = Instantiate(playerPrefab, spawnPlayer1, Quaternion.identity);
                player1.GetComponent<Player>().selfIndex = 0; 
                player1.AddComponent<InputsPlayerOne>();

                break;
            case 1:
                player1 = Instantiate(playerPrefab, spawnPlayer1, Quaternion.identity);
                player1.GetComponent<Player>().selfIndex = 0;
                player1.AddComponent<InputsPlayerOne>();

                player2 = Instantiate(playerPrefab, spawnPlayer2, Quaternion.identity);
                player2.GetComponent<Player>().selfIndex = 1;
                player2.AddComponent<InputsPlayerTwo>(); 
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
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)) 
        {
            isPause = !isPause;    
        }

        if(isPause) {
            _msekPauseCnt += Time.deltaTime;
            if(_msekPauseCnt > 1.0) 
                _msekPauseCnt = 0;
            pauseTextGO.SetActive(_msekPauseCnt > 0.5f);
        }
        else 
            pauseTextGO.SetActive(false);

        if(LevelController.isPause)
            return;

        if (_enemySpawnCounter > enemySpawnInterval)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnEnemy[_currentEnemySpawnIndex], Quaternion.identity);
            _enemySpawnCounter = 0;
            _currentEnemySpawnIndex++;
            if (_currentEnemySpawnIndex > 2)
                _currentEnemySpawnIndex = 0;
        }
        _enemySpawnCounter += Time.deltaTime;



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
        else {

            if(Input.GetKeyDown(KeyCode.Escape)) 
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
    }


    IEnumerator LoadAsyncScene(string name)
    { 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/" + name);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    
}
