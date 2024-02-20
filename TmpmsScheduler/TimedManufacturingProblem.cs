using Tmpms;
using Tmpms.Move;
using TmpmsChecker.Algorithm;

namespace TmpmsChecker;

public record TimedManufacturingProblem(String ProblemName, Configuration StartConfiguration, Location GoalLocation, IEnumerable<MoveAction> Actions);