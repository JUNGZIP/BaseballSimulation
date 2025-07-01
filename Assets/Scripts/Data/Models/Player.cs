/// <summary>
/// Player: ���� �⺻ Ŭ����
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
    /// PlayerData���� Player ���� (Factory Pattern �غ�)
    /// </summary>
    public static Player CreateFromData(PlayerData data)
    {
        // TODO: [Phase 2] Factory Pattern���� �����Ǻ� Player ����
        // TODO: [Phase 5] Ư�� �ý��� ����
        switch (data.Position)
        {
            case PlayerPosition.Pitcher:
                return new Pitcher(data.PlayerId, data.Name, data.Control, data.Velocity, 0.5f);
            default:
                return new Batter(data.PlayerId, data.Name, data.Contact, data.Power, 0.5f);
        }
    }

    // TODO: [Phase 5] ApplyTrait(TraitData trait) Ư�� ����
    // TODO: [Phase 6] UpdateCondition(float fatigue, float morale) ����� ����
}

/// <summary>
/// Batter: Ÿ�� Ŭ����
/// </summary>
public class Batter : Player
{
    public float Contact { get; private set; }
    public float Power { get; private set; }
    public float SwingAggressiveness { get; private set; }

    public Batter(string playerId, string name, float contact, float power, float swingAggressiveness)
        : base(playerId, name, PlayerPosition.DesignatedHitter) // TODO: ���� ������ ����
    {
        Contact = contact;
        Power = power;
        SwingAggressiveness = swingAggressiveness;
    }

    // TODO: [Phase 4] CalculateSwingTiming(PitchData pitch) ���� Ÿ�̹� ���
    // TODO: [Phase 5] GetClutchRating(GameSituation situation) Ŭ��ġ ��Ȳ ����
}

/// <summary>
/// Pitcher: ���� Ŭ����
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

    // TODO: [Phase 4] SelectPitch(BatterData batter, GameSituation situation) ���� ����
    // TODO: [Phase 5] CalculateFatigue(int pitchCount) �Ƿε� ���
}