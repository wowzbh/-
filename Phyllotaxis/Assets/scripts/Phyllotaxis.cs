using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/* Phyllotaxis 公式  --极坐标公式
 *  angle:   φ = n * degree
 *  radius:   r = c * √n
 *  x = r * cos(angle)
 *  y = r * sin(angle)
 *  degree : 137.5
 *  n: 1
 *  c: 0.5
 */
public class Phyllotaxis : MonoBehaviour {

    public GameObject _dot;
    //定义公式参数
    public float _degree, _c;
    public int _n;
    public float _dotScale;

    private Vector2 _phyllotaxisPosition;
    //Phyllotaxis计算坐标
    private Vector2 CalculatePhyllotaxis(float degree, float scale, int count)
    {
        double angle = count * (degree * Mathf.Deg2Rad);
        float r = scale * Mathf.Sqrt(count);
        float x = r * (float)System.Math.Cos(angle);
        float y = r * (float)System.Math.Sin(angle);
        Vector2 vec = new Vector2(x, y);
        return vec;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            _phyllotaxisPosition = CalculatePhyllotaxis(_degree, _c, _n);
            //更新新的dot
            GameObject dotInstance = (GameObject)Instantiate(_dot);
            //设置更新物体的位置与大小
            dotInstance.transform.position = new Vector3(_phyllotaxisPosition.x, _phyllotaxisPosition.y, 0);
            dotInstance.transform.localScale = new Vector3(_dotScale, _dotScale, _dotScale);
            _n++;
        }
	}
}
