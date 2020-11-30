using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Mpmp19
{
    internal class Program
    {
        private static int ApproximateNthPrime(int nn)
        {
            double n = nn;

            var p = n * Math.Log(n) + n * (Math.Log(Math.Log(n)) - 0.9385);

            return (int) p;
        }

// Find all primes up to and including the limit
        private static BitArray SieveOfEratosthenes(int limit)
        {
            var bits = new BitArray(limit + 1, true) {[0] = false, [1] = false};
            for (var i = 0; i * i <= limit; i++)
            {
                if (!bits[i])
                {
                    continue;
                }

                for (var j = i * i; j <= limit; j += i) bits[j] = false;
            }

            return bits;
        }

        private static List<int> GeneratePrimesSieveOfEratosthenes(int n)
        {
            var sw = new Stopwatch();
            sw.Start();
            var limit = ApproximateNthPrime(n);
            var bits = SieveOfEratosthenes(limit);
            var primes = new List<int>();
            for (int i = 0, found = 0; i < limit && found < n; i++)
            {
                if (!bits[i])
                {
                    continue;
                }

                primes.Add(i);
                found++;
            }

            sw.Stop();
            Console.WriteLine($"{primes.Count} found in " + sw.ElapsedMilliseconds + " ms");
            return primes;
        }

        public static void Main(string[] args)
        {
            try
            {
                var outputs = new List<OutputData>();

                var maxPower = 40;
                var maxN = 100000000;
                //var primeList = GeneratePrimes(maxN);

                var primeList = GeneratePrimesSieveOfEratosthenes(maxN);

                double primePowSum = 0;
                BigInteger primePowSumBigInt = 0;


                for (var power = 1; power <= maxPower; power++)
                {
                    var primeNFactors = FindPrimeNFactorsBigInt(maxN, primePowSumBigInt, primeList, power);

                    var outputData = new OutputData();
                    outputData.Power = power;
                    outputData.PrimeNFactors = primeNFactors;
                    outputs.Add(outputData);
                    Console.WriteLine($"Power {power} found {primeNFactors.Count} prime n factors");
                }

                var maxout = 25;
                var pstr = "";

                for (var i = 0; i < 25; i++) pstr = pstr + primeList[i] + "\n";

                File.WriteAllText("primelist.txt", pstr);

                WriteGnuplotData(outputs);


                WriteFullOutput(outputs);

                Console.WriteLine("done");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void WriteFullOutput(List<OutputData> outputs)
        {
            var dataLines = new List<string>();

            var headerLine = "";
            foreach (var o in outputs) headerLine = headerLine + "\t" + o.Power;

            dataLines.Add(headerLine);
            var maxRow = 0;
            foreach (var o in outputs)
                if (o.PrimeNFactors.Count > maxRow)
                {
                    maxRow = o.PrimeNFactors.Count;
                }

            for (var r = 0; r < maxRow; r++)
            {
                var rowLine = r + "\t";
                foreach (var o in outputs)
                {
                    var num = "";
                    if (o.PrimeNFactors.Count > r)
                    {
                        num = o.PrimeNFactors[r].ToString();
                    }

                    rowLine = rowLine + num + "\t";
                }

                dataLines.Add(rowLine);
            }

            var dataStr = "";
            foreach (var d in dataLines) dataStr = dataStr + d + "\n";
            File.WriteAllText("Mpmp19AllData.txt", dataStr);
        }

        private static void WriteGnuplotData(List<OutputData> outputs)
        {
            var maxRows = outputs.Select(e => e.PrimeNFactors.Count).Max();

            var gnuData = "";
            foreach (var o in outputs)
                for (var indx = 0; indx < maxRows; indx++)
                {
                    var p = -1;
                    if (o.PrimeNFactors.Count > indx)
                    {
                        p = o.PrimeNFactors[indx];
                    }

                    gnuData += indx + "\t" + o.Power + "\t" + p + "\n";
                }

            File.WriteAllText("gnudata.txt", gnuData);
        }

        private static List<int> FindPrimeNFactorsBigInt(int maxN, BigInteger primePowSum, List<int> primeList, int power)
        {
            var primeNFactors = new List<int>();
            try
            {
                for (var n = 1; n < maxN; n++)
                {
                    var p = primeList[n - 1];

                    var bigPow = BigInteger.Pow(p, power);
                    primePowSum = primePowSum + bigPow;

                    var remainder = primePowSum % n;
                    var isFactor = remainder == 0;

                    if (isFactor)
                    {
                        primeNFactors.Add(n);
                        Console.WriteLine($"The sum of the first {n} primes to the power of {power} is {primePowSum}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Big problem!");
                Console.WriteLine(e.Message);
            }

            return primeNFactors;
        }

        private static List<int> FindPrimeNFactors(int maxN, double primePowSum, List<int> primeList, int power)
        {
            var primeNFactors = new List<int>();

            for (var n = 1; n < maxN; n++)
            {
                var p = primeList[n - 1];

                var ps = Math.Pow(Convert.ToDouble(p), power);
                primePowSum += ps;

                var remainder = primePowSum % n;
                var isFactor = remainder == 0;

                if (isFactor)
                {
                    primeNFactors.Add(n);
                }
            }

            return primeNFactors;
        }

        private static double MakePrimePowSum(ArrayList primeList, double prevSum, int n)
        {
            var p = primeList[n - 1];
            var ps = Math.Pow(Convert.ToDouble(p), 2);
            prevSum += ps;

            return prevSum;
        }

        private static ArrayList GeneratePrimes(int toGenerate)
        {
            var sw = new Stopwatch();
            sw.Start();
            var primes = new ArrayList();
            primes.Add(2);
            primes.Add(3);
            while (primes.Count <= toGenerate)
            {
                var nextPrime = (int) primes[primes.Count - 1] + 2;
                while (true)
                {
                    var isPrime = true;
                    foreach (int n in primes)
                        if (nextPrime % n == 0)
                        {
                            isPrime = false;
                            break;
                        }

                    if (isPrime)
                    {
                        break;
                    }

                    nextPrime += 2;
                }

                primes.Add(nextPrime);
            }

            sw.Stop();
            Console.WriteLine("Generate primes " + sw.ElapsedMilliseconds + " ms");
            return primes;
        }

        private class OutputData
        {
            public int Power { get; set; }
            public List<int> PrimeNFactors { get; set; }
        }
    }
}