﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SparqlForHumans.Benchmark.Models;
using SparqlForHumans.Utilities;

namespace SparqlForHumans.Benchmark
{
    public static class BenchmarkAnalysis
    {
        public static void DoTheAverageRecallF1Thing()
        {
            var filename = @"benchmark.json";
            var benchmarkResults = JsonSerialization.DeserializeJson<List<QueryBenchmark>>(filename);
            var byHashcode = benchmarkResults.GroupBy(x => x.GraphHashCode);

            var results = new List<string>();
            results.Add($"QueryHashcode,localCount,remoteCount,tp,fp,fn,precision,recall,f1");

            foreach (var benchmark in byHashcode)
            {
                if (benchmark.Count() != 2)
                    continue;
                var local = benchmark.FirstOrDefault(x => x.BenchmarkType.Equals("Local"))?.ResultsDictionary?.FirstOrDefault(x => x.Key.Equals("prop2"));
                var remote = benchmark.FirstOrDefault(x => x.BenchmarkType.Equals("Endpoint"))?.ResultsDictionary?.FirstOrDefault(x => x.Key.Equals("prop2"));

                if (local == null || remote == null || local.Value.Key == null || remote.Value.Key == null)
                    continue;

                var localResults = local.Value.Value;
                var remoteResults = remote.Value.Value;

                var remoteCount = remoteResults.Length;
                var localCount = localResults.Length;

                var TP = localResults.Intersect(remoteResults).Count();
                var FP = localResults.Except(remoteResults).Count();
                var FN = remoteResults.Except(localResults).Count();

                double precision = 1.0 * TP / (TP + FP);
                double recall = 1.0 * TP / (TP + FN);
                double f1 = 2.0 * (precision * recall) / (precision + recall);

                results.Add($"{benchmark.Key},{localCount},{remoteCount},{TP},{FP},{FN},{precision},{recall},{f1}");
            }
            File.WriteAllLines(@"metrics.csv", results);
        }
        public static void LogPointByPoint()
        {
            var filename = @"benchmark.json";
            var benchmarkResults = JsonSerialization.DeserializeJson<List<QueryBenchmark>>(filename);
            var byHashcode = benchmarkResults.GroupBy(x => x.GraphHashCode);
            var results = new List<string>();
            results.Add($"QueryHashcode,Local,Remote");
            foreach (var benchmark in byHashcode)
            {
                if (benchmark.Count() != 2)
                    continue;
                var local = benchmark.First(x => x.BenchmarkType.Equals("Local"));
                var remote = benchmark.First(x => x.BenchmarkType.Equals("Endpoint"));
                results.Add($"{benchmark.Key},{local.ElapsedTime},{remote.ElapsedTime}");
            }
            File.WriteAllLines(@"points.csv", results);
        }

        public static void LogBaseMetrics()
        {
            var filename = @"benchmark.json";
            var benchmarkResults = JsonSerialization.DeserializeJson<List<QueryBenchmark>>(filename);
            var localResults = benchmarkResults.Where(x => x.BenchmarkType.Equals("Local")).ToArray();
            var endpointResults = benchmarkResults.Where(x => x.BenchmarkType.Equals("Endpoint")).ToArray();

            var minLocal = localResults.Min(x => x.ElapsedTime);
            var maxLocal = localResults.Max(x => x.ElapsedTime);
            var countLocal = localResults.Count();
            var avgLocal = new TimeSpan(Convert.ToInt64(localResults.Average(x => x.ElapsedTime.Ticks)));
            var medianLocal = new TimeSpan(Convert.ToInt64(Median(localResults.Select(x => x.ElapsedTime.Ticks))));

            var minRemote = endpointResults.Min(x => x.ElapsedTime);
            var maxRemote = endpointResults.Max(x => x.ElapsedTime);
            var countRemote = endpointResults.Count();
            var avgRemote = new TimeSpan(Convert.ToInt64(endpointResults.Average(x => x.ElapsedTime.Ticks)));
            var medianRemote = new TimeSpan(Convert.ToInt64(Median(endpointResults.Select(x => x.ElapsedTime.Ticks))));

            var results = new List<string>();
            results.Add($"Local Count: {countLocal}");
            results.Add($"Local Min: {minLocal}");
            results.Add($"Local Max: {maxLocal}");
            results.Add($"Local Avg: {avgLocal}");
            results.Add($"Local Med: {medianLocal}");

            results.Add($"Remote Count: {countRemote}");
            results.Add($"Remote Min: {minRemote}");
            results.Add($"Remote Max: {maxRemote}");
            results.Add($"Remote Avg: {avgRemote}");
            results.Add($"Remote Med: {medianRemote}");

            File.WriteAllLines(@"results.txt", results);
        }

        public static T Median<T>(IEnumerable<T> items)
        {
            var i = (int)Math.Ceiling((double)(items.Count() - 1) / 2);
            if (i >= 0)
            {
                var values = items.ToList();
                values.Sort();
                return values[i];
            }

            return default(T);
        }
    }
}
