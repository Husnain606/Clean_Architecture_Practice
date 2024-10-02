﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Application.Services.Common
{
    public class GridDto<T> where T : class
    {
        public List<T> Data { get; set; } = null!;
        public int TotalRecords { get; set; }
    }
}