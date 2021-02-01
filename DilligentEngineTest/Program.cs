using DilligentEngine;
using System;

namespace DilligentEngineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var engineFactory = new GenericEngineFactory();
            engineFactory.CreateDeviceAndSwapChain(IntPtr.Zero);

            Console.WriteLine("Hello World!");
        }
    }
}
