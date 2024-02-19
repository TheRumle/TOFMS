using Tmpms;
using Tmpms.Move;
using TmpmsChecker.Domain;
using MoveAction = TmpmsChecker.Domain.MoveAction;

namespace TmpmsChecker;

public record TimedManufacturingProblem(Configuration StartConfiguration, Location GoalLocation, IEnumerable<MoveAction> Actions);