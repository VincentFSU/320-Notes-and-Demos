﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Player
{
    Nobody,
    PlayerX,
    PlayerO
}
public class ControllerGameplay : MonoBehaviour
{
    private int columns = 3;
    private int rows = 3;

    public ButtonXO bttnPrefab;

    private Player whoseTurn = Player.PlayerX;
    private Player[,] boardData; // all of the data of who owns what
    private ButtonXO[,] boardUI; // all of the buttons

    public Transform panelGameBoard; // grid of buttons

    void Start()
    {
        BuildBoardUI();    
    }

    private void BuildBoardUI()
    {
        boardUI = new ButtonXO[columns, rows]; // instantiating array for buttons

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                ButtonXO bttn = Instantiate(bttnPrefab, panelGameBoard);
                bttn.Init(new GridPOS(x,y), ()=> { ButtonClicked(bttn); }); // anonymous functions have access to all variables in their scope when declared
                boardUI[x, y] = bttn;
            }
        }
    }

    void ButtonClicked(ButtonXO bttn)
    {
        //print($"X:{bttn.pos.X} Y:{bttn.pos.Y} was clicked");
        print($"Button was clicked. {bttn.pos} ");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}