namespace Company.PL.Helpers
{
    public static class DocumentSettings
    {
        //upload
        public static string UploadFile(IFormFile file, string foldername)
        {
            //1.Get folder location
            //string folderPath = "C:\\Users\\fatma\\source\\repos\\Company.Fatma01\\Company.Fatma01\\wwwroot\\files\\Images\\" + foldername;
            //var folderPath =Directory.GetCurrentDirectory()+"\\wwwroot\\files" + foldername;
            var folderPath = Path.Combine(Directory.GetCurrentDirectory() , @"wwwroot\files" , foldername);
            //2. get FileName and make it unique

            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            //3.file path
            var filepath = Path.Combine(folderPath, fileName);
           using var filestream = new FileStream(filepath,FileMode.Create);
            file.CopyTo(filestream);

            return fileName ;

        }
        //delete
        public static void DeleteFile(string fileName , string foldername)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files" ,foldername , fileName);
            if (File.Exists(filepath)) 
                File.Delete(filepath);
        }
    }
}
