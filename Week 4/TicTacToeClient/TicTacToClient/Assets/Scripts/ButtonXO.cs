using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public struct GridPOS
{
    public int X;
    public int Y;
    public GridPOS(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }

    public override string ToString()
    {
        return $"Grid Position: ({X}, {Y})";
    }
}
public class ButtonXO : MonoBehaviour
{
    public GridPOS pos;
    public TextMeshProUGUI textField;

    public void Init(GridPOS pos, UnityAction callback)
    {
        this.pos = pos;
        Button bttn = GetComponent<Button>();

        //bttn.onClick.AddListener(new UnityEngine.Events.UnityAction(ButtonClicked));
        //bttn.onClick.AddListener(() => ButtonClicked());
        bttn.onClick.AddListener(callback);

    }
    public void ButtonClicked()
    {
        print("Ooh that tickles...");
    }

    public void SetOwner(byte b)
    {
        if (b == 0)
        {
            textField.text = "";
            textField.text = "X";
            textField.text = "O";
        }
    }
}
