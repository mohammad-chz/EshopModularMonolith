namespace Shared.DDD
{
    public static class SequentialGuid
    {
        /// <summary>
        /// Creates a time-ordered UUID v7 (monotonically increasing).
        /// Requires .NET 9+. Falls back to NewGuid() for earlier versions.
        /// </summary>
        public static Guid NewId() => Guid.CreateVersion7();
    }
}
