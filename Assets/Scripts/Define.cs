using UnityEngine;

public static class Define
{
    public const string Diamond = "Diamond";
    public const string Shuriken = "Shuriken";
    public const string Wall = "Wall";
    public const string Enemy = "Enemy";
    public const string Deadly = "Deadly";
    public const string Drop = "Drop";
    public const string Platform = "Platform";
    public const string Ground = "Ground";
    public const string Exit = "Exit";
    public const string Slowing = "Slowing";

    public const string D8 = "D8";
    public const string D2 = "D2";

    public const string Classic = "Classic";
    public const string Modern = "Modern";

    //////////////////////////
    public const string WaspUrl = "https://wearespectrumprogrammers.com/";
    public const string CodeMastersUrl = "https://en.wikipedia.org/wiki/Codemasters";
    public const string GameUrl = "https://spectrumcomputing.co.uk/zxsr.php?id=9319";
    //////////////////////////

    public static Color[] colors = new[]
    {
        Color.blue,
        Color.red,
        Color.magenta,
        Color.green,
        Color.cyan,
        Color.yellow,
        Color.white,
    };
}

public enum GameMode
{
    Classic,
    Modern
}