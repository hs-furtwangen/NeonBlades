using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
	public static GameMaster _instance;
	public Text timerTextGame;
	public Text timerTextFinish;
	public GameObject pauseScreen;
	public GameObject finishScreen;
	public StarterAssetsInputs inputSystem;
 	public bool gamePaused = false;
	public List<Target> targets = new List<Target>();

	bool gameFinished = false;
	float secondCount = 0;
	float minuteCount = 0;
	public int targetNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
		if(GameMaster._instance == null) {
			Debug.Log("Trying to init. Game Master Instance");
			GameMaster._instance = this;
			Debug.Log("GameMaster: " + GameMaster._instance);
		}
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
		inputSystem.PauseGame(gamePaused);
		gameFinished = true;
		finishScreen.SetActive (true);
		timerTextFinish.text = "Time:\n " + minuteCount.ToString("00") + "m:"+ ((int)secondCount).ToString("00") + "s";
	}

	public void PauseGame(){
		gamePaused = !gamePaused;
		pauseScreen.SetActive (gamePaused);
		inputSystem.PauseGame(gamePaused);
	}

	public void StopGame(){
		SceneManager.LoadScene("Start"); //Add Name of Game Scene
	}

	public void RegisterTarget(Target target) {
		Debug.Log("Add Target: " + target);
		targets.Add(target);
		targetNumber++;
	}
	public void DestroyTarget(Target target) {
		foreach(var targetsItem in targets)
		{
			if(targetsItem == target)
			{
				targetNumber--;
				break;
			}
		}
		if (targetNumber == 0) {
			FinishGame();
		}
	}
}
