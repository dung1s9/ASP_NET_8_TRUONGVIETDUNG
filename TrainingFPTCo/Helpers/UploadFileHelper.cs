namespace TrainingFPTCo.Helpers
{
    public static class UploadFileHelper
    {
        public static string UpLoadFile(IFormFile file, string folderName)
        {
            string uniqueFileName;
            try
            {
                string pathUploadServer = string.Empty;
                switch (folderName.ToLower())
                {
                    case "images":
                        pathUploadServer = "wwwroot\\uploads\\images";
                        break;
                    case "attachfiles":
                        pathUploadServer = "wwwroot\\uploads\\attachfiles";
                        break;
                    case "documents":
                        pathUploadServer = "wwwroot\\uploads\\documents";
                        break;

                }
                string fileName = file.FileName;
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                fileNameWithoutExtension = CommonText.GenerateSlug(fileNameWithoutExtension);

                string ext = Path.GetExtension(fileName);

                // tao ten file khong trung nhau
                string uniqueStr = Guid.NewGuid().ToString(); // random string
                string fileNameUpload = uniqueStr + "-" + fileNameWithoutExtension + ext;

                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), pathUploadServer, fileNameUpload);
                var stream = new FileStream(uploadPath, FileMode.Create);
                file.CopyToAsync(stream);
                uniqueFileName = fileNameUpload;
            }
            catch (Exception ex)
            {
                uniqueFileName = ex.Message.ToString();
            }
            return uniqueFileName;
        }
    }
}
