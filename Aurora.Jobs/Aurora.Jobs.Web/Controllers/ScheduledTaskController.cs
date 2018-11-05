using Aurora.Jobs.Core.Business.enums;
using Aurora.Jobs.Core.Business.Info;
using Aurora.Jobs.Core.Common;
using Aurora.Jobs.Core.Services;
using System.Web.Mvc;

namespace Aurora.Jobs.Web.Controllers
{
    public class ScheduledTaskController : BaseController
    {
        //
        // GET: /ScheduledTask/

        [HttpGet]
        public ActionResult List()
        {
            ScheduledTaskService _scheduledTaskService = new ScheduledTaskService();
            var data = _scheduledTaskService.GetScheduledTaskInfoPagerList(this.GetPageParameter());
            var result = new ResponseResult() { success = true, message = "数据获取成功", data = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPost(ScheduledTask info)
        {
            var result = new ResponseResult();
            ScheduledTaskService _scheduledTaskService = new ScheduledTaskService();
            result.success = _scheduledTaskService.InsertScheduledTask(info);
            return Json(result);
        }

        public ActionResult Info()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InfoData(int jobId)
        {
            var result = new ResponseResult();
            ScheduledTaskService _scheduledTaskService = new ScheduledTaskService();
            result.data = _scheduledTaskService.GetScheduledTaskInfo(jobId);
            result.success = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdatePost(ScheduledTask scheduledTask)
        {
            var result = new ResponseResult();
            ScheduledTaskService _scheduledTaskService = new ScheduledTaskService();
            result.success = _scheduledTaskService.UpdateScheduledTask(scheduledTask);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(string idList)
        {
            var result = new ResponseResult();
            ScheduledTaskService _scheduledTaskService = new ScheduledTaskService();
            string rtMsg = string.Empty;
            result.success = _scheduledTaskService.DeleteScheduledTask(Utils.StringToIntList(idList), out rtMsg);
            result.message = rtMsg;
            return Json(result);
        }

        [HttpPost]
        public ActionResult UpdateState(int jobId, JobStatus state)
        {
            var result = new ResponseResult();
            ScheduledTaskService _scheduledTaskService = new ScheduledTaskService();
            result.success = _scheduledTaskService.UpdateScheduledTaskState(jobId, state);
            result.message = result.success == true ? "操作成功" : "操作失败";
            return Json(result);
        }

    }
}
