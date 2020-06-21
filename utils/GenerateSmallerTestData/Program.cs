﻿using System;
using System.IO;
using System.Linq;

namespace GenerateTestData
{
    class Program
    {
        // This program takes as input the output of 
        // https://raw.githubusercontent.com/remyoudompheng/fptest/master/fptest.py
        // and parses it generating data for the tests


        static void Main(string[] args)
        {
            try
            {
                var filenames = new string[] { "parse64minus.txt", "parse64plus.txt" };

                using (var outputFile = new StreamWriter(Path.Combine("..", "..", "..", "..", "..", "Ryu.Net.s2d_data", "SmallTestSet.cs")))
                {
                    outputFile.WriteLine(
@$"namespace RyuDotNet.UnitTests.s2d_data
{{
static byte[][] _TestAsciiArray = null;

            public static byte[][] TestAsciiArray
            {{
                get
                {{
                    if(_TestAsciiArray==null)
                    {{
                       _TestAsciiArray = TestArray.Select(a => Encoding.ASCII.GetBytes(a)).ToArray();
                    }}
                return _TestAsciiArray;
                }}
            }}

            public  static string[] TestArray =>  new string[]
            {{
                    // Numbers generated by Remy's Iterator
");
                    foreach (var inputFilename in filenames)
                    {
                        using (var inputFile = new StreamReader(inputFilename))
                        {

                            while (!inputFile.EndOfStream)
                            {

                                int lineNumber = 0;
                                string line;
                                while ((line = inputFile.ReadLine()) != null && lineNumber < 5000)
                                {
                                    if ((lineNumber % 600) == 0)
                                    {
                                        var tokens = line.Split(null).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                                        var theDouble = tokens[1];
                                        outputFile.WriteLine(@$"                 ""{theDouble}"",");
                                    }
                                    ++lineNumber;
                                }


                            }
                        }
                    }
                    outputFile.WriteLine(
@"
                    // Cos (x) for x= to Pi step Pi/360");
                    double delta =  Math.PI/360.0;
                    double angle = 0.0;
                    for (int n = 0; n < 360; ++n)
                    {
                        var theDouble = Math.Cos(angle);
                        outputFile.WriteLine(@$"                 ""{theDouble}"",");
                        angle += delta;

                    }
                    outputFile.WriteLine(
 @"
                    // Dates as excel values");
                    var bd = new DateTime(2007, 08, 25);
                    while (bd < DateTime.Today)
                    {
                        var theDouble = bd.ToOADate();
                        outputFile.WriteLine(@$"                 ""{theDouble}"",");
                        bd = bd.AddHours(23);

                    }

                    outputFile.WriteLine(
@"
                    // Sparse");

                    for (int n = 1; n < 200; ++n)
                    {
                        var theDouble = (1 + 5e-16) * (double)n;
                        outputFile.WriteLine(@$"                 ""{theDouble.ToString("G17")}"",");
                        bd = bd.AddHours(23);

                    }


                    outputFile.WriteLine(@$"
            }};
    }}
}}
             
");

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }
    }
}
