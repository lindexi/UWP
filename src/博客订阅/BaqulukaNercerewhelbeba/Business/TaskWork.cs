using System.Threading.Tasks;

namespace BaqulukaNercerewhelbeba.Business
{
    public class TaskWork
    {
        private readonly ITimeDelay _timeDelay;

        public TaskWork(ITimeDelay timeDelay)
        {
            _timeDelay = timeDelay;
        }

        public void StartWork(IJob job)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await job.Start();
                    await _timeDelay.Delay();
                }
            });
        }
    }
}