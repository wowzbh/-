using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/* Phyllotaxis 公式  --极坐标公式
 *  angle:   φ = n * degree
 *  radius:   r = c * √n
 *  x = r * cos(angle)
 *  y = r * sin(angle)
 *  degree : 0-360
 *  n: 0 - ∞
 *  c: 0.5
 */

/*TrailRenderer 组建实现拖尾
 * Lerping 控制拖尾运功轨迹与速度
 * 
 */
 /*         Vector3.Lerp (from: Vector3, to: Vector3, t: float)
  * t 在 [0 - 1], t = 0时 返回from, t =1 时返回 to, t = 0.5 时返回 from 与 to 的平均值
  */
public class Phyllotaxis : MonoBehaviour {

    public GameObject _dot;
    //定义公式参数
    public float _degree, _scale;
    public int _numberStart;
    private int _number;
    //拖尾渲染
    private TrailRenderer _trailRenderer;

    //lerping 控制循环
    public bool _userlerping;  //用户控制是否使用Lerping
    public float _intervalLerp;  //控制速度
    public int _stepSize;   //number 增加步数
    public int _maxIteration;  //执行lerp的最大次数
    private bool _isLerping;    //判断lerp的开始与结束
    private Vector3 _startPosition, _endPosition;
    private float _timeStartedLerping;
    private int _currentIteration;   //当前Lerp的次数

    private Vector2 _phyllotaxisPosition;
    //Phyllotaxis计算坐标
    private Vector2 CalculatePhyllotaxis(float degree, float scale, int number)
    {
        double angle = number * (degree * Mathf.Deg2Rad);
        float r = scale * Mathf.Sqrt(number);
        float x = r * (float)System.Math.Cos(angle);
        float y = r * (float)System.Math.Sin(angle);
        Vector2 vec = new Vector2(x, y);
        return vec;
    }

    //lerping 初始化
    void StartLerping()
    {
        _isLerping = true;
        _timeStartedLerping = Time.time;
        _phyllotaxisPosition = CalculatePhyllotaxis(_degree, _scale, _number);
        _startPosition = this.transform.localPosition;
        _endPosition = new Vector3(_phyllotaxisPosition.x, _phyllotaxisPosition.y, 0);
    }

    // Use this for initialization
    void Awake() {
        _trailRenderer = this.GetComponent<TrailRenderer>();
        _number = _numberStart;
        this.transform.localPosition = CalculatePhyllotaxis(_degree, _scale, _number);

        if (_userlerping)
        {
            StartLerping();
        }
    }

    private void FixedUpdate()
    {
        if (_userlerping)
        {
            if (_isLerping)
            {
                //计算轨迹运行时间
                float timeSinceStarted = Time.time - _timeStartedLerping;
                float percentageComplete = timeSinceStarted / _intervalLerp;
                //在percentageComplete时间内从_startPosition运动到_endPosition 
                this.transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);
                //当时间大于1s时开始下一个循环
                if (percentageComplete >= 0.97f)
                {
                    transform.localPosition = _endPosition;
                    _number += _stepSize;
                    _currentIteration++;
                    if (_currentIteration <= _maxIteration)
                    {
                        StartLerping();
                    }
                    else
                    {
                        _isLerping = false;
                    }
                }
            }
        }
        else
        {
            //坐标随_number的增加而变化
            _phyllotaxisPosition = CalculatePhyllotaxis(_degree, _scale, _number);
            this.transform.localPosition = new Vector3(_phyllotaxisPosition.x, _phyllotaxisPosition.y, 0);
            _number += _stepSize;
            _currentIteration++;
        }
    }

}
