using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void StartGame(){
		// SceneManager.LoadScene(/*scenename*/); //Add Name of Game Scene
	}
	
	public void ShowScreen(GameObject _showenScreen){
		_showenScreen.SetActive(true);
	}
	
	public void HideScreen(GameObject _hiddenScreen){
		_hiddenScreen.SetActive(false);
	}
	
	public void QuitGame(){
		Application.Quit();
	}
}
