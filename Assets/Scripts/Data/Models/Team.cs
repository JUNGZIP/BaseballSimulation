using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Team: �� ���� Ŭ����
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
    /// TeamData���� Team ����
    /// </summary>
    public static Team CreateFromData(TeamData teamData, List<PlayerData> playersData)
    {
        var team = new Team(teamData.TeamId, teamData.TeamName);

        // ���ξ� ������ �߰�
        foreach (var playerId in teamData.PlayerIds)
        {
            var playerData = playersData.FirstOrDefault(p => p.PlayerId == playerId);
            if (playerData.PlayerId != null && playerData.Position != PlayerPosition.Pitcher)
            {
                var batter = Player.CreateFromData(playerData) as Batter;
                if (batter != null) team.AddBatter(batter);
            }
        }

        // ���� ����
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

    // TODO: [Phase 3] SubstituteBatter(int position, Batter newBatter) ���� ��ü
    // TODO: [Phase 3] ChangePitcher(Pitcher newPitcher) ���� ��ü
    // TODO: [Phase 6] GetRoster() ��ü �ν��� ����
    // TODO: [Phase 6] CalculateTeamChemistry() �� �ɹ̽�Ʈ��
}