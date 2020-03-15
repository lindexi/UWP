using System.Threading.Tasks;

namespace BaqulukaNercerewhelbeba.Business.Tests
{
    public class FakeTimeDelay : ITimeDelay
    {
        /// <inheritdoc />
        public Task Delay()
        {
            return Task.CompletedTask;
        }
    }
}