using System;

/// <summary>
/// IEventDispatcher: 이벤트 시스템 인터페이스
/// </summary>
public interface IEventDispatcher
{
    /// <summary>
    /// 이벤트 구독
    /// </summary>
    void Subscribe<T>(Action<T> handler) where T : IGameEvent;

    /// <summary>
    /// 이벤트 구독 해제
    /// </summary>
    void Unsubscribe<T>(Action<T> handler) where T : IGameEvent;

    /// <summary>
    /// 이벤트 발생
    /// </summary>
    void Dispatch<T>(T gameEvent) where T : IGameEvent;

    /// <summary>
    /// 모든 구독 해제
    /// </summary>
    void Clear();

    // TODO: [Phase 3] DelayedDispatch(event, delay) 지연 이벤트
    // TODO: [Phase 4] FilteredDispatch(event, condition) 조건부 이벤트
}

/// <summary>
/// 모든 게임 이벤트의 기본 인터페이스
/// </summary>
public interface IGameEvent
{
    float Timestamp { get; }
    string EventType { get; }
}