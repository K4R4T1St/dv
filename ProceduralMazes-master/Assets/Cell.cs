using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject WallLeft;
    public GameObject WallBottom;

    public static implicit operator Cell(MazeGeneratorCell v)
    {
        throw new NotImplementedException();
    }
}