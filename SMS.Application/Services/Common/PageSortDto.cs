﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Application.Services.Common
{
    public class PageSortDto : PageDto
    {
        public string? SortBy { get; set; }
        public bool SortOrder { get; set; }
    }
}
