using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// MockDataProvider: 임시 데이터 제공자 (Phase 1용)
/// </summary>
public class MockDataProvider : IDataProvider
{
    private List<TeamData> mockTeams;
    private List<PlayerData> mockPlayers;

    public async Task<bool> Initialize()
    {
        Logger.Log("MockDataProvider 초기화 중...");

        CreateMockPlayers();
        CreateMockTeams();

        // 비동기 시뮬레이션
        await Task.Delay(10);

        Logger.Log("Mock 데이터 로드 완료");
        return true;
    }

    public async Task<List<TeamData>> LoadTeamData()
    {
        await Task.Delay(1); // 비동기 시뮬레이션
        return new List<TeamData>(mockTeams);
    }

    public async Task<List<PlayerData>> LoadPlayerData()
    {
        await Task.Delay(1); // 비동기 시뮬레이션
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

        Logger.LogWarning($"선수를 찾을 수 없습니다: {playerId}");
        return default;
    }

    public async Task<bool> SaveGameData(GameSaveData data)
    {
        await Task.Delay(10);
        Logger.Log($"게임 저장 완료: {data.SaveId}");
        return true;

        // TODO: [Phase 6] 실제 파일 저장 구현
    }

    /// <summary>
    /// Mock 선수 데이터 생성
    /// </summary>
    private void CreateMockPlayers()
    {
        mockPlayers = new List<PlayerData>
        {
            // 홈팀 선수들
            new PlayerData
            {
                PlayerId = "H_P01",
                Name = "김투수",
                Position = PlayerPosition.Pitcher,
                Contact = 0.3f,
                Power = 0.2f,
                Control = 0.8f,
                Velocity = 0.7f
            },
            new PlayerData
            {
                PlayerId = "H_B01",
                Name = "박타자",
                Position = PlayerPosition.FirstBase,
                Contact = 0.7f,
                Power = 0.6f,
                Control = 0.2f,
                Velocity = 0.3f
            },
            new PlayerData
            {
                PlayerId = "H_B02",
                Name = "이강타",
                Position = PlayerPosition.CenterField,
                Contact = 0.6f,
                Power = 0.8f,
                Control = 0.1f,
                Velocity = 0.2f
            },
            new PlayerData
            {
                PlayerId = "H_B03",
                Name = "최안타",
                Position = PlayerPosition.SecondBase,
                Contact = 0.8f,
                Power = 0.4f,
                Control = 0.1f,
                Velocity = 0.2f
            },

            // 원정팀 선수들
            new PlayerData
            {
                PlayerId = "A_P01",
                Name = "정에이스",
                Position = PlayerPosition.Pitcher,
                Contact = 0.2f,
                Power = 0.1f,
                Control = 0.9f,
                Velocity = 0.8f
            },
            new PlayerData
            {
                PlayerId = "A_B01",
                Name = "홍클린업",
                Position = PlayerPosition.ThirdBase,
                Contact = 0.6f,
                Power = 0.9f,
                Control = 0.1f,
                Velocity = 0.2f
            },
            new PlayerData
            {
                PlayerId = "A_B02",
                Name = "윤스피드",
                Position = PlayerPosition.LeftField,
                Contact = 0.7f,
                Power = 0.3f,
                Control = 0.1f,
                Velocity = 0.3f
            },
            new PlayerData
            {
                PlayerId = "A_B03",
                Name = "장수비",
                Position = PlayerPosition.ShortStop,
                Contact = 0.5f,
                Power = 0.4f,
                Control = 0.1f,
                Velocity = 0.2f
            }
        };

        Logger.LogDebug($"Mock 선수 데이터 생성: {mockPlayers.Count}명");
    }

    /// <summary>
    /// Mock 팀 데이터 생성
    /// </summary>
    private void CreateMockTeams()
    {
        mockTeams = new List<TeamData>
        {
            new TeamData
            {
                TeamId = "HOME",
                TeamName = "홈 타이거즈",
                PlayerIds = new string[] { "H_B01", "H_B02", "H_B03" },
                PitcherId = "H_P01"
            },
            new TeamData
            {
                TeamId = "AWAY",
                TeamName = "어웨이 라이온즈",
                PlayerIds = new string[] { "A_B01", "A_B02", "A_B03" },
                PitcherId = "A_P01"
            }
        };

        Logger.LogDebug($"Mock 팀 데이터 생성: {mockTeams.Count}팀");
    }

    // TODO: [Phase 5] LoadFromCSV(string filePath) CSV 파일 로드
    // TODO: [Phase 5] LoadFromJSON(string filePath) JSON 파일 로드
    // TODO: [Phase 6] LoadFromDatabase(string connectionString) DB 연동
    // TODO: [Phase 6] ValidateData() 데이터 유효성 검증
}