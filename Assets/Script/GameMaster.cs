using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
	public Text timerTextGame;
	public Text timerTextFinish;
	public GameObject pauseScreen;
	public GameObject finishScreen;
 	public bool gamePaused = false;

	bool gameFinished = false;
	float secondCount = 0;
	float minuteCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){

		if(!gamePaused){
			CalculateGameTime ();
		}

		if(Input.GetKeyDown(KeyCode.Escape) && !gameFinished){
			PauseGame ();
		}
        
    }

	void CalculateGameTime(){
		secondCount += Time.deltaTime;
		if (secondCount >= 60) {
			minuteCount++;
			secondCount %= 60;
		}
		timerTextGame.text = "Time: " + minuteCount.ToString("00") + "m:"+ ((int)secondCount).ToString("00") + "s";
	}

	void FinishGame(){
		gamePaused = true;
		gameFinished = true;
		finishScreen.SetActive (true);
		timerTextFinish.text = "Time:\n " + minuteCount.ToString("00") + "m:"+ ((int)secondCount).ToString("00") + "s";
	}

	public void PauseGame(){
		gamePaused = !gamePaused;
		pauseScreen.SetActive (gamePaused);
	}

	public void StopGame(){
		SceneManager.LoadScene("Start"); //Add Name of Game Scene
	}
}
