using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/****************************************************/
// The Load Scence script is used for loading a new
// scence or end a game
/****************************************************/
public class LoadScene : MonoBehaviour
{
    // Fading effect image and animator
    public Image black;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadScene("Scenes/Starting", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Load a new scence
    public void ChangeScene()
    {
        // Fading effect
        StartCoroutine(Fading());

    }

    // End game function
    public void EndGame()
    {
        Application.Quit();
    }

    // Fading function activated when change between scence
    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        // Load the next scence in building order
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
