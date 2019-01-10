﻿namespace Library.Logic.ViewModel
{
    public class NullSourceProvider : ISourceProvider
    {
        public bool GetAccess()
        {
            return true;
        }

        public string GetFilePath()
        {
            return "";
        }
    }
}