using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// IDataProvider: 데이터 제공 인터페이스 (CSV/JSON/DB 추상화)
/// </summary>
public interface IDataProvider
{
    /// <summary>
    /// 초기화
    /// </summary>
    Task<bool> Initialize();

    /// <summary>
    /// 팀 데이터 로드
    /// </summary>
    Task<List<TeamData>> LoadTeamData();

    /// <summary>
    /// 선수 데이터 로드
    /// </summary>
    Task<List<PlayerData>> LoadPlayerData();

    /// <summary>
    /// 특정 선수 데이터 조회
    /// </summary>
    Task<PlayerData> GetPlayerData(string playerId);

    /// <summary>
    /// 데이터 저장 (세이브/로드용)
    /// </summary>
    Task<bool> SaveGameData(GameSaveData data);

    // TODO: [Phase 5] LoadSeasonData() 시즌 데이터
    // TODO: [Phase 5] LoadStatsData() 실제 통계 데이터
    // TODO: [Phase 6] LoadTradingData() 트레이드 데이터
}

/// <summary>
/// 팀 데이터 구조
/// </summary>
[System.Serializable]
public struct TeamData
{
    public string TeamId;
    public string TeamName;
    public string[] PlayerIds;
    public string PitcherId;

    // TODO: [Phase 6] Budget, Reputation, FacilityLevel 등 팀 메타데이터
}

/// <summary>
/// 선수 데이터 구조
/// </summary>
[System.Serializable]
public struct PlayerData
{
    public string PlayerId;
    public string Name;
    public PlayerPosition Position;

    // 기본 능력치
    public float Contact;
    public float Power;
    public float Control;
    public float Velocity;

    // TODO: [Phase 5] TraitData[] Traits 특성 시스템
    // TODO: [Phase 6] ContractData Contract 계약 정보
}

/// <summary>
/// 포지션 열거형
/// </summary>
public enum PlayerPosition
{
    Pitcher,
    Catcher,
    FirstBase,
    SecondBase,
    ThirdBase,
    ShortStop,
    LeftField,
    CenterField,
    RightField,
    DesignatedHitter
}

/// <summary>
/// 게임 저장 데이터
/// </summary>
[System.Serializable]
public struct GameSaveData
{
    public string SaveId;
    public System.DateTime SaveTime;
    public GameStateData CurrentState;
    public TeamData[] Teams;

    // TODO: [Phase 6] SeasonData, ManagerData 추가
}