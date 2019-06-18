using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wordUI : MonoBehaviour {

    private Text text;
	// Use this for initialization
	void Start () {
        text = this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (word.SongWord == true)
        {
            text.text = word.Word[word._index];
        }
        else
        {
            text.text = "";
        }
    }
}
