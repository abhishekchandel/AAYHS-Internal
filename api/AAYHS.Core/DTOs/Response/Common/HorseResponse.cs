﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response.Common
{
    public class HorseResponse
    {
        public int HorseId { get; set; }
        public string Name { get; set; }
        public int HorseTypeId { get; set; }
        public int Number { get; set; }
        public int GroupId { get; set; }
        public string JumpHeight { get; set; }
        public bool NSBAIndicator { get; set; }
        public int MyProperty { get; set; }

    }
}
