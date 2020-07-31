using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response.Common
{
    public class StateResponse
    {
        public List<State> State { get; set; }
    }
    public class State
    {
        public int StateId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
