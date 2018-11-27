using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {









    enum Vector { up, right, down, left }

    Cell[,] board;
    int xMax, yMax;

    float rand = 0.2f;

    void Start() {
        xMax = yMax = 5;

        board = new Cell[xMax, yMax];

        // 0 1 2 3 4
        for (int x = 0; x < xMax; x++) {
            for (int y = 0; y < yMax; y++) {
                board[x, y] = new Cell();
            }
        }

        RandCreate();
        Draw();
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            Move(Vector.up);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Move(Vector.right);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            Move(Vector.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            Move(Vector.left);
        }
    }


    void RandCreate() {
        foreach (var cell in board) {
            if (cell.scale == 0 && UnityEngine.Random.value <= rand) {
                cell.scale = 2;
            }
        }
    }
    // 0 1 2 3
    // 1
    // 2
    // 3


    void Move(Vector vec) {
        int xVec = vec == Vector.right ? 1 : vec == Vector.left ? -1 : 0;
        int yVec = vec == Vector.up ? 1 : vec == Vector.down ? -1 : 0;



        bool vecInv = false;
        if (vec == Vector.right || vec == Vector.up) vecInv = true;



        // Первыми обрабатываются ближайшие к границе направления плитки
        for (int x = vecInv ? xMax - 1 : 0; x < xMax && x >= 0; x = vecInv ? x - 1 : x + 1) {
            for (int y = vecInv ? yMax - 1 : 0; y < yMax && y >= 0; y = vecInv ? y - 1 : y + 1) {



                Cell cell = board[x, y];
                var scale = cell.scale;
                var content = cell.cont;
                if (scale > 0) {

                    int xMove = x;
                    int yMove = y;

                    while (true) {
                        int xNext = xMove + xVec,
                            yNext = yMove + yVec;


                        if (xNext >= xMax || yNext >= yMax || xNext < 0 || yNext < 0 || board[xNext, yNext].scale > 0 && board[xNext, yNext].scale != cell.scale) {

                            // Если следующей не существует или она занята
                            if (xMove != x || yMove != y) {
                                //cell.scale = 0;
                                //cell.cont = null;
                                board[xMove, yMove].scale = scale;

                                Content.MoveFromTo(cell, board[xMove, yMove], xMove, yMove);
                            }
                            break;

                        }
                        else if (board[xNext, yNext].scale == cell.scale) {
                            // Если значение следующей равно значению текущей
                            //cell.scale = 0;
                            board[xNext, yNext].scale = scale * 2;

                            Content.MoveFromTo(cell, board[xMove, yMove], xMove, yMove);
                            break;

                        }
                        else {
                            xMove += xVec;
                            yMove += yVec;
                        }
                    }
                }
            }
        }
        RandCreate();
        Draw();
    }





    [SerializeField] Text text;

    void Draw() {
        string str = "";
        for (int y = yMax - 1; y >= 0; y--) {
            for (int x = 0; x < xMax; x++) {
                str += "\t\t" + board[x, y].scale.ToString();
            }
            str += "\n\n";
        }

        text.text = str;
    }

    IEnumerator Animation() {
        yield return null;
    }
}



public class Cell {
    public int scale = 0;
    public Content cont = null;
}


public class Content : MonoBehaviour {
    public int scale = 0;

    float xPos, yPos, xTarget, yTarget;


    public void Move(float x, float y) {
        xTarget = x;
        yTarget = y;
    }





    public static void MoveFromTo(Cell a, Cell b, float x, float y) {
        a.scale = 0;
        b.cont = a.cont;
        a.cont = null;
        a.cont.Move(x, y);
    }
}







