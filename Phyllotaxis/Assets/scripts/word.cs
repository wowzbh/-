using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class word : MonoBehaviour
{

     float[] Time;        //歌词时间数组
    [HideInInspector]
    public static string[] Word;       //时间数组对应的单句歌词
     int count;          //时间数组大小
    [HideInInspector]
    public  static int _index;         //当前歌曲播放时间对应的数组下标
    public AudioSource _audioSource;
    [HideInInspector]
    public static bool SongWord;         //判断是否播放歌词
    // Use this for initialization
    void Start()
    {
        SortWordTime();
    }

    // Update is called once per frame
    void Update()
    {
        isSongText();
        Find(_audioSource.time);
    }

    void isSongText()          //判断SongWord的值
    {
        if (AudioPeer._currentIndex == 1)
            SongWord = true;
        if (AudioPeer._currentIndex == 0)
            SongWord = false;
    }

    void SortWordTime()       //或缺lrc文件的时间数组和歌词数组并排序
    {
        count = 0;
        string str = File.ReadAllText(@"C:\Users\wowzbh\Desktop\可乐.lrc");
        if (str == null)
            Debug.Log("error");
        MatchCollection asd = Regex.Matches(str, @"\[.*?[\u4E00-\u9FA5]{1,100}\\");//切分成单个时间和对应歌词
        string[] numt = new string[asd.Count];
        for (int i = 0; i < asd.Count; i++)
        {
            numt[i] = asd[i].Groups[0].Value;
            MatchCollection num = Regex.Matches(numt[i], @"\[.*?\]");//计算数组数目
            count += num.Count;
        }
        Time = new float[count];
        float Timet;

        string[] Time1 = new string[count];//临时储存时间和秒，最终储存分钟
        string[] Time2 = new string[count];//储存秒
        Word = new string[count];
        string[] Array = new string[asd.Count];
        for (int i = 0, j = 0; i < asd.Count; i++)
        {
            Array[i] = asd[i].Groups[0].Value;

            MatchCollection TimeA = Regex.Matches(Array[i], @"\[.*?\]");//切分成单个时间
            MatchCollection Words = Regex.Matches(Array[i], @"[\u4E00-\u9FA5].*?\\");//切分成单句歌词

            for (int k = 0; k < TimeA.Count; k++)
            {

                Time1[j] = TimeA[k].Groups[0].Value;//将TimeA转化string
                MatchCollection TimeB = Regex.Matches(Time1[j], @"\d{2}");//提出分钟
                MatchCollection TimeC = Regex.Matches(Time1[j], @"\d{2}\.\d{2}");//提出秒
                Time1[j] = TimeB[0].Groups[0].Value;//MatchCollection转化string
                Time2[j] = TimeC[0].Groups[0].Value;//MatchCollection转化string
                float.TryParse(Time1[j], out Time[j]);//string转化float
                float.TryParse(Time2[j], out Timet);//string转化float
                Time[j] = Time[j] * 60 + Timet;
                Word[j] = Words[0].Groups[0].Value;
                j++;
            }
        }
        for (int i = 1; i <= count - 1; i++)
        {
            int j;
            float flag1 = Time[i];
            string flag2 = Word[i];
            for (j = i - 1; j >= 0; j--)
            {
                if (flag1 < Time[j])
                {
                    Time[j + 1] = Time[j];
                    Word[j + 1] = Word[j];
                }
                else
                    break;
            }
            Time[j + 1] = flag1;
            Word[j + 1] = flag2;
        }

    }

    void Find(float _audioTime)          //二分查找
    {
        int low = 0, high = count - 1, mid;
        while (low <= high)
        {
            mid = (low + high) / 2;
            if (_audioTime >= Time[mid - 1] && _audioTime <= Time[mid])
            {
                _index = mid - 1 ;
                return;
            }
            if (_audioTime < Time[mid - 1])
            {
                high = mid - 1;
            }
            else
            {
                low = mid + 1;
            }

        }
    }
}
