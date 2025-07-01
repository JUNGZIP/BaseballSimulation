/// <summary>
/// IAIDecisionMaker: AI �ǻ���� �������̽�
/// </summary>
public interface IAIDecisionMaker
{
    /// <summary>
    /// AI ���� ���� (����� �ܼ�, ���� ��ȭ)
    /// </summary>
    AIDecision MakeDecision();

    /// <summary>
    /// AI ����ġ ����
    /// </summary>
    void SetDifficulty(AIDifficulty difficulty);

    // TODO: [Phase 4] MakeDecision(GameContext context, PlayerData data)
    // TODO: [Phase 4] GetDecisionConfidence() ���� �ŷڵ� ��ȯ
    // TODO: [Phase 5] LoadPersonality(PersonalityData data) ���� �ε�
}

/// <summary>
/// AI ���� ���
/// </summary>
public struct AIDecision
{
    public DecisionType Type;
    public float Confidence;
    public object Data; // ������ ���� �߰� ������

    // TODO: [Phase 4] Vector3 TargetLocation Ÿ�� ��ġ
    // TODO: [Phase 4] float[] UtilityScores IAUS�� ���� �迭
}

/// <summary>
/// AI ���� Ÿ��
/// </summary>
public enum DecisionType
{
    Swing,      // ����
    Watch,      // ���Ѻ�
    Pitch,      // ����
    Strategy    // ����

    // TODO: [Phase 3] Bunt, Steal, HitAndRun �� ���� �߰�
}

/// <summary>
/// AI ���̵�
/// </summary>
public enum AIDifficulty
{
    Easy,
    Normal,
    Hard,
    Expert

    // TODO: [Phase 5] Custom ���̵� �߰�
}