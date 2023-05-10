﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities.Result
{
    public interface IResult
    {
        bool Success { get; }
        string? Message { get; }
    }
}
