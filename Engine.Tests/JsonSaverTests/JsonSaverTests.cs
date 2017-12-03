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
    public class SaveTest<T> : Saveable
    {
        public SaveTest()
        {

        }

        public T TheProp { get; set; }

        protected SaveTest(LoadInfo info)
        {
            ReflectedSaver.RestoreObject(this, info);
        }

        public void getInfo(SaveInfo info)
        {
            ReflectedSaver.SaveObject(this, info);
        }
    }

    public class BlobTest : JsonTestBase<byte[]>
    {
        public BlobTest() : base(new byte[] { 1, 2, 3, 4, 5 }) { }

        protected override void Check(SaveTest<byte[]> expected, SaveTest<byte[]> loaded)
        {
            throw new NotImplementedException("Do a check that compares the byte arrays");
            //Assert.(expected.TheProp, loaded.TheProp);
        }
    }

    public class BooleanTest : JsonTestBase<bool>
    {
        public BooleanTest() : base(true) { }

        protected override void Check(SaveTest<bool> expected, SaveTest<bool> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class ByteTest : JsonTestBase<byte>
    {
        public ByteTest() : base(byte.MaxValue) { }

        protected override void Check(SaveTest<byte> expected, SaveTest<byte> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class CharTest : JsonTestBase<char>
    {
        public CharTest() : base('y') { }

        protected override void Check(SaveTest<char> expected, SaveTest<char> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class ColorTest : JsonTestBase<Color>
    {
        public ColorTest() : base(new Color(0.1f, 0.2f, 0.3f, 0.4f)) { }

        protected override void Check(SaveTest<Color> expected, SaveTest<Color> loaded)
        {
            Assert.Equal(expected.TheProp.r, loaded.TheProp.r);
            Assert.Equal(expected.TheProp.g, loaded.TheProp.g);
            Assert.Equal(expected.TheProp.b, loaded.TheProp.b);
            Assert.Equal(expected.TheProp.a, loaded.TheProp.a);
        }
    }

    public class DecimalTest : JsonTestBase<decimal>
    {
        public DecimalTest() : base(decimal.MaxValue) { }

        protected override void Check(SaveTest<decimal> expected, SaveTest<decimal> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class DoubleTest : JsonTestBase<double>
    {
        public DoubleTest() : base(double.MaxValue) { }

        protected override void Check(SaveTest<double> expected, SaveTest<double> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public enum ReallyCoolEnum
    {
        Choice1,
        Option2,
        Value3
    }

    public class EnumTest : JsonTestBase<ReallyCoolEnum>
    {
        public EnumTest() : base(ReallyCoolEnum.Option2) { }

        protected override void Check(SaveTest<ReallyCoolEnum> expected, SaveTest<ReallyCoolEnum> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class FloatTest : JsonTestBase<float>
    {
        public FloatTest() : base(float.MaxValue) { }

        protected override void Check(SaveTest<float> expected, SaveTest<float> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class GuidTest : JsonTestBase<Guid>
    {
        public GuidTest() : base(Guid.Empty) { }

        protected override void Check(SaveTest<Guid> expected, SaveTest<Guid> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class IntTest : JsonTestBase<int>
    {
        public IntTest() : base(int.MaxValue) { }

        protected override void Check(SaveTest<int> expected, SaveTest<int> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class LongTest : JsonTestBase<long>
    {
        public LongTest() : base(long.MaxValue) { }

        protected override void Check(SaveTest<long> expected, SaveTest<long> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class QuaternionTest : JsonTestBase<Quaternion>
    {
        public QuaternionTest() : base(new Quaternion(0.1f, 0.2f, 0.3f, 0.4f)) { }

        protected override void Check(SaveTest<Quaternion> expected, SaveTest<Quaternion> loaded)
        {
            Assert.Equal(expected.TheProp.x, loaded.TheProp.x);
            Assert.Equal(expected.TheProp.y, loaded.TheProp.y);
            Assert.Equal(expected.TheProp.z, loaded.TheProp.z);
            Assert.Equal(expected.TheProp.w, loaded.TheProp.w);
        }
    }

    public class Ray3Test : JsonTestBase<Ray3>
    {
        public Ray3Test() : base(new Ray3(new Vector3(0.1f, 0.2f, 0.3f), new Vector3(0.4f, 0.5f, 0.6f))) { }

        protected override void Check(SaveTest<Ray3> expected, SaveTest<Ray3> loaded)
        {
            Assert.Equal(expected.TheProp.Origin.x, loaded.TheProp.Origin.x);
            Assert.Equal(expected.TheProp.Origin.y, loaded.TheProp.Origin.y);
            Assert.Equal(expected.TheProp.Origin.z, loaded.TheProp.Origin.z);

            Assert.Equal(expected.TheProp.Direction.x, loaded.TheProp.Direction.x);
            Assert.Equal(expected.TheProp.Direction.y, loaded.TheProp.Direction.y);
            Assert.Equal(expected.TheProp.Direction.z, loaded.TheProp.Direction.z);
        }
    }

    public class SaveableTest : JsonTestBase<SaveTest<int>>
    {
        public SaveableTest() : base(new SaveTest<int>() { TheProp = 5 }) { }

        protected override void Check(SaveTest<SaveTest<int>> expected, SaveTest<SaveTest<int>> loaded)
        {
            Assert.Equal(expected.TheProp.TheProp, loaded.TheProp.TheProp);
        }
    }

    public class SByteTest : JsonTestBase<SByte>
    {
        public SByteTest() : base(sbyte.MaxValue) { }

        protected override void Check(SaveTest<SByte> expected, SaveTest<SByte> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class ShortTest : JsonTestBase<short>
    {
        public ShortTest() : base(short.MaxValue) { }

        protected override void Check(SaveTest<short> expected, SaveTest<short> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class StringTest : JsonTestBase<String>
    {
        public StringTest() : base("I'm a string <>{}\"[]") { }

        protected override void Check(SaveTest<String> expected, SaveTest<String> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class UIntTest : JsonTestBase<uint>
    {
        public UIntTest() : base(uint.MaxValue) { }

        protected override void Check(SaveTest<uint> expected, SaveTest<uint> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class ULongTest : JsonTestBase<ulong>
    {
        public ULongTest() : base(ulong.MaxValue) { }

        protected override void Check(SaveTest<ulong> expected, SaveTest<ulong> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class UShortTest : JsonTestBase<ushort>
    {
        public UShortTest() : base(ushort.MaxValue) { }

        protected override void Check(SaveTest<ushort> expected, SaveTest<ushort> loaded)
        {
            Assert.Equal(expected.TheProp, loaded.TheProp);
        }
    }

    public class Vector3Test : JsonTestBase<Vector3>
    {
        public Vector3Test() : base(new Vector3(0.1f, 0.2f, 0.3f)) { }

        protected override void Check(SaveTest<Vector3> expected, SaveTest<Vector3> loaded)
        {
            Assert.Equal(expected.TheProp.x, loaded.TheProp.x);
            Assert.Equal(expected.TheProp.y, loaded.TheProp.y);
            Assert.Equal(expected.TheProp.z, loaded.TheProp.z);
        }
    }

    public abstract class JsonTestBase<T>
    {
        private Mockup mockup = new Mockup();
        private String filePath;
        private bool writeFile;
        private SaveTest<T> data;

        public JsonTestBase(T value)
        {
            mockup.Add<TestTypeFinder>(s => new TestTypeFinder());

            mockup.Add<JsonSaver>(s => new JsonSaver(s.Get<TestTypeFinder>()));

            filePath = $"../../../JsonSaverTests/Examples/{this.GetType().Name}.json";
            writeFile = Directory.Exists(Path.GetDirectoryName(filePath));

            data = new SaveTest<T>()
            {
                TheProp = value
            };
        }

        [Fact]
        public void Save()
        {
            var saver = mockup.Get<JsonSaver>();
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            saver.saveObject(data, new JsonTextWriter(stringWriter)
            {
                Formatting = Formatting.Indented,
            });

            var json = sb.ToString();

            if (writeFile)
            {
                File.WriteAllText(filePath, json);
            }
        }

        [Fact]
        public void Load()
        {
            var saver = mockup.Get<JsonSaver>();

            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            saver.saveObject(data, new JsonTextWriter(stringWriter));

            var json = sb.ToString();

            var loaded = saver.restoreObject<SaveTest<T>>(new JsonTextReader(new StringReader(json)));
            Check(data, loaded);
        }

        protected abstract void Check(SaveTest<T> expected, SaveTest<T> loaded);
    }
}



