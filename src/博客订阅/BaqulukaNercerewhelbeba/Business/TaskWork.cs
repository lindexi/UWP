using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BaqulukaNercerewhelbeba.Business
{
    public class TaskWork
    {
        private readonly ITimeDelay _timeDelay;
        private readonly ILogger<TaskWork> _logger;

        public TaskWork(ITimeDelay timeDelay,ILogger<TaskWork> logger)
        {
            _timeDelay = timeDelay;
            _logger = logger;
        }

        public void StartWork(IJob job)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await job.Start();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "StartWork");
                    }

                    await _timeDelay.Delay();
                }
            });
        }
    }
}