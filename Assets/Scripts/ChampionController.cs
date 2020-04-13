using System.Collections;
using UnityEngine;

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
        champions[activeChampionIdx].playerCanControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Spacebar increments the activeChampionIdx;
        if (Input.GetKeyUp("space"))
        {
            champions[activeChampionIdx].playerCanControl = false;
            activeChampionIdx += 1;
            activeChampionIdx %= champions.Length;
            champions[activeChampionIdx].playerCanControl = true;
        }
    }
}
