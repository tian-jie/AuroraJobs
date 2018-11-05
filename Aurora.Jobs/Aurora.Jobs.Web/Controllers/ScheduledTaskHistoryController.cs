using Aurora.Jobs.Core.Common;
using Aurora.Jobs.Core.Services;
using System.Web.Mvc;

namespace Aurora.Jobs.Web.Controllers
{
    /// <summary>
    /// ScheduledTaskHistoryController
    /// </summary>
    public class ScheduledTaskHistoryController : BaseController
    {

        /// <summary>
        /// List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult List()
        {
            ScheduledTaskService _ScheduledTaskService = new ScheduledTaskService();
            var data = _ScheduledTaskService.GetScheduledTaskHistoryInfoPagerList(this.GetPageParameter());
            var result = new ResponseResult() { success = true, message = "数据获取成功", data = data };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Info
        /// </summary>
        /// <returns></returns>
        public ActionResult Info()
        {
            return View();
        }

        /// <summary>
        /// InfoData
        /// </summary>
        /// <param name="taskHistoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InfoData(int taskHistoryId)
        {
            var result = new ResponseResult();
            ScheduledTaskService _ScheduledTaskService = new ScheduledTaskService();
            result.data = _ScheduledTaskService.GetScheduledTaskHistoryInfo(taskHistoryId);
            result.success = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string idList)
        {
            var result = new ResponseResult();
            ScheduledTaskService _ScheduledTaskService = new ScheduledTaskService();
            result.success = _ScheduledTaskService.DeleteScheduledTaskHistory(Utils.StringToIntList(idList));
            result.message = result.success == true ? "操作成功" : "操作失败";
            return Json(result);
        }
    }
}
