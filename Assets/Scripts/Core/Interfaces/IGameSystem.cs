/// <summary>
/// IGameSystem: ��� ���� �ý����� �⺻ �������̽�
/// </summary>
public interface IGameSystem
{
    /// <summary>
    /// �ý��� �ʱ�ȭ
    /// </summary>
    void Initialize();

    /// <summary>
    /// �� ó�� (����� �ܼ�, ���� �Ű����� �߰�)
    /// </summary>
    void ProcessTurn();

    /// <summary>
    /// �ý��� ����
    /// </summary>
    void Cleanup();

    // TODO: [Phase 4] ProcessTurn(GameState state, AIContext context) ��ȭ
    // TODO: [Phase 3] GetSystemPriority() �켱���� ��ȯ
}