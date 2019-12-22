using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Dto
{
    public class JsonResponse
    {
        public long took { get; set; }
        public string message { get; set; }
        public object body { get; set; }

        public JsonResponse()
        {

        }

        public JsonResponse(long took, string message, object body)
        {
            this.took = took;
            this.message = message;
            this.body= body;
        }
    }
}
