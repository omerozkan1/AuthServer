using System;
using System.Collections.Generic;

namespace SharedLibrary.DTO
{
    public class ErrorDTO
    {
        public List<String> Errors { get; private set; }
        public bool IsShow { get; private set; }
        public ErrorDTO()
        {
            Errors = new List<string>();
        }
        public ErrorDTO(string error, bool isShow)
        {
            Errors.Add(error);
            isShow = true;
        }
        public ErrorDTO(List<string> errors, bool isShow)
        {
            Errors = errors;
            IsShow = isShow;
        }
    }
}
