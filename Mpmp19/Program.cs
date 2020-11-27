using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mpmp19
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var primeNFactors = new List<int>();

            var maxN = 1000;
            var primeList = GeneratePrimes(maxN);

            for (var i = 2; i < 10000; i++)
            {
                var isFactor = IsSquarePrimeSumFactor(primeList, i);
                if (isFactor)
                {
                    primeNFactors.Add(i);
                }
            }

            foreach (var n in primeNFactors) Console.WriteLine(n);
        }

        private static bool IsSquarePrimeSumFactor(ArrayList primeList, int n)
        {
            double psSum = 0;
            for (var i = 0; i < n; i++)
            {
                var p = primeList[i];
                var ps = Math.Pow(Convert.ToDouble(p), 2);
                psSum += ps;
            }

            var psSumDiv = psSum / n;
            var remainder = psSum % n;
            var isFactor = remainder == 0;

            return isFactor;
        }

        private static ArrayList GeneratePrimes(int toGenerate)
        {
            var sw = new Stopwatch();
            sw.Start();
            var primes = new ArrayList();
            primes.Add(2);
            primes.Add(3);
            while (primes.Count < toGenerate)
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
    }
}