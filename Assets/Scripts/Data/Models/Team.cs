using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Team: 팀 관리 클래스
/// </summary>
public class Team
{
    public string TeamId { get; private set; }
    public string TeamName { get; private set; }

    private List<Batter> lineup = new List<Batter>();
    private Pitcher currentPitcher;
    private int currentBatterIndex = 0;

    public Team(string teamId, string teamName)
    {
        TeamId = teamId;
        TeamName = teamName;
    }

    /// <summary>
    /// TeamData에서 Team 생성
    /// </summary>
    public static Team CreateFromData(TeamData teamData, List<PlayerData> playersData)
    {
        var team = new Team(teamData.TeamId, teamData.TeamName);

        // 라인업 선수들 추가
        foreach (var playerId in teamData.PlayerIds)
        {
            var playerData = playersData.FirstOrDefault(p => p.PlayerId == playerId);
            if (playerData.PlayerId != null && playerData.Position != PlayerPosition.Pitcher)
            {
                var batter = Player.CreateFromData(playerData) as Batter;
                if (batter != null) team.AddBatter(batter);
            }
        }

        // 투수 설정
        var pitcherData = playersData.FirstOrDefault(p => p.PlayerId == teamData.PitcherId);
        if (pitcherData.PlayerId != null)
        {
            var pitcher = Player.CreateFromData(pitcherData) as Pitcher;
            if (pitcher != null) team.SetPitcher(pitcher);
        }

        return team;
    }

    public void AddBatter(Batter batter)
    {
        lineup.Add(batter);
    }

    public void SetPitcher(Pitcher pitcher)
    {
        currentPitcher = pitcher;
    }

    public Pitcher GetPitcher()
    {
        return currentPitcher;
    }

    public Batter GetNextBatter()
    {
        if (lineup.Count == 0) return null;

        Batter batter = lineup[currentBatterIndex];
        currentBatterIndex = (currentBatterIndex + 1) % lineup.Count;
        return batter;
    }

    public void ResetBattingOrder()
    {
        currentBatterIndex = 0;
    }

    // TODO: [Phase 3] SubstituteBatter(int position, Batter newBatter) 선수 교체
    // TODO: [Phase 3] ChangePitcher(Pitcher newPitcher) 투수 교체
    // TODO: [Phase 6] GetRoster() 전체 로스터 관리
    // TODO: [Phase 6] CalculateTeamChemistry() 팀 케미스트리
}