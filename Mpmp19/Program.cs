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
            
            
            
            for (int i = 2; i < 1000; i++)
            {
             var isFactor = IsSquarePrimeSumFactor(i);
             if (isFactor)
             {
                 primeNFactors.Add(i);
             }
            }

            foreach (var n in primeNFactors)
            {
                Console.WriteLine(n);
            }
            
        }

        private static bool IsSquarePrimeSumFactor(int n)
        {
            var primeList = GeneratePrimes(n);

            double psSum = 0;
            foreach (var p in primeList)
            {
                var ps = Math.Pow(Convert.ToDouble(p), 2);
                psSum += ps;
            }
            

            var psSumDiv = psSum / n;
            var remainder = psSum % n;
            bool isFactor = remainder == 0;
            
            return isFactor;
        }

        private static ArrayList GeneratePrimes(int toGenerate)
        {
            var sw = new Stopwatch();
            sw.Start();
            ArrayList primes = new ArrayList();
            primes.Add(2);
            primes.Add(3);
            while (primes.Count < toGenerate)
            {
                int nextPrime = (int)(primes[primes.Count - 1]) + 2;
                while (true)
                {
                    bool isPrime = true;
                    foreach (int n in primes)
                    {
                        if (nextPrime % n == 0)
                        {
                            isPrime = false;
                            break;
                        }
                    }
                    if (isPrime)
                    {
                        break;
                    }
                    else
                    {
                        nextPrime += 2;
                    }
                }
                primes.Add(nextPrime);
            }
            sw.Stop();
            Console.WriteLine("Generate primes " + sw.ElapsedMilliseconds + " ms");
            return primes;
        }
    }
}