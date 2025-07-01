using UnityEngine;
using System;

/// <summary>
/// Logger: ���� �α� ��� ��ƿ��Ƽ
/// </summary>
public static class Logger
{
    public static bool EnableDebugLog = true;
    public static bool EnableGameLog = true;

    /// <summary>
    /// �Ϲ� �α�
    /// </summary>
    public static void Log(string message)
    {
        if (EnableGameLog)
        {
            Debug.Log($"[GAME] {message}");
        }
    }

    /// <summary>
    /// ����� �α� (���߿�)
    /// </summary>
    public static void LogDebug(string message)
    {
        if (EnableDebugLog)
        {
            Debug.Log($"[DEBUG] {message}");
        }
    }

    /// <summary>
    /// ��� �α�
    /// </summary>
    public static void LogWarning(string message)
    {
        Debug.LogWarning($"[WARN] {message}");
    }

    /// <summary>
    /// ���� �α�
    /// </summary>
    public static void LogError(string message)
    {
        Debug.LogError($"[ERROR] {message}");
    }

    /// <summary>
    /// ���� �̺�Ʈ �α� (�̺�Ʈ ����)
    /// </summary>
    public static void LogEvent(IGameEvent gameEvent)
    {
        if (EnableGameLog)
        {
            Debug.Log($"[EVENT] {gameEvent.EventType} at {gameEvent.Timestamp:F2}s");
        }
    }

    /// <summary>
    /// �÷��� ��� �α� (�����õ� ���)
    /// </summary>
    public static void LogPlay(string batterName, string pitcherName, PlayResult result)
    {
        string resultText = GetPlayResultText(result);
        Log($"{batterName} vs {pitcherName}: {resultText}");
    }

    private static string GetPlayResultText(PlayResult result)
    {
        return result switch
        {
            PlayResult.Single => "��Ÿ!",
            PlayResult.Double => "2��Ÿ!",
            PlayResult.Triple => "3��Ÿ!",
            PlayResult.HomeRun => "Ȩ��!",
            PlayResult.Strikeout => "�����ƿ�",
            PlayResult.Groundout => "�����ƿ�",
            PlayResult.Flyout => "�ö��̾ƿ�",
            PlayResult.Walk => "����",
            PlayResult.HitByPitch => "���� �´� ��",
            _ => result.ToString()
        };
    }

    // TODO: [Phase 3] LogUI(message) UI �ؽ�Ʈ�� ���
    // TODO: [Phase 3] LogToFile(message) ���� ����
    // TODO: [Phase 6] LogStatistics(data) ��� �α�
}