using System;
using System.Collections.Generic;

namespace App.Business.ViewModels
{
    public class FilesSignViewModel
    {
        public Guid id { get; set; }

        public List<FilesViewModel> files { get; set; }
    }

    public class FilesViewModel
    {
        public Guid id { get; set; }

        public Guid idFileStore { get; set; }

        public string name { get; set; }

        public List<string> names { get; set; }

        public string file { get; set; }

        public bool isSystemFile { get; set; }

        public Dictionary<string, string> errors { get; set; }
    }
}
