using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UIElements;

/// <summary>
/// GameManager: 메인 게임 루프 관리 (Singleton)
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Game Systems")]
    [SerializeField] private bool autoStartGame = true;

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    // 핵심 시스템들
    private List<IGameSystem> gameSystems = new List<IGameSystem>();
    private IGameState gameState;
    private IEventDispatcher eventDispatcher;
    private IDataProvider dataProvider;

    // 현재 게임 데이터
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
    /// 모든 시스템 초기화
    /// </summary>
    private async Task InitializeSystems()
    {
        Logger.Log("게임 시스템 초기화 중...");

        // 핵심 시스템 생성
        eventDispatcher = new EventDispatcher();
        gameState = new GameStateManager();
        dataProvider = new MockDataProvider();

        // 데이터 로드
        await dataProvider.Initialize();

        // 게임 시스템 등록
        RegisterGameSystem(gameState as IGameSystem);
        // TODO: [Phase 2] RegisterGameSystem(new BattingSystem());
        // TODO: [Phase 2] RegisterGameSystem(new PitchingSystem());

        // 모든 시스템 초기화
        foreach (var system in gameSystems)
        {
            system.Initialize();
        }

        Logger.Log("게임 시스템 초기화 완료");
    }

    /// <summary>
    /// 게임 시스템 등록
    /// </summary>
    public void RegisterGameSystem(IGameSystem system)
    {
        if (system != null && !gameSystems.Contains(system))
        {
            gameSystems.Add(system);
        }
    }

    /// <summary>
    /// 새 게임 시작
    /// </summary>
    public async Task StartNewGame()
    {
        Logger.Log("===== 새 게임 시작 =====");

        // 팀 데이터 로드 및 생성
        await LoadTeams();

        // 게임 시작 이벤트 발생
        var gameStartEvent = new GameStartEvent(homeTeam.TeamName, awayTeam.TeamName);
        eventDispatcher.Dispatch(gameStartEvent);

        // 게임 루프 시작
        await RunGameLoop();
    }

    /// <summary>
    /// 팀 로드
    /// </summary>
    private async Task LoadTeams()
    {
        var teamDataList = await dataProvider.LoadTeamData();
        var playerDataList = await dataProvider.LoadPlayerData();

        if (teamDataList.Count >= 2)
        {
            homeTeam = Team.CreateFromData(teamDataList[0], playerDataList);
            awayTeam = Team.CreateFromData(teamDataList[1], playerDataList);

            Logger.Log($"팀 로드 완료: {homeTeam.TeamName} vs {awayTeam.TeamName}");
        }
        else
        {
            Logger.LogError("팀 데이터가 부족합니다.");
        }
    }

    /// <summary>
    /// 메인 게임 루프
    /// </summary>
    private async Task RunGameLoop()
    {
        while (!gameState.IsGameOver())
        {
            // 현재 상황 로그
            LogCurrentSituation();

            // 모든 게임 시스템 처리
            foreach (var system in gameSystems)
            {
                system.ProcessTurn();
            }

            // TODO: [Phase 2] 실제 타석 처리 로직
            // 임시: 단순 진행
            await SimulateAtBat();

            // 짧은 대기 (시각적 효과용)
            await Task.Delay(100);
        }

        // 게임 종료 처리
        EndGame();
    }

    /// <summary>
    /// 현재 상황 로그 출력
    /// </summary>
    private void LogCurrentSituation()
    {
        string half = gameState.IsTopHalf ? "초" : "말";
        Logger.Log($"{gameState.CurrentInning}회 {half} - {gameState.OutCount}아웃 " +
                  $"[{gameState.HomeScore}:{gameState.AwayScore}]");
    }

    /// <summary>
    /// 임시 타석 시뮬레이션 (Phase 2에서 교체 예정)
    /// </summary>
    private async Task SimulateAtBat()
    {
        Team battingTeam = gameState.IsTopHalf ? awayTeam : homeTeam;
        Team pitchingTeam = gameState.IsTopHalf ? homeTeam : awayTeam;

        var batter = battingTeam.GetNextBatter();
        var pitcher = pitchingTeam.GetPitcher();

        if (batter == null || pitcher == null) return;

        // 간단한 확률 계산
        float hitChance = 0.3f + (batter.Contact - pitcher.Control) * 0.2f;
        hitChance = Mathf.Clamp(hitChance, 0.1f, 0.9f);

        bool isHit = Random.value < hitChance;
        PlayResult result = isHit ? PlayResult.Single : PlayResult.Groundout;

        // 결과 처리
        ProcessAtBatResult(batter.Name, pitcher.Name, result);

        await Task.Delay(50); // 시각적 딜레이
    }

    /// <summary>
    /// 타석 결과 처리
    /// </summary>
    private void ProcessAtBatResult(string batterName, string pitcherName, PlayResult result)
    {
        // 이벤트 발생
        var atBatEvent = new AtBatResultEvent(batterName, pitcherName, result);
        eventDispatcher.Dispatch(atBatEvent);

        // 결과에 따른 처리
        switch (result)
        {
            case PlayResult.Single:
            case PlayResult.Double:
            case PlayResult.Triple:
            case PlayResult.HomeRun:
                Logger.LogPlay(batterName, pitcherName, result);
                // TODO: [Phase 2] 주자 진루 및 득점 처리
                if (result == PlayResult.HomeRun)
                {
                    gameState.AddScore(!gameState.IsTopHalf, 1);
                }
                break;

            default: // 아웃
                Logger.LogPlay(batterName, pitcherName, result);
                gameState.NextOut();
                break;
        }
    }

    /// <summary>
    /// 게임 종료 처리
    /// </summary>
    private void EndGame()
    {
        string winner = gameState.GetWinner();
        var gameEndEvent = new GameEndEvent(winner, gameState.HomeScore, gameState.AwayScore);
        eventDispatcher.Dispatch(gameEndEvent);

        Logger.Log($"===== 게임 종료 =====");
        Logger.Log($"최종 스코어: {homeTeam.TeamName} {gameState.HomeScore} - {gameState.AwayScore} {awayTeam.TeamName}");
        Logger.Log($"승리팀: {winner}");
    }

    /// <summary>
    /// 게임 시스템 정리
    /// </summary>
    private void OnDestroy()
    {
        foreach (var system in gameSystems)
        {
            system.Cleanup();
        }
        eventDispatcher?.Clear();
    }

    // TODO: [Phase 3] PauseGame(), ResumeGame() 일시정지 기능
    // TODO: [Phase 3] SetGameSpeed(float speed) 게임 속도 조절
    // TODO: [Phase 6] SaveGame(), LoadGame() 세이브/로드
}