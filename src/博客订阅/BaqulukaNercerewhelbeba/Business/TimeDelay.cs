using System;
using System.Threading.Tasks;

namespace BaqulukaNercerewhelbeba.Business
{
    public class TimeDelay : ITimeDelay
    {
        public async Task Delay()
        {
            await Task.Delay(TimeSpan.FromMinutes(10));
        }
    }
}