using Common.Logging;
using Aurora.Jobs.Core;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System.Threading.Tasks;
using Topshelf;
using System;

namespace Aurora.Jobs
{
    public class ServiceRunner : ServiceControl, ServiceSuspend
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ServiceRunner));
        private ISchedulerFactory schedulerFactory;
        private IScheduler scheduler;

        private string ServiceName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("ServiceName");
            }
        }

        public ServiceRunner()
        {
            scheduler = Task.Run(() => StdSchedulerFactory.GetDefaultScheduler()).Result;
        }

        public virtual async Task Initialize()
        {
            try
            {
                schedulerFactory = CreateSchedulerFactory();
                scheduler = await GetScheduler().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.Error("Server initialization failed:" + e.Message, e);
                throw;
            }
        }


        /// <summary>
        /// Gets the scheduler with which this server should operate with.
        /// </summary>
        /// <returns></returns>
        protected virtual Task<IScheduler> GetScheduler()
        {
            return schedulerFactory.GetScheduler();
        }

        /// <summary>
        /// Returns the current scheduler instance (usually created in <see cref="Initialize" />
        /// using the <see cref="GetScheduler" /> method).
        /// </summary>
	    protected virtual IScheduler Scheduler => scheduler;

        /// <summary>
        /// Creates the scheduler factory that will be the factory
        /// for all schedulers on this instance.
        /// </summary>
        /// <returns></returns>
        protected virtual ISchedulerFactory CreateSchedulerFactory()
        {
            return new StdSchedulerFactory();
        }


        /// <summary>
        /// Starts this instance, delegates to scheduler.
        /// </summary>
        public virtual async void Start()
        {
            try
            {
                scheduler.ListenerManager.AddJobListener(new SchedulerJobListener(), GroupMatcher<JobKey>.AnyGroup());
                await scheduler.Start();
                var quartzManager = new QuartzManager();
                await quartzManager.JobScheduler(scheduler);
                _logger.Info(string.Format("{0} Start", ServiceName));
            }
            catch (Exception ex)
            {
                _logger.Fatal($"Scheduler start failed: {ex.Message}", ex);
                throw;
            }

            _logger.Info("Scheduler started successfully");
        }

        /// <summary>
        /// Stops this instance, delegates to scheduler.
        /// </summary>
        public virtual async void Stop()
        {
            try
            {
                await scheduler.Shutdown(true);
            }
            catch (Exception ex)
            {
                _logger.Error($"Scheduler stop failed: {ex.Message}", ex);
                throw;
            }

            _logger.Info("Scheduler shutdown complete");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
	    public virtual void Dispose()
        {
            // no-op for now
        }

        /// <summary>
        /// Pauses all activity in scheduler.
        /// </summary>
	    public virtual async void Pause()
        {
            await scheduler.PauseAll();
        }

        /// <summary>
        /// Resumes all activity in server.
        /// </summary>
	    public async void Resume()
        {
            await scheduler.ResumeAll();
        }



        public bool Start(HostControl hostControl)
        {
            _logger.Info(" Start");
            Start();
            _logger.Info(" Started");
            return true;
        }


        public bool Stop(HostControl hostControl)
        {
            Stop();
            return true;
        }

        public bool Continue(HostControl hostControl)
        {
            Resume();
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            Pause();
            return true;
        }

    }
}
