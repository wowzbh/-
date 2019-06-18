using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SliderState      //判断Slider当前状态的枚举类型 ：默认，按下，松开
{
    Normal, Down, Up
}

public class SliderUI : Slider {
    public static SliderState State = SliderState.Normal;    //设置默认状态
    public override void OnPointerDown(PointerEventData eventData)      //重写Slider中的 OnPointerDown
    {
        base.OnPointerDown(eventData);
        State = SliderState.Down;
    }
    public override void OnPointerUp(PointerEventData eventData)      //重写Slider中的 OnPointerUp
    {
        base.OnPointerUp(eventData);
        State = SliderState.Up;
    }
    public void Reset()
    {
        State = SliderState.Normal;
    }

}
