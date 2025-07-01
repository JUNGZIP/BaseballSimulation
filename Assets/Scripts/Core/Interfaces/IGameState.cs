/// <summary>
/// IGameState: ���� ���� ���� �������̽�
/// </summary>
public interface IGameState
{
    // �⺻ ���� ����
    int CurrentInning { get; }
    bool IsTopHalf { get; }
    int OutCount { get; }
    int BallCount { get; }
    int StrikeCount { get; }

    // ���� ����
    int HomeScore { get; }
    int AwayScore { get; }

    // ���� ���� �޼ҵ�
    void NextInning();
    void NextOut();
    void ResetCount();
    void AddBall();
    void AddStrike();
    void AddScore(bool isHome, int score);

    // ���� Ȯ��
    bool IsGameOver();
    string GetWinner();

    // TODO: [Phase 2] BaseState GetBaseState() ���� ��Ȳ
    // TODO: [Phase 3] WeatherCondition, FieldCondition �߰�
}