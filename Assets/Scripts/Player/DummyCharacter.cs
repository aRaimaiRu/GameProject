using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyCharacter : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> Bodyparts;
    [SerializeField] Text text;
    public void initlialize(Color color, string name, Color nameColor)
    {
        text.text = name;
        text.color = nameColor;
        SetColorRPC(color);
    }
    public void SetColorRPC(Color color)
    {
        foreach (SpriteRenderer _sr in Bodyparts)
        {
            _sr.color = color;
        }
    }
}
