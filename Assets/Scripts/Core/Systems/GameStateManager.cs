using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// GameStateManager: ���� ���� ���� ����ü
/// </summary>
public class GameStateManager : IGameState, IGameSystem
{
    // IGameState �Ӽ���
    public int CurrentInning { get; private set; } = 1;
    public bool IsTopHalf { get; private set; } = true;
    public int OutCount { get; private set; } = 0;
    public int BallCount { get; private set; } = 0;
    public int StrikeCount { get; private set; } = 0;
    public int HomeScore { get; private set; } = 0;
    public int AwayScore { get; private set; } = 0;

    // �̺�Ʈ �ý���
    private IEventDispatcher eventDispatcher;

    // ���� ����
    private const int MAX_INNINGS = 9;
    private const int MAX_OUTS = 3;
    private const int MAX_BALLS = 4;
    private const int MAX_STRIKES = 3;

    public void Initialize()
    {
        eventDispatcher = GameManager.Instance?.GetComponent<EventDispatcher>();
        ResetGame();
        Logger.LogDebug("GameStateManager �ʱ�ȭ �Ϸ�");
    }

    public void ProcessTurn()
    {
        // ���� �����ڴ� �ϸ��� Ư���� ó�� ����
        // TODO: [Phase 6] �̴׺� ��� ������Ʈ
    }

    public void Cleanup()
    {
        Logger.LogDebug("GameStateManager ����");
    }

    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    private void ResetGame()
    {
        CurrentInning = 1;
        IsTopHalf = true;
        OutCount = 0;
        ResetCount();
        HomeScore = 0;
        AwayScore = 0;

        Logger.LogDebug("���� ���� �ʱ�ȭ");
    }

    public void NextInning()
    {
        if (IsTopHalf)
        {
            // �� �� ��
            IsTopHalf = false;
        }
        else
        {
            // �� �� ���� �̴� ��
            IsTopHalf = true;
            CurrentInning++;
        }

        OutCount = 0;
        ResetCount();

        // �̴� ���� �̺�Ʈ
        var inningEvent = new InningChangeEvent(CurrentInning, IsTopHalf);
        eventDispatcher?.Dispatch(inningEvent);

        string half = IsTopHalf ? "��" : "��";
        Logger.Log($"--- {CurrentInning}ȸ {half} ���� ---");
    }

    public void NextOut()
    {
        OutCount++;
        ResetCount();

        Logger.LogDebug($"�ƿ�ī��Ʈ: {OutCount}/{MAX_OUTS}");

        if (OutCount >= MAX_OUTS)
        {
            Logger.Log($"{MAX_OUTS}�ƿ�, ��������");
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
        Logger.LogDebug($"��ī��Ʈ: {BallCount}-{StrikeCount}");

        if (BallCount >= MAX_BALLS)
        {
            Logger.Log("����!");
            // TODO: [Phase 2] ���� ���� ó��
            ResetCount();
        }
    }

    public void AddStrike()
    {
        StrikeCount++;
        Logger.LogDebug($"��ī��Ʈ: {BallCount}-{StrikeCount}");

        if (StrikeCount >= MAX_STRIKES)
        {
            Logger.Log("�����ƿ�!");
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

        // ���� ���� �̺�Ʈ
        var scoreEvent = new ScoreChangeEvent(isHome, isHome ? HomeScore : AwayScore, score);
        eventDispatcher?.Dispatch(scoreEvent);

        string teamName = isHome ? "Ȩ��" : "������";
        Logger.Log($"{teamName} {score}�� ����! (�� {(isHome ? HomeScore : AwayScore)}��)");
    }

    public bool IsGameOver()
    {
        // �⺻ 9ȸ �Ϸ� üũ
        if (CurrentInning > MAX_INNINGS)
        {
            return true;
        }

        // 9ȸ���� Ȩ���� �̱�� ������ ���� ����
        if (CurrentInning == MAX_INNINGS && !IsTopHalf && HomeScore > AwayScore)
        {
            return true;
        }

        // TODO: [Phase 2] ������ ���� �߰�
        // TODO: [Phase 2] �ݵ���� ��Ģ �߰�

        return false;
    }

    public string GetWinner()
    {
        if (!IsGameOver()) return "���� ������";

        if (HomeScore > AwayScore)
        {
            return "Ȩ��";
        }
        else if (AwayScore > HomeScore)
        {
            return "������";
        }
        else
        {
            return "���º�";
        }
    }

    /// <summary>
    /// ���� ���� ������ ���� (�����)
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
    /// ���� �����ͷκ��� ���� (�ε��)
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

        Logger.Log("���� ���� ���� �Ϸ�");
    }

    // TODO: [Phase 2] GetBaseState() ���� ��Ȳ ��ȯ
    // TODO: [Phase 3] GetInningScores() �̴׺� ���� ��ȯ
    // TODO: [Phase 6] UpdateStatistics() ��� ��� ������Ʈ
}