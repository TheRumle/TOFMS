using Tmpms.Move;

namespace Tmpms.Translate;

public interface IMoveActionTranslation<Target>
{
    public Target Translate(MoveAction moveAction);

    public Task<Target> TranslateAsync(MoveAction moveAction);
}