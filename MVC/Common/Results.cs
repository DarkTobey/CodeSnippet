using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MVC.Common
{
    public interface IResult
    {
        String GetResult();
    }

    public class StringResult : IResult
    {
        public string Content { get; set; }

        public StringResult(string content)
        {
            Content = content;
        }

        public string GetResult()
        {
            return Content;
        }
    }

    public class ViewResult : IResult
    {
        public string ViewPath { get; set; }

        public ViewResult(string path)
        {
            ViewPath = path;
        }

        public string GetResult()
        {
            //TODO 根据path，利用模板解析引擎（Razor等）, 获取模板脚本被解析后的结果，并返回
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ViewPath));
        }
    }

}
