using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EventDispatcher: �̺�Ʈ �ý��� ����ü (Observer Pattern)
/// </summary>
public class EventDispatcher : MonoBehaviour, IEventDispatcher
{
    // �̺�Ʈ Ÿ�Ժ� �ڵ鷯 ����
    private Dictionary<Type, List<Delegate>> eventHandlers = new Dictionary<Type, List<Delegate>>();

    private void Awake()
    {
        Logger.LogDebug("EventDispatcher �ʱ�ȭ");
    }

    public void Subscribe<T>(Action<T> handler) where T : IGameEvent
    {
        Type eventType = typeof(T);

        if (!eventHandlers.ContainsKey(eventType))
        {
            eventHandlers[eventType] = new List<Delegate>();
        }

        eventHandlers[eventType].Add(handler);
        Logger.LogDebug($"�̺�Ʈ ����: {eventType.Name}");
    }

    public void Unsubscribe<T>(Action<T> handler) where T : IGameEvent
    {
        Type eventType = typeof(T);

        if (eventHandlers.ContainsKey(eventType))
        {
            eventHandlers[eventType].Remove(handler);

            // �� ����Ʈ ����
            if (eventHandlers[eventType].Count == 0)
            {
                eventHandlers.Remove(eventType);
            }

            Logger.LogDebug($"�̺�Ʈ ���� ����: {eventType.Name}");
        }
    }

    public void Dispatch<T>(T gameEvent) where T : IGameEvent
    {
        Type eventType = typeof(T);

        // �̺�Ʈ �α�
        Logger.LogEvent(gameEvent);

        // �ڵ鷯 ����
        if (eventHandlers.ContainsKey(eventType))
        {
            var handlers = eventHandlers[eventType];

            // �ڵ鷯 ����Ʈ ���� (���� �� ���� ����)
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
                    Logger.LogError($"�̺�Ʈ �ڵ鷯 ���� ����: {ex.Message}");
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
        Logger.LogDebug($"��� �̺�Ʈ �ڵ鷯 ���� �Ϸ� (�� {totalHandlers}��)");
    }

    /// <summary>
    /// ��ϵ� �̺�Ʈ Ÿ�� �� ��ȯ (����׿�)
    /// </summary>
    public int GetEventTypeCount()
    {
        return eventHandlers.Count;
    }

    /// <summary>
    /// Ư�� �̺�Ʈ Ÿ���� �ڵ鷯 �� ��ȯ (����׿�)
    /// </summary>
    public int GetHandlerCount<T>() where T : IGameEvent
    {
        Type eventType = typeof(T);
        return eventHandlers.ContainsKey(eventType) ? eventHandlers[eventType].Count : 0;
    }

    /// <summary>
    /// �̺�Ʈ �ý��� ���� �α� ��� (����׿�)
    /// </summary>
    [ContextMenu("Debug Event System")]
    public void DebugEventSystem()
    {
        Logger.LogDebug("=== Event System Status ===");
        Logger.LogDebug($"�� �̺�Ʈ Ÿ��: {eventHandlers.Count}");

        foreach (var kvp in eventHandlers)
        {
            Logger.LogDebug($"- {kvp.Key.Name}: {kvp.Value.Count}�� �ڵ鷯");
        }
    }

    private void OnDestroy()
    {
        Clear();
    }

    // TODO: [Phase 3] DelayedDispatch ���� �̺�Ʈ ����
    // TODO: [Phase 3] QueuedDispatch ť ��� �̺�Ʈ ó��
    // TODO: [Phase 4] FilteredDispatch ���Ǻ� �̺�Ʈ �߼�
    // TODO: [Phase 6] EventHistory �̺�Ʈ ��� ���
}