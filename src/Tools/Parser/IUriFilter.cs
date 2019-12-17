using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserTool
{
    public interface IUriFilter
    {
        bool Accept(Uri uri);
    }

    public class AnyUriFilter : IUriFilter
    {
        public bool Accept(Uri uri)
        {
            return true;
        }

        public static readonly IUriFilter Instance = new AnyUriFilter();
    }

   

    public class AndUriFilter : IUriFilter
    {
        private readonly IUriFilter[] _filters;

        public AndUriFilter(params IUriFilter[] filters)
        {
            _filters = filters;
        }

        public bool Accept(Uri uri)
        {
            return _filters.All(f => f.Accept(uri));
        }
    }

    public class AcceptOnceUriFilter : IUriFilter
    {
        private HashSet<string> _known = new HashSet<string>();

        public bool Accept(Uri uri)
        {
            var url = uri.AbsoluteUri.ToLower();
            return _known.Add(url);
        }
    }


    public class CalorizatorProductsUriFilter : IUriFilter
    {
        private static Uri Root = new Uri("http://www.calorizator.ru/product/all");
        public bool Accept(Uri uri)
        {
            if (Root.IsBaseOf(uri) && uri.AbsolutePath == Root.AbsolutePath)
            {
                var @params = System.Web.HttpUtility.ParseQueryString(uri.Query);
                return @params.Count == 0 
                       || (@params.Count == 1 && string.Equals(@params.GetKey(0), "page", StringComparison.OrdinalIgnoreCase)
                         /*  && int.Parse(@params.Get(0))<=2*/);
            }
            return false;

        }
    }
}