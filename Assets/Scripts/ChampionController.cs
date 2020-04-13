using System.Collections;
using UnityEngine;
using Cinemachine;

/****************************************************/
// The Player script is used for defining player
// properties and player behaviors
/****************************************************/
public class ChampionController : Singleton<ChampionController>
{
    /****************************************************/
    /***************** Basic Properties *****************/
    /****************************************************/
    public Champion[] champions;
    public int activeChampionIdx = 0;
    public CinemachineVirtualCamera activeChampionCamera;
    /****************************************************/

    /****************************************************/
    /***************** Basic Functions ******************/
    /****************************************************/
    // Start is called before the first frame update
    void Start()
    {
        foreach (Champion champ in champions)
        {
            champ.playerCanControl = false;
        }
        setActiveChampion(0);
    }

    // Update is called once per frame
    void Update()
    {
        // Spacebar increments the activeChampionIdx;
        if (Input.GetKeyUp("space"))
        {
            setActiveChampion((activeChampionIdx + 1) % champions.Length);
        }
    }

    private void setActiveChampion(int idx)
    {
        champions[activeChampionIdx].playerCanControl = false;
        activeChampionIdx = idx;
        champions[activeChampionIdx].playerCanControl = true;
        activeChampionCamera.Follow = champions[activeChampionIdx].transform;
    }
}
