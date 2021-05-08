using System;

namespace Algodat
{
    /// <summary>
    /// We put this in an extra class, to avoid generating one instance for every set
    /// of type parameters of the generic BinarySearchTree class.
    /// </summary>
    internal static class Static
    {
        public static Random random = new();
    }
}