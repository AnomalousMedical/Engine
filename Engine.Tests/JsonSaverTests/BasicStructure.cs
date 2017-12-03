using Engine.Saving;
using Engine.Saving.JsonSaver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Threax.AspNetCore.Tests;
using Xunit;

namespace Engine.Tests.JsonSaverTests
{ 
    class SimpleSaveable<T> : Saveable
    {
        public SimpleSaveable()
        {

        }

        public T TheProp { get; set; }

        protected SimpleSaveable(LoadInfo info)
        {
            ReflectedSaver.RestoreObject(this, info);
        }

        public void getInfo(SaveInfo info)
        {
            ReflectedSaver.SaveObject(this, info);
        }
    }

    public class BasicStructure
    {
        private Mockup mockup = new Mockup();

        public BasicStructure()
        {
            mockup.Add<TestTypeFinder>(s => new TestTypeFinder());

            mockup.Add<JsonSaver>(s => new JsonSaver(s.Get<TestTypeFinder>()));
        }

        private const String FilePath = "../../../JsonSaverTests/BasicStructure.json";

        private readonly SimpleSaveable<bool> Data = new SimpleSaveable<bool>()
        {
            TheProp = true
        };

        [Fact]
        public void Save()
        {
            var saver = new JsonSaver();
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            saver.saveObject(Data, new JsonTextWriter(stringWriter)
            {
                Formatting = Formatting.Indented
            });

            var json = sb.ToString();
            File.WriteAllText(FilePath, json);
        }

        [Fact]
        public void Load()
        {
            SimpleSaveable<bool> loaded;
            using (var stream = new StreamReader(File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                var saver = new JsonSaver(new TestTypeFinder());
                loaded = saver.restoreObject<SimpleSaveable<bool>>(new JsonTextReader(stream));
            }

            Assert.Equal(Data.TheProp, loaded.TheProp);
        }
    }
}



