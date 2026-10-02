namespace WheelSpin
{
    // The zone rhythm. Every rule that depends on a level's zone type goes through here.
    public static class Zones
    {
        public const int FirstLevel = 1;
        public const int SafeInterval = 5;    // every 5th zone: silver wheel, no bomb
        public const int SuperInterval = 30;  // every 30th zone: gold wheel, no bomb

        public static ZoneType TypeOf(int level)
        {
            if (level % SuperInterval == 0) return ZoneType.Super;
            if (level % SafeInterval == 0) return ZoneType.Safe;
            return ZoneType.Normal;
        }

        // Cash-out is only offered where the wheel has no bomb
        public static bool AllowsExit(int level) => TypeOf(level) != ZoneType.Normal;

        // Normal zones carry exactly one bomb; safe and super zones carry none
        public static int BombsPerWheel(ZoneType zone) => zone == ZoneType.Normal ? 1 : 0;
    }
}
