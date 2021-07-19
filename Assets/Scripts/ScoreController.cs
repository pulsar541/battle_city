using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{

    GameObject txt;
    // Start is called before the first frame update
    void Start()
    { 
        txt = GameObject.Find("Player1ScoreValue");
        if (txt != null)  
            txt.GetComponent<UnityEngine.UI.Text>().text = Global.score[0].ToString();

        txt = GameObject.Find("P1TankCount1Value");
        if (txt != null)  
            txt.GetComponent<UnityEngine.UI.Text>().text =  Global.destroyedTankTypesCounter[0, (int)Tank.Type.TYPE_SIMPLE] .ToString();
      
        txt = GameObject.Find("P1TankCount2Value");
        if (txt != null)  
            txt.GetComponent<UnityEngine.UI.Text>().text =  Global.destroyedTankTypesCounter[0, (int)Tank.Type.TYPE_BTR] .ToString();

        txt = GameObject.Find("P1TankScore1Value");
        if (txt != null)  
            txt.GetComponent<UnityEngine.UI.Text>().text =  (Global.destroyedTankTypesCounter[0, 0] * 100).ToString();
      
        txt = GameObject.Find("P1TankScore2Value");
        if (txt != null)  
            txt.GetComponent<UnityEngine.UI.Text>().text =  (Global.destroyedTankTypesCounter[0, 1] * 200).ToString();




        txt = GameObject.Find("Player2ScoreValue");
        if (txt != null)  
            txt.GetComponent<UnityEngine.UI.Text>().text = Global.score[1].ToString();
      
        txt = GameObject.Find("P2TankCount1Value");
        if (txt != null)  
            txt.GetComponent<UnityEngine.UI.Text>().text =  Global.destroyedTankTypesCounter[1, (int)Tank.Type.TYPE_SIMPLE] .ToString();
      
        txt = GameObject.Find("P2TankCount2Value");
        if (txt != null)  
            txt.GetComponent<UnityEngine.UI.Text>().text =  Global.destroyedTankTypesCounter[1, (int)Tank.Type.TYPE_BTR] .ToString();

        txt = GameObject.Find("P2TankScore1Value");
        if (txt != null)  
            txt.GetComponent<UnityEngine.UI.Text>().text =  (Global.destroyedTankTypesCounter[1, 0] * 100).ToString();
      
        txt = GameObject.Find("P2TankScore2Value");
        if (txt != null)  
            txt.GetComponent<UnityEngine.UI.Text>().text =  (Global.destroyedTankTypesCounter[1, 1] * 200).ToString();


 

        txt = GameObject.Find("TotalScoreValue");
        if (txt != null)  
        {
            int total = 0;
            for(int i = 0; i< Global.MaxPlayersCount; i++) {
                 total += Global.score[i];
            }
            txt.GetComponent<UnityEngine.UI.Text>().text = total.ToString();
        }




 
    }

    // Update is called once per frame
    void Update()
    {
        

        if(Input.GetButtonDown("Submit"))
        {
            StartCoroutine(LoadAsyncStartupScene());
        }
    }
    IEnumerator LoadAsyncStartupScene()
    { 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/StartupScene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    } 
}
