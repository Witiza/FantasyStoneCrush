public struct GameEndInfo
{
    public GameEndInfo(int remainingMoves,int score, int level, int highestLevel,bool gameWon)
    {
        _remainingMoves = remainingMoves;
        _score = score;
        _level = level;
        _highestLevel = highestLevel;
        _gameWon = gameWon;
    }
    public int _remainingMoves;
    public int _score;
    public int _level;
    public int _highestLevel;
    public bool _gameWon;
}
