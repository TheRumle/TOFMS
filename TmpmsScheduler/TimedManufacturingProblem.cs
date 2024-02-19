using Tmpms;
using Tmpms.Move;

namespace TmpmsChecker;

public record TimedManufacturingProblem(Configuration StartConfiguration, Location GoalLocation, IEnumerable<MoveAction> Actions);