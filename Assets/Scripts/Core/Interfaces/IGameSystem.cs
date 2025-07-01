/// <summary>
/// IGameSystem: 모든 게임 시스템의 기본 인터페이스
/// </summary>
public interface IGameSystem
{
    /// <summary>
    /// 시스템 초기화
    /// </summary>
    void Initialize();

    /// <summary>
    /// 턴 처리 (현재는 단순, 향후 매개변수 추가)
    /// </summary>
    void ProcessTurn();

    /// <summary>
    /// 시스템 정리
    /// </summary>
    void Cleanup();

    // TODO: [Phase 4] ProcessTurn(GameState state, AIContext context) 고도화
    // TODO: [Phase 3] GetSystemPriority() 우선순위 반환
}