using System;
using Dao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
//using JiebaNet.Analyser;
//using JiebaNet.Segmenter.PosSeg;
//using JiebaNet.Segmenter;
using Utils;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;

namespace web.Api.Controllers
{
    [Route("api/test")]
    public class TestController : ApiBaseController
    {
        public TestController(DwDbContext dbc, ILoggerFactory logFac, IServiceProvider svp) : base(dbc, logFac, svp)
        {
        }
        protected override void LoginFail(ActionExecutingContext context) { }
        // [HttpGet("jieba-test")]
        // public ActionResult JiebaTest(string sentence)
        // {
        //     var s = new JiebaSegmenter();
        //     var resArr = s.Cut(sentence);
        //     var res = string.Join(",", resArr);
        //     return JsonReturn.ReturnSuccess(res);
        // }
        [HttpGet("emit-test")]
        public ActionResult EmitTest()
        {
            var assemblyName = new AssemblyName("Etest");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("extraModule");
            var typeBuilder = moduleBuilder.DefineType("extra", TypeAttributes.Public);
            MethodBuilder sayHelloMethod = typeBuilder.DefineMethod("SayHello", MethodAttributes.Public, null, new Type[] { typeof(string) });
            ILGenerator ilOfSayHello = sayHelloMethod.GetILGenerator();
            ilOfSayHello.Emit(OpCodes.Ldstr, "hello");
            ilOfSayHello.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            ilOfSayHello.Emit(OpCodes.Pop);
            ilOfSayHello.Emit(OpCodes.Ret);
            return JsonReturn.ReturnSuccess("");
        }
        [HttpGet("run_shell")]
        public ActionResult RunShell(string filePath, string arguments)
        {
            var p = new Process();
            p.StartInfo.FileName = filePath;
            p.StartInfo.Arguments = arguments;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.WaitForExit();
            p.Close();
            return JsonReturn.ReturnSuccess("ok");
        }
    }
}