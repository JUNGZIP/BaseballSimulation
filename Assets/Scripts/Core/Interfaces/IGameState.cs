/// <summary>
/// IGameState: 게임 상태 관리 인터페이스
/// </summary>
public interface IGameState
{
    // 기본 게임 상태
    int CurrentInning { get; }
    bool IsTopHalf { get; }
    int OutCount { get; }
    int BallCount { get; }
    int StrikeCount { get; }

    // 점수 관리
    int HomeScore { get; }
    int AwayScore { get; }

    // 상태 변경 메소드
    void NextInning();
    void NextOut();
    void ResetCount();
    void AddBall();
    void AddStrike();
    void AddScore(bool isHome, int score);

    // 상태 확인
    bool IsGameOver();
    string GetWinner();

    // TODO: [Phase 2] BaseState GetBaseState() 주자 상황
    // TODO: [Phase 3] WeatherCondition, FieldCondition 추가
}