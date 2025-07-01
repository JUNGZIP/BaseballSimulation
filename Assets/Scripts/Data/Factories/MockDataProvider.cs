using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// MockDataProvider: �ӽ� ������ ������ (Phase 1��)
/// </summary>
public class MockDataProvider : IDataProvider
{
    private List<TeamData> mockTeams;
    private List<PlayerData> mockPlayers;

    public async Task<bool> Initialize()
    {
        Logger.Log("MockDataProvider �ʱ�ȭ ��...");

        CreateMockPlayers();
        CreateMockTeams();

        // �񵿱� �ùķ��̼�
        await Task.Delay(10);

        Logger.Log("Mock ������ �ε� �Ϸ�");
        return true;
    }

    public async Task<List<TeamData>> LoadTeamData()
    {
        await Task.Delay(1); // �񵿱� �ùķ��̼�
        return new List<TeamData>(mockTeams);
    }

    public async Task<List<PlayerData>> LoadPlayerData()
    {
        await Task.Delay(1); // �񵿱� �ùķ��̼�
        return new List<PlayerData>(mockPlayers);
    }

    public async Task<PlayerData> GetPlayerData(string playerId)
    {
        await Task.Delay(1);

        foreach (var player in mockPlayers)
        {
            if (player.PlayerId == playerId)
            {
                return player;
            }
        }

        Logger.LogWarning($"������ ã�� �� �����ϴ�: {playerId}");
        return default;
    }

    public async Task<bool> SaveGameData(GameSaveData data)
    {
        await Task.Delay(10);
        Logger.Log($"���� ���� �Ϸ�: {data.SaveId}");
        return true;

        // TODO: [Phase 6] ���� ���� ���� ����
    }

    /// <summary>
    /// Mock ���� ������ ����
    /// </summary>
    private void CreateMockPlayers()
    {
        mockPlayers = new List<PlayerData>
        {
            // Ȩ�� ������
            new PlayerData
            {
                PlayerId = "H_P01",
                Name = "������",
                Position = PlayerPosition.Pitcher,
                Contact = 0.3f,
                Power = 0.2f,
                Control = 0.8f,
                Velocity = 0.7f
            },
            new PlayerData
            {
                PlayerId = "H_B01",
                Name = "��Ÿ��",
                Position = PlayerPosition.FirstBase,
                Contact = 0.7f,
                Power = 0.6f,
                Control = 0.2f,
                Velocity = 0.3f
            },
            new PlayerData
            {
                PlayerId = "H_B02",
                Name = "�̰�Ÿ",
                Position = PlayerPosition.CenterField,
                Contact = 0.6f,
                Power = 0.8f,
                Control = 0.1f,
                Velocity = 0.2f
            },
            new PlayerData
            {
                PlayerId = "H_B03",
                Name = "�־�Ÿ",
                Position = PlayerPosition.SecondBase,
                Contact = 0.8f,
                Power = 0.4f,
                Control = 0.1f,
                Velocity = 0.2f
            },

            // ������ ������
            new PlayerData
            {
                PlayerId = "A_P01",
                Name = "�����̽�",
                Position = PlayerPosition.Pitcher,
                Contact = 0.2f,
                Power = 0.1f,
                Control = 0.9f,
                Velocity = 0.8f
            },
            new PlayerData
            {
                PlayerId = "A_B01",
                Name = "ȫŬ����",
                Position = PlayerPosition.ThirdBase,
                Contact = 0.6f,
                Power = 0.9f,
                Control = 0.1f,
                Velocity = 0.2f
            },
            new PlayerData
            {
                PlayerId = "A_B02",
                Name = "�����ǵ�",
                Position = PlayerPosition.LeftField,
                Contact = 0.7f,
                Power = 0.3f,
                Control = 0.1f,
                Velocity = 0.3f
            },
            new PlayerData
            {
                PlayerId = "A_B03",
                Name = "�����",
                Position = PlayerPosition.ShortStop,
                Contact = 0.5f,
                Power = 0.4f,
                Control = 0.1f,
                Velocity = 0.2f
            }
        };

        Logger.LogDebug($"Mock ���� ������ ����: {mockPlayers.Count}��");
    }

    /// <summary>
    /// Mock �� ������ ����
    /// </summary>
    private void CreateMockTeams()
    {
        mockTeams = new List<TeamData>
        {
            new TeamData
            {
                TeamId = "HOME",
                TeamName = "Ȩ Ÿ�̰���",
                PlayerIds = new string[] { "H_B01", "H_B02", "H_B03" },
                PitcherId = "H_P01"
            },
            new TeamData
            {
                TeamId = "AWAY",
                TeamName = "����� ���̿���",
                PlayerIds = new string[] { "A_B01", "A_B02", "A_B03" },
                PitcherId = "A_P01"
            }
        };

        Logger.LogDebug($"Mock �� ������ ����: {mockTeams.Count}��");
    }

    // TODO: [Phase 5] LoadFromCSV(string filePath) CSV ���� �ε�
    // TODO: [Phase 5] LoadFromJSON(string filePath) JSON ���� �ε�
    // TODO: [Phase 6] LoadFromDatabase(string connectionString) DB ����
    // TODO: [Phase 6] ValidateData() ������ ��ȿ�� ����
}