using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneGameService.Models.EdgeConditionals
{
    public class TransitionResult
    {
        public TransitionResult() { Success = true; Message = ""; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
