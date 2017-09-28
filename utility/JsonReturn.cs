using Microsoft.AspNetCore.Mvc;

namespace Utils
{
    public class JsonReturn : JsonResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }

        private JsonReturn(int code, string msg, object data) : base(new { code = code, msg = msg, data = data})
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
        }

        public static JsonReturn ReturnSuccess(object data)
        {
            return new JsonReturn(0, "", data);
        }
        public static JsonReturn ReturnSuccess()
        {
            return new JsonReturn(0, "", null);
        }
        public static JsonReturn ReturnFail(string msg)
        {
            return new JsonReturn(-1, msg, null);
        }
        public static JsonReturn ReturnFail(int code, string msg)
        {
            return new JsonReturn(code, msg, null);
        }
    }
}