/// <summary>
/// Player: 선수 기본 클래스
/// </summary>
public abstract class Player
{
    public string PlayerId { get; protected set; }
    public string Name { get; protected set; }
    public PlayerPosition Position { get; protected set; }

    protected Player(string playerId, string name, PlayerPosition position)
    {
        PlayerId = playerId;
        Name = name;
        Position = position;
    }

    /// <summary>
    /// PlayerData에서 Player 생성 (Factory Pattern 준비)
    /// </summary>
    public static Player CreateFromData(PlayerData data)
    {
        // TODO: [Phase 2] Factory Pattern으로 포지션별 Player 생성
        // TODO: [Phase 5] 특성 시스템 적용
        switch (data.Position)
        {
            case PlayerPosition.Pitcher:
                return new Pitcher(data.PlayerId, data.Name, data.Control, data.Velocity, 0.5f);
            default:
                return new Batter(data.PlayerId, data.Name, data.Contact, data.Power, 0.5f);
        }
    }

    // TODO: [Phase 5] ApplyTrait(TraitData trait) 특성 적용
    // TODO: [Phase 6] UpdateCondition(float fatigue, float morale) 컨디션 관리
}

/// <summary>
/// Batter: 타자 클래스
/// </summary>
public class Batter : Player
{
    public float Contact { get; private set; }
    public float Power { get; private set; }
    public float SwingAggressiveness { get; private set; }

    public Batter(string playerId, string name, float contact, float power, float swingAggressiveness)
        : base(playerId, name, PlayerPosition.DesignatedHitter) // TODO: 실제 포지션 적용
    {
        Contact = contact;
        Power = power;
        SwingAggressiveness = swingAggressiveness;
    }

    // TODO: [Phase 4] CalculateSwingTiming(PitchData pitch) 스윙 타이밍 계산
    // TODO: [Phase 5] GetClutchRating(GameSituation situation) 클러치 상황 보정
}

/// <summary>
/// Pitcher: 투수 클래스
/// </summary>
public class Pitcher : Player
{
    public float Control { get; private set; }
    public float Velocity { get; private set; }
    public float BreakingBall { get; private set; }

    public Pitcher(string playerId, string name, float control, float velocity, float breakingBall)
        : base(playerId, name, PlayerPosition.Pitcher)
    {
        Control = control;
        Velocity = velocity;
        BreakingBall = breakingBall;
    }

    // TODO: [Phase 4] SelectPitch(BatterData batter, GameSituation situation) 구종 선택
    // TODO: [Phase 5] CalculateFatigue(int pitchCount) 피로도 계산
}