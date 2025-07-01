using System;

/// <summary>
/// IEventDispatcher: �̺�Ʈ �ý��� �������̽�
/// </summary>
public interface IEventDispatcher
{
    /// <summary>
    /// �̺�Ʈ ����
    /// </summary>
    void Subscribe<T>(Action<T> handler) where T : IGameEvent;

    /// <summary>
    /// �̺�Ʈ ���� ����
    /// </summary>
    void Unsubscribe<T>(Action<T> handler) where T : IGameEvent;

    /// <summary>
    /// �̺�Ʈ �߻�
    /// </summary>
    void Dispatch<T>(T gameEvent) where T : IGameEvent;

    /// <summary>
    /// ��� ���� ����
    /// </summary>
    void Clear();

    // TODO: [Phase 3] DelayedDispatch(event, delay) ���� �̺�Ʈ
    // TODO: [Phase 4] FilteredDispatch(event, condition) ���Ǻ� �̺�Ʈ
}

/// <summary>
/// ��� ���� �̺�Ʈ�� �⺻ �������̽�
/// </summary>
public interface IGameEvent
{
    float Timestamp { get; }
    string EventType { get; }
}