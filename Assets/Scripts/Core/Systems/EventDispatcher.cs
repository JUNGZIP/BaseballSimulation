using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EventDispatcher: 이벤트 시스템 구현체 (Observer Pattern)
/// </summary>
public class EventDispatcher : MonoBehaviour, IEventDispatcher
{
    // 이벤트 타입별 핸들러 저장
    private Dictionary<Type, List<Delegate>> eventHandlers = new Dictionary<Type, List<Delegate>>();

    private void Awake()
    {
        Logger.LogDebug("EventDispatcher 초기화");
    }

    public void Subscribe<T>(Action<T> handler) where T : IGameEvent
    {
        Type eventType = typeof(T);

        if (!eventHandlers.ContainsKey(eventType))
        {
            eventHandlers[eventType] = new List<Delegate>();
        }

        eventHandlers[eventType].Add(handler);
        Logger.LogDebug($"이벤트 구독: {eventType.Name}");
    }

    public void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
    {
        Type eventType = typeof(T);

        if (eventHandlers.ContainsKey(eventType))
        {
            eventHandlers[eventType].Remove(handler);

            // 빈 리스트 정리
            if (eventHandlers[eventType].Count == 0)
            {
                eventHandlers.Remove(eventType);
            }

            Logger.LogDebug($"이벤트 구독 해제: {eventType.Name}");
        }
    }

    public void Dispatch<T>(T gameEvent) where T : IGameEvent
    {
        Type eventType = typeof(T);

        // 이벤트 로깅
        Logger.LogEvent(gameEvent);

        // 핸들러 실행
        if (eventHandlers.ContainsKey(eventType))
        {
            var handlers = eventHandlers[eventType];

            // 핸들러 리스트 복사 (실행 중 수정 방지)
            var handlersCopy = new List<Delegate>(handlers);

            foreach (var handler in handlersCopy)
            {
                try
                {
                    if (handler is Action<T> typedHandler)
                    {
                        typedHandler.Invoke(gameEvent);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"이벤트 핸들러 실행 오류: {ex.Message}");
                }
            }
        }
    }

    public void Clear()
    {
        int totalHandlers = 0;
        foreach (var handlers in eventHandlers.Values)
        {
            totalHandlers += handlers.Count;
        }

        eventHandlers.Clear();
        Logger.LogDebug($"모든 이벤트 핸들러 정리 완료 (총 {totalHandlers}개)");
    }

    /// <summary>
    /// 등록된 이벤트 타입 수 반환 (디버그용)
    /// </summary>
    public int GetEventTypeCount()
    {
        return eventHandlers.Count;
    }

    /// <summary>
    /// 특정 이벤트 타입의 핸들러 수 반환 (디버그용)
    /// </summary>
    public int GetHandlerCount<T>() where T : IGameEvent
    {
        Type eventType = typeof(T);
        return eventHandlers.ContainsKey(eventType) ? eventHandlers[eventType].Count : 0;
    }

    /// <summary>
    /// 이벤트 시스템 상태 로그 출력 (디버그용)
    /// </summary>
    [ContextMenu("Debug Event System")]
    public void DebugEventSystem()
    {
        Logger.LogDebug("=== Event System Status ===");
        Logger.LogDebug($"총 이벤트 타입: {eventHandlers.Count}");

        foreach (var kvp in eventHandlers)
        {
            Logger.LogDebug($"- {kvp.Key.Name}: {kvp.Value.Count}개 핸들러");
        }
    }

    private void OnDestroy()
    {
        Clear();
    }

    // TODO: [Phase 3] DelayedDispatch 지연 이벤트 구현
    // TODO: [Phase 3] QueuedDispatch 큐 기반 이벤트 처리
    // TODO: [Phase 4] FilteredDispatch 조건부 이벤트 발송
    // TODO: [Phase 6] EventHistory 이벤트 기록 기능
}