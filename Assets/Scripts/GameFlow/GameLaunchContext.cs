public static class GameLaunchContext
{
    public static bool ShouldPlayStartIntro { get; private set; }

    public static void RequestStartIntro()
    {
        ShouldPlayStartIntro = true;
    }

    public static void SkipStartIntro()
    {
        ShouldPlayStartIntro = false;
    }

    public static bool ConsumeStartIntroFlag()
    {
        bool shouldPlayIntro = ShouldPlayStartIntro;
        ShouldPlayStartIntro = false;

        return shouldPlayIntro;
    }
}