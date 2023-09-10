namespace FQ.Libraries.Randomness
{
    /// <summary>
    /// Random number generator.
    /// </summary>
    public class Random : IRandom
    {
        /// <summary>
        ///   <para>Return a random int within [minInclusive..maxExclusive) (Read Only).</para>
        /// </summary>
        /// <param name="minInclusive">Lowest number, may be selected.</param>
        /// <param name="maxExclusive">High number, will never be selected.</param>
        public int Range(int minInclusive, int maxExclusive)
        {
            return UnityEngine.Random.Range(minInclusive, maxExclusive);
        }
    }
}