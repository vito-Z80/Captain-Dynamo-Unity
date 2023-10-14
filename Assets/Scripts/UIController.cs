using System;
using Game;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMesh textScores;
    public TextMesh textDiamonds;


    public GameData gameData;

    private const string D8 = "D8";
    private const string D3 = "D3";

    private void FixedUpdate()
    {
        SetScores();
    }


    private void SetScores()
    {
        if (gameData.HasScoresUpdate()) textScores.text = gameData.scores.ToString(D8);
        if (gameData.HasDiamondUpdate()) textDiamonds.text = gameData.diamondsCollected.ToString(D3);
    }
}