using System.Collections; 
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupController : MonoBehaviour
{

    [SerializeField] public GameObject selectorPrefab;

     
    int currentSelectorMode;
    int modesCount = 3;

    Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        currentSelectorMode = 0;
        UpdateSelectorPos(currentSelectorMode); 
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Vertical")) {
                if(Input.GetAxis("Vertical") < 0) {
                currentSelectorMode++;
                if(currentSelectorMode >= modesCount)
                    currentSelectorMode = 0;
                } else {
                    currentSelectorMode--;
                    if(currentSelectorMode < 0)
                    currentSelectorMode = modesCount - 1;
                }    
            UpdateSelectorPos(currentSelectorMode);
        }  

        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Submit")) {
            if(mainCamera.GetComponent<StartupCamera>().IsOnTarget())
                StartCoroutine(LoadAsyncLevelScene(currentSelectorMode));
            else 
                mainCamera.GetComponent<StartupCamera>().SetPositionToTarget(); 
        }      
    }

    void UpdateSelectorPos(int modeNum) {
        selectorPrefab.transform.position = new Vector3(-8, -4.63f-modeNum * 2.5f, 0);
    }
 
    IEnumerator LoadAsyncLevelScene(int modeNum)
    {   
        LevelController.gameMode = modeNum;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/LevelScene"); 
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
