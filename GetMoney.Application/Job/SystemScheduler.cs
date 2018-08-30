using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetMoney.Application.Job
{
    public class SystemScheduler
    {
        public static SystemScheduler CreateInstance()
        {
            return new SystemScheduler();
        }
        private IScheduler _scheduler;
        public void StartScheduler()
        {
            //这里读取配置文件中的任务开始时间
            // int Seconds = int.Parse(((NameValueCollection)ConfigurationSettings.GetConfig("JobList/Job"))["MYloopTime"]);
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();//内存调度
            _scheduler = schedulerFactory.GetScheduler();

            //创建一个Job来执行特定的任务
            IJobDetail synchronousData = new JobDetailImpl("JobBLL", typeof(JobBLL));
            //创建并定义触发器的规则（每天执行一次时间为：时：分）
            ITrigger trigger = TriggerBuilder.Create()
                                       .WithIdentity("多少时间执行", "90秒")
                                       .StartNow()                        //现在开始
                                       .WithSimpleSchedule(x => x         //触发时间，5秒一次。
                                           .WithIntervalInHours(24)
                                           .RepeatForever())              //不间断重复执行
                                       .Build();
            //将创建好的任务和触发规则加入到Quartz中
            _scheduler.ScheduleJob(synchronousData, trigger);
            //开始
            _scheduler.Start();
        }
        public void StopScheduler()
        {
            _scheduler.Shutdown();
        }
    }
}
