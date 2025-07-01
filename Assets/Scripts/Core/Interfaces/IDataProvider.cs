using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// IDataProvider: ������ ���� �������̽� (CSV/JSON/DB �߻�ȭ)
/// </summary>
public interface IDataProvider
{
    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    Task<bool> Initialize();

    /// <summary>
    /// �� ������ �ε�
    /// </summary>
    Task<List<TeamData>> LoadTeamData();

    /// <summary>
    /// ���� ������ �ε�
    /// </summary>
    Task<List<PlayerData>> LoadPlayerData();

    /// <summary>
    /// Ư�� ���� ������ ��ȸ
    /// </summary>
    Task<PlayerData> GetPlayerData(string playerId);

    /// <summary>
    /// ������ ���� (���̺�/�ε��)
    /// </summary>
    Task<bool> SaveGameData(GameSaveData data);

    // TODO: [Phase 5] LoadSeasonData() ���� ������
    // TODO: [Phase 5] LoadStatsData() ���� ��� ������
    // TODO: [Phase 6] LoadTradingData() Ʈ���̵� ������
}

/// <summary>
/// �� ������ ����
/// </summary>
[System.Serializable]
public struct TeamData
{
    public string TeamId;
    public string TeamName;
    public string[] PlayerIds;
    public string PitcherId;

    // TODO: [Phase 6] Budget, Reputation, FacilityLevel �� �� ��Ÿ������
}

/// <summary>
/// ���� ������ ����
/// </summary>
[System.Serializable]
public struct PlayerData
{
    public string PlayerId;
    public string Name;
    public PlayerPosition Position;

    // �⺻ �ɷ�ġ
    public float Contact;
    public float Power;
    public float Control;
    public float Velocity;

    // TODO: [Phase 5] TraitData[] Traits Ư�� �ý���
    // TODO: [Phase 6] ContractData Contract ��� ����
}

/// <summary>
/// ������ ������
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
/// ���� ���� ������
/// </summary>
[System.Serializable]
public struct GameSaveData
{
    public string SaveId;
    public System.DateTime SaveTime;
    public GameStateData CurrentState;
    public TeamData[] Teams;

    // TODO: [Phase 6] SeasonData, ManagerData �߰�
}