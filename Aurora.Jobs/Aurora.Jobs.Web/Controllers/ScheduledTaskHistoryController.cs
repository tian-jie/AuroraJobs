using Aurora.Jobs.Core.Common;
using Aurora.Jobs.Core.Services;
using System.Web.Mvc;

namespace Aurora.Jobs.Web.Controllers
{
    public class ScheduledTaskHistoryController : BaseController
    {
        //
        // GET: /ScheduledTaskHistory/

        [HttpGet]
        public ActionResult List()
        {
            ScheduledTaskService _ScheduledTaskService = new ScheduledTaskService();
            var data = _ScheduledTaskService.GetScheduledTaskHistoryInfoPagerList(this.GetPageParameter());
            var result = new ResponseResult() { success = true, message = "数据获取成功", data = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Info()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InfoData(System.Guid ScheduledTaskHistoryId)
        {
            var result = new ResponseResult();
            ScheduledTaskService _ScheduledTaskService = new ScheduledTaskService();
            result.data = _ScheduledTaskService.GetScheduledTaskHistoryInfo(ScheduledTaskHistoryId);
            result.success = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(string idList)
        {
            var result = new ResponseResult();
            ScheduledTaskService _ScheduledTaskService = new ScheduledTaskService();
            result.success = _ScheduledTaskService.DeleteScheduledTaskHistory(Utils.StringToGuidList(idList));
            result.message = result.success == true ? "操作成功" : "操作失败";
            return Json(result);
        }
    }
}
