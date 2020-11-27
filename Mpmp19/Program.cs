using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mpmp19
{
    internal class Program
    {
        private class OutputData
        {
            public int Power { get; set; }
            public List<int> PrimeNFactors { get; set; }
            
        }
        public static void Main(string[] args)
        {
            try
            {

                var outputs = new List<OutputData>();

                var maxPower = 20;
                var maxN = 10000;
                var primeList = GeneratePrimes(maxN);
                double primePowSum = 0;

            
                for (int power = 2; power <= maxPower; power++)
                {
                    var primeNFactors = FindPrimeNFactors(maxN, primePowSum, primeList,power);

                    var outputData = new OutputData();
                    outputData.Power = power;
                    outputData.PrimeNFactors = primeNFactors;
                    outputs.Add(outputData);
                
                }


                var headerLine = "";
                foreach (var o in outputs)
                {
                    headerLine = headerLine + "\t" + o.Power;
                }
                Console.WriteLine(headerLine);

                var maxRow = 0;
                foreach (var o in outputs)
                {
                    if (o.PrimeNFactors.Count > maxRow)
                    {
                        maxRow = o.PrimeNFactors.Count;
                    }
                }

                for (int r = 0; r < maxRow; r++)
                {
                    Console.Write(r + "\t");
                    foreach (var o in outputs)
                    {
                        var num = "";
                        if (o.PrimeNFactors.Count > r)
                        {
                            num = o.PrimeNFactors[r].ToString();
                        }
                        Console.Write(num + "\t");
                    }
                
                    Console.WriteLine();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        private static List<int> FindPrimeNFactors(int maxN, double primePowSum, ArrayList primeList, int power)
        {
            var primeNFactors = new List<int>();

            for (var n = 1; n < maxN; n++)
            {
                var p = primeList[n-1];
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
            var p = primeList[n-1];
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
    }
}