using Common.Move;

namespace Common.Translate;

public interface IMoveActionTranslation<Target>
{
    public Target Translate(MoveAction moveAction);

    public IEnumerable<Target> Translate(IEnumerable<MoveAction> moveActions)
    {
        return moveActions.Select(Translate);
    }

    public Task<Target> TranslateAsync(MoveAction moveAction);

    public async Task<IEnumerable<Target>> TranslateAsync(IEnumerable<MoveAction> moveActions)
    {
        var translationTasks = moveActions.Select(TranslateAsync);
        return await Task.WhenAll(translationTasks);
    }
}