using Service;
using System;

namespace LineBot
{
    public class Factory
    {
        //public static ILineHandler CreateLineHandler() => new LineHandler();
        public static ILineHandler CreateLineHandler() => fMyLazy.Value;

        private static readonly Lazy<ILineHandler> fMyLazy = new Lazy<ILineHandler>(() => new LineHandler());
    }
}