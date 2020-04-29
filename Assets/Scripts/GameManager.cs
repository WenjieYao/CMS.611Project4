using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/****************************************************/
// The Game Manager script is used for all game
// control functionalities and record current
// game status
/****************************************************/
public class GameManager : Singleton<GameManager>
{
    /****************************************************/
    /************* Game Control Parameters **************/

    public AudioSource globalAudioSource;
    public float globalListenerVolume = 0.1f;

    /****************************************************/
    /************ Basic Properties (private) ************/
    /****************************************************/


    /****************************************************/
    // Public properties that corresponds to the private
    // properties above
    /****************************************************/

    /****************************************************/


    /****************************************************/
    /***************** Control Functions ****************/
    /****************************************************/
    // Start is called before the first frame update
    void Start()
    {
        globalAudioSource = gameObject.AddComponent<AudioSource>();
        AudioListener.volume = globalListenerVolume;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    public GameObject GameOver;
    public GameObject Win;
    // Function to end game
    public void QuitGame()
    {
        Time.timeScale = 1;
        //Debug.Log("LoadScene");
        SceneManager.LoadScene(2);

    }

    // Function to restart a new game
    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
