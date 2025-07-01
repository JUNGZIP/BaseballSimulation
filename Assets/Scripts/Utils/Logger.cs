using UnityEngine;
using System;

/// <summary>
/// Logger: 게임 로그 출력 유틸리티
/// </summary>
public static class Logger
{
    public static bool EnableDebugLog = true;
    public static bool EnableGameLog = true;

    /// <summary>
    /// 일반 로그
    /// </summary>
    public static void Log(string message)
    {
        if (EnableGameLog)
        {
            Debug.Log($"[GAME] {message}");
        }
    }

    /// <summary>
    /// 디버그 로그 (개발용)
    /// </summary>
    public static void LogDebug(string message)
    {
        if (EnableDebugLog)
        {
            Debug.Log($"[DEBUG] {message}");
        }
    }

    /// <summary>
    /// 경고 로그
    /// </summary>
    public static void LogWarning(string message)
    {
        Debug.LogWarning($"[WARN] {message}");
    }

    /// <summary>
    /// 에러 로그
    /// </summary>
    public static void LogError(string message)
    {
        Debug.LogError($"[ERROR] {message}");
    }

    /// <summary>
    /// 게임 이벤트 로그 (이벤트 전용)
    /// </summary>
    public static void LogEvent(IGameEvent gameEvent)
    {
        if (EnableGameLog)
        {
            Debug.Log($"[EVENT] {gameEvent.EventType} at {gameEvent.Timestamp:F2}s");
        }
    }

    /// <summary>
    /// 플레이 결과 로그 (포맷팅된 출력)
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
            PlayResult.Single => "안타!",
            PlayResult.Double => "2루타!",
            PlayResult.Triple => "3루타!",
            PlayResult.HomeRun => "홈런!",
            PlayResult.Strikeout => "삼진아웃",
            PlayResult.Groundout => "땅볼아웃",
            PlayResult.Flyout => "플라이아웃",
            PlayResult.Walk => "볼넷",
            PlayResult.HitByPitch => "몸에 맞는 공",
            _ => result.ToString()
        };
    }

    // TODO: [Phase 3] LogUI(message) UI 텍스트로 출력
    // TODO: [Phase 3] LogToFile(message) 파일 저장
    // TODO: [Phase 6] LogStatistics(data) 통계 로깅
}