using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// GameStateManager: 게임 상태 관리 구현체
/// </summary>
public class GameStateManager : IGameState, IGameSystem
{
    // IGameState 속성들
    public int CurrentInning { get; private set; } = 1;
    public bool IsTopHalf { get; private set; } = true;
    public int OutCount { get; private set; } = 0;
    public int BallCount { get; private set; } = 0;
    public int StrikeCount { get; private set; } = 0;
    public int HomeScore { get; private set; } = 0;
    public int AwayScore { get; private set; } = 0;

    // 이벤트 시스템
    private IEventDispatcher eventDispatcher;

    // 게임 설정
    private const int MAX_INNINGS = 9;
    private const int MAX_OUTS = 3;
    private const int MAX_BALLS = 4;
    private const int MAX_STRIKES = 3;

    public void Initialize()
    {
        eventDispatcher = GameManager.Instance?.GetComponent<EventDispatcher>();
        ResetGame();
        Logger.LogDebug("GameStateManager 초기화 완료");
    }

    public void ProcessTurn()
    {
        // 상태 관리자는 턴마다 특별한 처리 없음
        // TODO: [Phase 6] 이닝별 통계 업데이트
    }

    public void Cleanup()
    {
        Logger.LogDebug("GameStateManager 정리");
    }

    /// <summary>
    /// 게임 초기화
    /// </summary>
    private void ResetGame()
    {
        CurrentInning = 1;
        IsTopHalf = true;
        OutCount = 0;
        ResetCount();
        HomeScore = 0;
        AwayScore = 0;

        Logger.LogDebug("게임 상태 초기화");
    }

    public void NextInning()
    {
        if (IsTopHalf)
        {
            // 초 → 말
            IsTopHalf = false;
        }
        else
        {
            // 말 → 다음 이닝 초
            IsTopHalf = true;
            CurrentInning++;
        }

        OutCount = 0;
        ResetCount();

        // 이닝 변경 이벤트
        var inningEvent = new InningChangeEvent(CurrentInning, IsTopHalf);
        eventDispatcher?.Dispatch(inningEvent);

        string half = IsTopHalf ? "초" : "말";
        Logger.Log($"--- {CurrentInning}회 {half} 시작 ---");
    }

    public void NextOut()
    {
        OutCount++;
        ResetCount();

        Logger.LogDebug($"아웃카운트: {OutCount}/{MAX_OUTS}");

        if (OutCount >= MAX_OUTS)
        {
            Logger.Log($"{MAX_OUTS}아웃, 공수교대");
            NextInning();
        }
    }

    public void ResetCount()
    {
        BallCount = 0;
        StrikeCount = 0;
    }

    public void AddBall()
    {
        BallCount++;
        Logger.LogDebug($"볼카운트: {BallCount}-{StrikeCount}");

        if (BallCount >= MAX_BALLS)
        {
            Logger.Log("볼넷!");
            // TODO: [Phase 2] 주자 진루 처리
            ResetCount();
        }
    }

    public void AddStrike()
    {
        StrikeCount++;
        Logger.LogDebug($"볼카운트: {BallCount}-{StrikeCount}");

        if (StrikeCount >= MAX_STRIKES)
        {
            Logger.Log("삼진아웃!");
            NextOut();
        }
    }

    public void AddScore(bool isHome, int score)
    {
        int oldScore = isHome ? HomeScore : AwayScore;

        if (isHome)
        {
            HomeScore += score;
        }
        else
        {
            AwayScore += score;
        }

        // 점수 변경 이벤트
        var scoreEvent = new ScoreChangeEvent(isHome, isHome ? HomeScore : AwayScore, score);
        eventDispatcher?.Dispatch(scoreEvent);

        string teamName = isHome ? "홈팀" : "원정팀";
        Logger.Log($"{teamName} {score}점 득점! (총 {(isHome ? HomeScore : AwayScore)}점)");
    }

    public bool IsGameOver()
    {
        // 기본 9회 완료 체크
        if (CurrentInning > MAX_INNINGS)
        {
            return true;
        }

        // 9회말에 홈팀이 이기고 있으면 게임 종료
        if (CurrentInning == MAX_INNINGS && !IsTopHalf && HomeScore > AwayScore)
        {
            return true;
        }

        // TODO: [Phase 2] 연장전 로직 추가
        // TODO: [Phase 2] 콜드게임 규칙 추가

        return false;
    }

    public string GetWinner()
    {
        if (!IsGameOver()) return "게임 진행중";

        if (HomeScore > AwayScore)
        {
            return "홈팀";
        }
        else if (AwayScore > HomeScore)
        {
            return "원정팀";
        }
        else
        {
            return "무승부";
        }
    }

    /// <summary>
    /// 현재 상태 데이터 추출 (저장용)
    /// </summary>
    public GameStateData GetStateData()
    {
        return new GameStateData
        {
            CurrentInning = this.CurrentInning,
            IsTopHalf = this.IsTopHalf,
            OutCount = this.OutCount,
            BallCount = this.BallCount,
            StrikeCount = this.StrikeCount,
            HomeScore = this.HomeScore,
            AwayScore = this.AwayScore
        };
    }

    /// <summary>
    /// 상태 데이터로부터 복원 (로드용)
    /// </summary>
    public void LoadStateData(GameStateData data)
    {
        CurrentInning = data.CurrentInning;
        IsTopHalf = data.IsTopHalf;
        OutCount = data.OutCount;
        BallCount = data.BallCount;
        StrikeCount = data.StrikeCount;
        HomeScore = data.HomeScore;
        AwayScore = data.AwayScore;

        Logger.Log("게임 상태 복원 완료");
    }

    // TODO: [Phase 2] GetBaseState() 주자 상황 반환
    // TODO: [Phase 3] GetInningScores() 이닝별 점수 반환
    // TODO: [Phase 6] UpdateStatistics() 경기 통계 업데이트
}