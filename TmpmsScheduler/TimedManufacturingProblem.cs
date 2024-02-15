using Tmpms;
using Tmpms.Move;
using TMPMSChecker.Algorithm;

namespace TMPMSChecker;

public record TimedManufacturingProblem(Configuration StartConfiguration, Location GoalLocation, IEnumerable<MoveAction> Actions);