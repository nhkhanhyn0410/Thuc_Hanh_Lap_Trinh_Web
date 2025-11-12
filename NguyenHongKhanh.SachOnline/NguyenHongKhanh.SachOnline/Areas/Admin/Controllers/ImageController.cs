using NguyenHongKhanh.SachOnline.Filters;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace NguyenHongKhanh.SachOnline.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class ImageController : Controller
    {
        /// <summary>
        /// Upload ảnh từ CKEditor
        /// </summary>
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload)
        {
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    // Lấy tên file
                    var fileName = Path.GetFileName(upload.FileName);

                    // Tạo tên file duy nhất để tránh trùng lặp
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

                    // Đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images"), uniqueFileName);

                    // Tạo thư mục Images nếu chưa tồn tại
                    var directory = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Lưu file
                    upload.SaveAs(path);

                    // Lấy CKEditorFuncNum từ query string
                    var CKEditorFuncNum = Request["CKEditorFuncNum"];

                    // URL của ảnh đã upload
                    var imageUrl = Url.Content("~/Images/" + uniqueFileName);

                    // Trả về script cho CKEditor
                    var script = $"<script>window.parent.CKEDITOR.tools.callFunction({CKEditorFuncNum}, '{imageUrl}', 'Upload thành công!');</script>";

                    return Content(script, "text/html");
                }
                else
                {
                    var CKEditorFuncNum = Request["CKEditorFuncNum"];
                    var script = $"<script>window.parent.CKEDITOR.tools.callFunction({CKEditorFuncNum}, '', 'Vui lòng chọn file ảnh!');</script>";
                    return Content(script, "text/html");
                }
            }
            catch (Exception ex)
            {
                var CKEditorFuncNum = Request["CKEditorFuncNum"];
                var script = $"<script>window.parent.CKEDITOR.tools.callFunction({CKEditorFuncNum}, '', 'Lỗi upload: {ex.Message}');</script>";
                return Content(script, "text/html");
            }
        }
    }
}
