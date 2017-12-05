using Engine.Saving;
using Engine.Tests.JsonSaverTests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Engine.Tests
{
    public class SaverTests
    {
        private Mockup mockup = new Mockup();
        private SaveTest<int> data;

        public SaverTests()
        {
            mockup.Add<TestTypeFinder>(s => new TestTypeFinder());

            mockup.Add<Saver>(s => new Saver(s.Get<TestTypeFinder>()));

            data = new SaveTest<int>()
            {
                TheProp = 6
            };
        }

        [Theory]
        [InlineData(SaverOutputType.Xml)]
        [InlineData(SaverOutputType.Json)]
        [InlineData(SaverOutputType.Bson)]
        public void Save(SaverOutputType type)
        {
            var saver = mockup.Get<Saver>();
            using (var stream = new MemoryStream())
            {
                saver.saveObject(data, stream, type);
            }
        }

        [Theory]
        [InlineData(SaverOutputType.Xml)]
        [InlineData(SaverOutputType.Json)]
        [InlineData(SaverOutputType.Bson)]
        public void Load(SaverOutputType type)
        {
            var saver = mockup.Get<Saver>();
            using (var stream = new MemoryStream())
            {
                saver.saveObject(data, stream, type);

                stream.Seek(0, SeekOrigin.Begin);

                var loaded = saver.restoreObject<SaveTest<int>>(stream, type);
                Assert.Equal(data.TheProp, loaded.TheProp);
            }
        }
    }
}
