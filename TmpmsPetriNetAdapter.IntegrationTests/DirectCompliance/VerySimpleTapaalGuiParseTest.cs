using JsonFixtures.Tofms.Fixtures;
using Tmpms;
using Xunit;

namespace TACPN.IntegrationTests.DirectCompliance;

public class VerySimpleTapaalGuiParseTest : GuiTranslationAdherenceTest, IClassFixture<VerySimpleReader>
{
    public VerySimpleTapaalGuiParseTest(VerySimpleReader reader)
    {
        System = reader.System;
    }


    protected override string TestName { get; } = nameof(VerySimpleTapaalGuiParseTest);
    protected override TimedMultipartSystem System { get; }
}