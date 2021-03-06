﻿using RyuDotNet.Internal;
using RyuDotNet.UnitTests.s2d_data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace RyuDotNet.UnitTests
{
    unsafe public class Test_s2d_n 
    {
        //[Theory]
        [ClassData(typeof(DataGenerator))]
        public void TestDataSelfConsistency(string str, double val) =>
            Assert.Equal(double.Parse(str), val);



        // Probably not best to run these tests through VS's test runner
        // Run them ftom the command line  'dotnet test RyuCsharp.UnitTests.dll'
        [Theory]
        [InlineData("1.2999999999999999E+154", 1.2999999999999999E+154)]
        [ClassData(typeof(DataGenerator))]
        public void TestWithRemysIteratoutput(string str, double val)
        {

        

            double ryuVale;
            var eq1 = RyuDotNet.Internal.Ryu.s2d_n(str.AsSpan(), out ryuVale);
            Assert.Equal(Status.SUCCESS, eq1);
            
           
            //Uncomment the following code to produce a text file
            //summarising the Failed tests. 
            //if (ryuVale.ToString("G18") != double.Parse(str).ToString("G18"))
            //{
            //    File.AppendAllText(@"../.././../FAILS.txt", $"string={str}  ryuVal={ryuVale.ToString("G18")}  cs={double.Parse(str).ToString("G18")}\r\n");
            //}


            Assert.Equal(ryuVale.ToString("G18"), double.Parse(str).ToString("G18"));

        }


    }

    public class DataGenerator : IEnumerable<object[]>
    {
        IEnumerable<IFPTestData> _allFPTestDatas;
        public DataGenerator()
        {
            _allFPTestDatas = typeof(IFPTestData).Assembly.
                GetTypes().
                Where(t => typeof(IFPTestData).IsAssignableFrom(t) && !t.IsInterface).
                Select(t => (IFPTestData)Activator.CreateInstance(t));
        }


        public IEnumerator<object[]> GetEnumerator()
        {
            foreach(var klass in _allFPTestDatas)
            {
                var text = File.ReadLines(klass.TextFileName);
                var pairs = text.Zip(klass.TestArray);
                foreach (var kv in pairs)
                {
                    yield return new object[] { kv.First, kv.Second };
                }
               
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }

}
