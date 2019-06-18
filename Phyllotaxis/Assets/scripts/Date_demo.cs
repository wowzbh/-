using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Date_demo : MonoBehaviour {


    public static Date_demo Instance;
   // [HideInInspector]
    public AudioClip[] clips;
    [HideInInspector]
    public GameObject prefabs;

    private void Awake()
    {

        Instance = this;
        //加载Resources/Musics目录下的音频
        clips = Resources.LoadAll<AudioClip>("Audio");

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
