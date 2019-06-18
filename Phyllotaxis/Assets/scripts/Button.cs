using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour {

    // Use this for initialization
    public Camera background;
    public Camera sceneLd;
    public GameObject BtnEsc;
    public GameObject BtnLd;
    public GameObject Phyllotaxis;
    public GameObject slider;

	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void btn_esc()
    {
        background.enabled = true;
        sceneLd.enabled = false;
        BtnEsc.SetActive(false);
        BtnLd.SetActive(true);
        Phyllotaxis.SetActive(false);
        slider.SetActive(true);
    }
    public void btn_ld()
    {
        Debug.Log("测试1");
        background.enabled = false;
        sceneLd.enabled = true;
        BtnEsc.SetActive(true);
        BtnLd.SetActive(false);
        Phyllotaxis.SetActive(true);
        slider.SetActive(false);
    }
}
