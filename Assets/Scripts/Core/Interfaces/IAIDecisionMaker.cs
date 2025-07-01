/// <summary>
/// IAIDecisionMaker: AI 의사결정 인터페이스
/// </summary>
public interface IAIDecisionMaker
{
    /// <summary>
    /// AI 결정 실행 (현재는 단순, 향후 고도화)
    /// </summary>
    AIDecision MakeDecision();

    /// <summary>
    /// AI 가중치 설정
    /// </summary>
    void SetDifficulty(AIDifficulty difficulty);

    // TODO: [Phase 4] MakeDecision(GameContext context, PlayerData data)
    // TODO: [Phase 4] GetDecisionConfidence() 결정 신뢰도 반환
    // TODO: [Phase 5] LoadPersonality(PersonalityData data) 개성 로드
}

/// <summary>
/// AI 결정 결과
/// </summary>
public struct AIDecision
{
    public DecisionType Type;
    public float Confidence;
    public object Data; // 결정에 따른 추가 데이터

    // TODO: [Phase 4] Vector3 TargetLocation 타겟 위치
    // TODO: [Phase 4] float[] UtilityScores IAUS용 점수 배열
}

/// <summary>
/// AI 결정 타입
/// </summary>
public enum DecisionType
{
    Swing,      // 스윙
    Watch,      // 지켜봄
    Pitch,      // 투구
    Strategy    // 전략

    // TODO: [Phase 3] Bunt, Steal, HitAndRun 등 전략 추가
}

/// <summary>
/// AI 난이도
/// </summary>
public enum AIDifficulty
{
    Easy,
    Normal,
    Hard,
    Expert

    // TODO: [Phase 5] Custom 난이도 추가
}