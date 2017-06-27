
using System;
using Newtonsoft.Json;
using NUnit.Framework;
using SingingFinder.Core;

namespace SingingFinder.Web.Tests
{
    public class SerializationTests
    {
        [TestCase(0, ExpectedResult = @"""All""")]
        [TestCase(1, ExpectedResult = @"""1991 Edition""")]
        [TestCase(3, ExpectedResult = @"""1991 Edition, Cooper Edition""")]
        public string BookIsSerializedProperly(int test) => JsonConvert.SerializeObject((Book)test);

        [TestCase(0, ExpectedResult = "All")]
        [TestCase(1, ExpectedResult = "1991 Edition")]
        [TestCase(3, ExpectedResult = "1991 Edition, Cooper Edition")]
        public string BookToString(int test) => ((Book)test).ToString();
    }
}
