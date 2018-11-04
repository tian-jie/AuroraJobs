using Aurora.Jobs.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Aurora.Jobs.Tests
{
    [TestClass()]
    public class ServiceRunnerTests
    {
        [TestMethod()]
        public void StartTest()
        {
            var scheduledTaskService = new ScheduledTaskService();
            var jobs = scheduledTaskService.GetAllowScheduleJobInfoList();

            var obj = new ServiceRunner();
            var task = Task.Run(()=>obj.Start());
            task.Wait();

            //var executedJobs = scheduledTaskService
           // Assert.IsTrue(result);


        }
    }
}