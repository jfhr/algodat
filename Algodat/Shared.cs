using System;

namespace Algodat
{
    /// <summary>
    /// Shared services for all implementations.
    /// </summary>
    internal static class Shared
    {
        public static Random Random { get; } = new();
    }
}