using System;
using Newtonsoft.Json;

namespace LotteyServerApp.Models
{
    [Serializable]
    public class ResponseMessage : EventArgs
    {
        public int Code;
        public string Message;

        public ResponseMessage(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}