using UnityEngine;

/// <summary>
/// GameEvents: 게임 이벤트 정의
/// </summary>

/// <summary>
/// 게임 시작 이벤트
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
/// 이닝 변경 이벤트
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
/// 타석 결과 이벤트
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
/// 점수 변경 이벤트
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
/// 게임 종료 이벤트
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
/// 플레이 결과 열거형
/// </summary>
public enum PlayResult
{
    // 아웃
    Strikeout,
    Groundout,
    Flyout,

    // 안타
    Single,
    Double,
    Triple,
    HomeRun,

    // 기타
    Walk,
    HitByPitch,

    // TODO: [Phase 2] Bunt, SacriflyFly, FieldersChoice 등
    // TODO: [Phase 4] 구체적인 수비 위치별 아웃 (GroundoutTo1B 등)
}

/// <summary>
/// 게임 상태 데이터 (저장용)
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

    // TODO: [Phase 2] BaseState 주자 상황
    // TODO: [Phase 6] InningScores[] 이닝별 점수
}