using Halyard.Abstractions;

namespace Halyard
{
    /// <inheritdoc cref="Host"/>
    public class StandardHost : Host
    {
        /// <inheritdoc />
        public StandardHost(bool logging = false)
            : base(logging) { }
    }
}
