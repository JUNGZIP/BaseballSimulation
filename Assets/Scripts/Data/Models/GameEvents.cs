using UnityEngine;

/// <summary>
/// GameEvents: ���� �̺�Ʈ ����
/// </summary>

/// <summary>
/// ���� ���� �̺�Ʈ
/// </summary>
public struct GameStartEvent : IGameEvent
{
    public float Timestamp { get; private set; }
    public string EventType => "GameStart";

    public string HomeTeamName;
    public string AwayTeamName;

    public GameStartEvent(string homeTeam, string awayTeam)
    {
        Timestamp = Time.time;
        HomeTeamName = homeTeam;
        AwayTeamName = awayTeam;
    }
}

/// <summary>
/// �̴� ���� �̺�Ʈ
/// </summary>
public struct InningChangeEvent : IGameEvent
{
    public float Timestamp { get; private set; }
    public string EventType => "InningChange";

    public int Inning;
    public bool IsTopHalf;

    public InningChangeEvent(int inning, bool isTopHalf)
    {
        Timestamp = Time.time;
        Inning = inning;
        IsTopHalf = isTopHalf;
    }
}

/// <summary>
/// Ÿ�� ��� �̺�Ʈ
/// </summary>
public struct AtBatResultEvent : IGameEvent
{
    public float Timestamp { get; private set; }
    public string EventType => "AtBatResult";

    public string BatterName;
    public string PitcherName;
    public PlayResult Result;

    public AtBatResultEvent(string batterName, string pitcherName, PlayResult result)
    {
        Timestamp = Time.time;
        BatterName = batterName;
        PitcherName = pitcherName;
        Result = result;
    }
}

/// <summary>
/// ���� ���� �̺�Ʈ
/// </summary>
public struct ScoreChangeEvent : IGameEvent
{
    public float Timestamp { get; private set; }
    public string EventType => "ScoreChange";

    public bool IsHome;
    public int NewScore;
    public int RunsScored;

    public ScoreChangeEvent(bool isHome, int newScore, int runsScored)
    {
        Timestamp = Time.time;
        IsHome = isHome;
        NewScore = newScore;
        RunsScored = runsScored;
    }
}

/// <summary>
/// ���� ���� �̺�Ʈ
/// </summary>
public struct GameEndEvent : IGameEvent
{
    public float Timestamp { get; private set; }
    public string EventType => "GameEnd";

    public string Winner;
    public int HomeScore;
    public int AwayScore;

    public GameEndEvent(string winner, int homeScore, int awayScore)
    {
        Timestamp = Time.time;
        Winner = winner;
        HomeScore = homeScore;
        AwayScore = awayScore;
    }
}

/// <summary>
/// �÷��� ��� ������
/// </summary>
public enum PlayResult
{
    // �ƿ�
    Strikeout,
    Groundout,
    Flyout,

    // ��Ÿ
    Single,
    Double,
    Triple,
    HomeRun,

    // ��Ÿ
    Walk,
    HitByPitch,

    // TODO: [Phase 2] Bunt, SacriflyFly, FieldersChoice ��
    // TODO: [Phase 4] ��ü���� ���� ��ġ�� �ƿ� (GroundoutTo1B ��)
}

/// <summary>
/// ���� ���� ������ (�����)
/// </summary>
[System.Serializable]
public struct GameStateData
{
    public int CurrentInning;
    public bool IsTopHalf;
    public int OutCount;
    public int BallCount;
    public int StrikeCount;
    public int HomeScore;
    public int AwayScore;

    // TODO: [Phase 2] BaseState ���� ��Ȳ
    // TODO: [Phase 6] InningScores[] �̴׺� ����
}