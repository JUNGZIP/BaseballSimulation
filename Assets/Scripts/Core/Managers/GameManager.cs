using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UIElements;

/// <summary>
/// GameManager: ���� ���� ���� ���� (Singleton)
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Game Systems")]
    [SerializeField] private bool autoStartGame = true;

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    // �ٽ� �ý��۵�
    private List<IGameSystem> gameSystems = new List<IGameSystem>();
    private IGameState gameState;
    private IEventDispatcher eventDispatcher;
    private IDataProvider dataProvider;

    // ���� ���� ������
    private Team homeTeam;
    private Team awayTeam;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private async void Start()
    {
        await InitializeSystems();

        if (autoStartGame)
        {
            await StartNewGame();
        }
    }

    /// <summary>
    /// ��� �ý��� �ʱ�ȭ
    /// </summary>
    private async Task InitializeSystems()
    {
        Logger.Log("���� �ý��� �ʱ�ȭ ��...");

        // �ٽ� �ý��� ����
        eventDispatcher = new EventDispatcher();
        gameState = new GameStateManager();
        dataProvider = new MockDataProvider();

        // ������ �ε�
        await dataProvider.Initialize();

        // ���� �ý��� ���
        RegisterGameSystem(gameState as IGameSystem);
        // TODO: [Phase 2] RegisterGameSystem(new BattingSystem());
        // TODO: [Phase 2] RegisterGameSystem(new PitchingSystem());

        // ��� �ý��� �ʱ�ȭ
        foreach (var system in gameSystems)
        {
            system.Initialize();
        }

        Logger.Log("���� �ý��� �ʱ�ȭ �Ϸ�");
    }

    /// <summary>
    /// ���� �ý��� ���
    /// </summary>
    public void RegisterGameSystem(IGameSystem system)
    {
        if (system != null && !gameSystems.Contains(system))
        {
            gameSystems.Add(system);
        }
    }

    /// <summary>
    /// �� ���� ����
    /// </summary>
    public async Task StartNewGame()
    {
        Logger.Log("===== �� ���� ���� =====");

        // �� ������ �ε� �� ����
        await LoadTeams();

        // ���� ���� �̺�Ʈ �߻�
        var gameStartEvent = new GameStartEvent(homeTeam.TeamName, awayTeam.TeamName);
        eventDispatcher.Dispatch(gameStartEvent);

        // ���� ���� ����
        await RunGameLoop();
    }

    /// <summary>
    /// �� �ε�
    /// </summary>
    private async Task LoadTeams()
    {
        var teamDataList = await dataProvider.LoadTeamData();
        var playerDataList = await dataProvider.LoadPlayerData();

        if (teamDataList.Count >= 2)
        {
            homeTeam = Team.CreateFromData(teamDataList[0], playerDataList);
            awayTeam = Team.CreateFromData(teamDataList[1], playerDataList);

            Logger.Log($"�� �ε� �Ϸ�: {homeTeam.TeamName} vs {awayTeam.TeamName}");
        }
        else
        {
            Logger.LogError("�� �����Ͱ� �����մϴ�.");
        }
    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    private async Task RunGameLoop()
    {
        while (!gameState.IsGameOver())
        {
            // ���� ��Ȳ �α�
            LogCurrentSituation();

            // ��� ���� �ý��� ó��
            foreach (var system in gameSystems)
            {
                system.ProcessTurn();
            }

            // TODO: [Phase 2] ���� Ÿ�� ó�� ����
            // �ӽ�: �ܼ� ����
            await SimulateAtBat();

            // ª�� ��� (�ð��� ȿ����)
            await Task.Delay(100);
        }

        // ���� ���� ó��
        EndGame();
    }

    /// <summary>
    /// ���� ��Ȳ �α� ���
    /// </summary>
    private void LogCurrentSituation()
    {
        string half = gameState.IsTopHalf ? "��" : "��";
        Logger.Log($"{gameState.CurrentInning}ȸ {half} - {gameState.OutCount}�ƿ� " +
                  $"[{gameState.HomeScore}:{gameState.AwayScore}]");
    }

    /// <summary>
    /// �ӽ� Ÿ�� �ùķ��̼� (Phase 2���� ��ü ����)
    /// </summary>
    private async Task SimulateAtBat()
    {
        Team battingTeam = gameState.IsTopHalf ? awayTeam : homeTeam;
        Team pitchingTeam = gameState.IsTopHalf ? homeTeam : awayTeam;

        var batter = battingTeam.GetNextBatter();
        var pitcher = pitchingTeam.GetPitcher();

        if (batter == null || pitcher == null) return;

        // ������ Ȯ�� ���
        float hitChance = 0.3f + (batter.Contact - pitcher.Control) * 0.2f;
        hitChance = Mathf.Clamp(hitChance, 0.1f, 0.9f);

        bool isHit = Random.value < hitChance;
        PlayResult result = isHit ? PlayResult.Single : PlayResult.Groundout;

        // ��� ó��
        ProcessAtBatResult(batter.Name, pitcher.Name, result);

        await Task.Delay(50); // �ð��� ������
    }

    /// <summary>
    /// Ÿ�� ��� ó��
    /// </summary>
    private void ProcessAtBatResult(string batterName, string pitcherName, PlayResult result)
    {
        // �̺�Ʈ �߻�
        var atBatEvent = new AtBatResultEvent(batterName, pitcherName, result);
        eventDispatcher.Dispatch(atBatEvent);

        // ����� ���� ó��
        switch (result)
        {
            case PlayResult.Single:
            case PlayResult.Double:
            case PlayResult.Triple:
            case PlayResult.HomeRun:
                Logger.LogPlay(batterName, pitcherName, result);
                // TODO: [Phase 2] ���� ���� �� ���� ó��
                if (result == PlayResult.HomeRun)
                {
                    gameState.AddScore(!gameState.IsTopHalf, 1);
                }
                break;

            default: // �ƿ�
                Logger.LogPlay(batterName, pitcherName, result);
                gameState.NextOut();
                break;
        }
    }

    /// <summary>
    /// ���� ���� ó��
    /// </summary>
    private void EndGame()
    {
        string winner = gameState.GetWinner();
        var gameEndEvent = new GameEndEvent(winner, gameState.HomeScore, gameState.AwayScore);
        eventDispatcher.Dispatch(gameEndEvent);

        Logger.Log($"===== ���� ���� =====");
        Logger.Log($"���� ���ھ�: {homeTeam.TeamName} {gameState.HomeScore} - {gameState.AwayScore} {awayTeam.TeamName}");
        Logger.Log($"�¸���: {winner}");
    }

    /// <summary>
    /// ���� �ý��� ����
    /// </summary>
    private void OnDestroy()
    {
        foreach (var system in gameSystems)
        {
            system.Cleanup();
        }
        eventDispatcher?.Clear();
    }

    // TODO: [Phase 3] PauseGame(), ResumeGame() �Ͻ����� ���
    // TODO: [Phase 3] SetGameSpeed(float speed) ���� �ӵ� ����
    // TODO: [Phase 6] SaveGame(), LoadGame() ���̺�/�ε�
}