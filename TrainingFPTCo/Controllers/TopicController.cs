using Microsoft.AspNetCore.Mvc;
using TrainingFPTCo.Models.Queries;
using TrainingFPTCo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingFPTCo.Helpers;

namespace TrainingFPTCo.Controllers
{
    public class TopicController : Controller
    {
        [HttpGet]
        public IActionResult Index(string? keyword,string? filter)
        {
            TopicViewModel topic = new TopicViewModel();
            topic.TopicDetailsList = new List<TopicDetail>();
            var dataTopic = new TopicQuery().GetAllDataTopics(keyword, filter);
            var courseQuery = new CourseQuery();
            foreach (var data in dataTopic)
            {
                string courseName = courseQuery.GetCourseNameById(data.CourseId);
                topic.TopicDetailsList.Add(new TopicDetail
                {
                    Id = data.Id,
                    Name = data.Name,
                    CourseId = data.CourseId,
                    Description = data.Description,
                    TypeDocument = data.TypeDocument,
                    Status = data.Status,
                    ViewDocumentTopic = data.ViewDocumentTopic,
                    ViewAttachFileTopic = data.ViewAttachFileTopic,
                    ViewPosterTopic = data.ViewPosterTopic,
                    ViewCourseName = courseName
                });
            }
            ViewData["currentFilter"] = keyword;
            ViewBag.FilterStatus = filter;
            return View(topic);
        }

        [HttpGet]
        public IActionResult Add()
        {
            TopicDetail topic = new TopicDetail();

            List<SelectListItem> itemCourse = new List<SelectListItem>();
            var dataTopic = new CourseQuery().GetAllDataCourses();
            foreach (var item in dataTopic)
            {
                itemCourse.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.Courses = itemCourse;
            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TopicDetail topic, IFormFile Document, IFormFile? Attachfile, IFormFile PosterTopic)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    string nameDocument = UploadFileHelper.UpLoadFile(Document, "documents");
                    string nameAttchfile = UploadFileHelper.UpLoadFile(Attachfile, "attachfiles");
                    string namePosterTopic = UploadFileHelper.UpLoadFile(PosterTopic, "images");
                    int idTopic = new TopicQuery().InsertTopic(
                        topic.Name,
                        topic.CourseId,
                        topic.Description,
                        topic.TypeDocument,
                        topic.Status,
                        nameDocument,
                        nameAttchfile,
                        namePosterTopic
                    );
                    if (idTopic > 0)
                    {
                        TempData["saveStatus"] = true;
                    }
                    else
                    {
                        TempData["saveStatus"] = false;
                    }
                    // quay lai trang danh sach courses
                    return RedirectToAction(nameof(TopicController.Index), "Topic");
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message);
                }
            }
            List<SelectListItem> itemCourse = new List<SelectListItem>();
            var dataTopic = new CourseQuery().GetAllDataCourses();
            foreach (var item in dataTopic)
            {
                itemCourse.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.Courses = itemCourse;
            return View(topic);

        }

        [HttpPost]
        public JsonResult Delete(int id = 0)
        {
            bool deleteTopic = new TopicQuery().DeleteTopicById(id);
            if (deleteTopic)
            {
                return Json(new { cod = 200, message = "Successfully" });
            }
            return Json(new { cod = 500, message = "Failure" });
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            TopicDetail detail = new TopicQuery().GetDetailTopicById(id);
            List<SelectListItem> itemCourse = new List<SelectListItem>();
            var dataTopic = new CourseQuery().GetAllDataCourses();
            foreach (var item in dataTopic)
            {
                itemCourse.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.Courses = itemCourse;
            return View(detail);
        }

        [HttpPost]
        public IActionResult Update(TopicDetail topicDetail, IFormFile Document, IFormFile AttachFile, IFormFile PosterTopic)
        {
            try
            {
                var infoTopic = new TopicQuery().GetDetailTopicById(topicDetail.Id);
                string document = infoTopic.ViewDocumentTopic;
                string attachFile = infoTopic.ViewAttachFileTopic;
                string posterTopic = infoTopic.ViewPosterTopic;
                // check xem nguoi co thay anh hay ko?
                if (topicDetail.Document != null)
                {

                    document = UploadFileHelper.UpLoadFile(Document, "documents");
                }
                if (topicDetail.AttachFile != null)
                {

                    attachFile = UploadFileHelper.UpLoadFile(AttachFile, "attachfiles");
                }
                if (topicDetail.PosterTopic != null)
                {

                    posterTopic = UploadFileHelper.UpLoadFile(PosterTopic, "images");
                }
                bool update = new TopicQuery().UpdateTopicById(
                        topicDetail.Id,
                        topicDetail.Name,
                        topicDetail.CourseId,
                        topicDetail.Description,
                        topicDetail.TypeDocument,
                        topicDetail.Status,
                        document,
                        attachFile,
                        posterTopic

                        );
                if (update)
                {
                    TempData["saveUpdate"] = true;
                }
                else
                {
                    TempData["saveUpdate"] = false;
                }
                return RedirectToAction("Index", "Topic");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            List<SelectListItem> itemCourse = new List<SelectListItem>();
            var dataTopic = new CourseQuery().GetAllDataCourses();
            foreach (var item in dataTopic)
            {
                itemCourse.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.Courses = itemCourse;
            return View(topicDetail);
        }
    }
}