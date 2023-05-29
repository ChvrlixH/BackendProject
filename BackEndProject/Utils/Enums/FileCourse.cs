﻿namespace BackEndProject.Utils.Enums
{
    public static class FileCourse
    {
        public static void DeleteFile(params string[] path)
        {
            string resultPath = Path.Combine(path);
            if (System.IO.File.Exists(resultPath))
            {
                System.IO.File.Delete(resultPath);
            }
        }
    }
}
