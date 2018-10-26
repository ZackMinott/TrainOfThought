using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGlassLayer : MonoBehaviour {
    public GameObject player;

    Player1 check;

	// Use this for initialization
	void Start () {
        check = player.GetComponent<Player1>();
    }
	
	// Update is called once per frame
	void Update () {
        GameObject[] childs = new GameObject[transform.childCount];
        if (check.isNormalForm == false)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                childs[i] = transform.GetChild(i).gameObject;
                childs[i].layer = 2;
            }
        } else if (check.isNormalForm)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                childs[i] = transform.GetChild(i).gameObject;
                childs[i].layer = 9;
            }
        }
	}
}


