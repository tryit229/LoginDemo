using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberDemo.Models
{
    public class Response<TData>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public TData Data { get; set; }

        //public string ErrorID { get; set; }
        //TODO 若有定義錯誤帶碼，可供前端更有效率判斷錯誤訊息，也方便多語系的置換。
    }
}