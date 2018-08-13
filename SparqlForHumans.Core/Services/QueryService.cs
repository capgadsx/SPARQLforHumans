﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SparqlForHumans.Core.Models;
using SparqlForHumans.Core.Properties;
using SparqlForHumans.Core.Utilities;
using Version = Lucene.Net.Util.Version;

namespace SparqlForHumans.Core.Services
{
    public static class QueryService
    {
        /// <summary>
        /// Uses the default Lucene Index to query.
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public static IEnumerable<Entity> QueryEntitiesByLabel(string searchText)
        {
            return QueryEntitiesByLabel(searchText, LuceneIndexExtensions.LuceneIndexDirectory);
        }

        public static Entity QueryEntityByLabel(string searchText, Directory luceneIndexDirectory)
        {
            if (string.IsNullOrEmpty(searchText))
                return null;

            searchText = PrepareSearchTerm(searchText);

            // NotEmpty Validation
            if (string.IsNullOrEmpty(searchText.Replace("*", "").Replace("?", "")))
                return null;

            var entity = new Entity();

            using (var searcher = new IndexSearcher(luceneIndexDirectory, true))
            using (var queryAnalyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                entity = searchEntity(searchText, queryAnalyzer, searcher);

                queryAnalyzer.Close();
                searcher.Dispose();
            }
            return entity;
        }

        public static IEnumerable<Entity> QueryEntitiesByLabel(string searchText, Directory luceneIndexDirectory)
        {
            if (string.IsNullOrEmpty(searchText))
                return new List<Entity>();

            searchText = PrepareSearchTerm(searchText);
            const int resultsLimit = 20;

            var list = new List<Entity>();

            // NotEmpty Validation
            if (string.IsNullOrEmpty(searchText.Replace("*", "").Replace("?", "")))
                return list;

            using (var searcher = new IndexSearcher(luceneIndexDirectory, true))
            using (var queryAnalyzer = new StandardAnalyzer(Version.LUCENE_30))
            {
                list = searchEntities(searchText, queryAnalyzer, searcher, resultsLimit);

                queryAnalyzer.Close();
                searcher.Dispose();
            }
            return list;
        }

        private static Entity searchEntity(string searchText, Analyzer queryAnalyzer, Searcher searcher)
        {
            QueryParser parser = new MultiFieldQueryParser(Version.LUCENE_30,
                new[] { Labels.Id.ToString(), Labels.Label.ToString(), Labels.AltLabel.ToString() },
                queryAnalyzer);

            return searchEntity(searchText, searcher, parser);
        }

        private static List<Entity> searchEntities(string searchText, Analyzer queryAnalyzer, Searcher searcher,
            int resultsLimit)
        {
            QueryParser parser = new MultiFieldQueryParser(Version.LUCENE_30,
                new[] { Labels.Id.ToString(), Labels.Label.ToString(), Labels.AltLabel.ToString() },
                queryAnalyzer);

            return searchEntities(searchText, searcher, resultsLimit, parser);
        }

        //TODO: Create Entity Interface
        //TODO: Refactor MapEntity to be a class object that can be exchanged to create different mappings.
        private static Entity searchEntity(string searchText, Searcher searcher, QueryParser parser)
        {
            var query = ParseQuery(searchText, parser);
            var hit = searcher.Search(query, null, 1).ScoreDocs;

            if (hit == null || hit.Length.Equals(0))
                return null;

            return searcher.Doc(hit.FirstOrDefault().Doc).MapEntity();
        }

        private static List<Entity> searchEntities(string searchText, Searcher searcher, int resultsLimit, QueryParser parser)
        {
            var query = ParseQuery(searchText, parser);
            var hits = searcher.Search(query, null, resultsLimit).ScoreDocs;

            var entityList = new List<Entity>();
            foreach (var hit in hits)
            {
                var doc = searcher.Doc(hit.Doc);
                entityList.Add(doc.MapEntity());
            }

            return entityList;
        }

        public static string PrepareSearchTerm(string input)
        {
            var terms = input.Trim()
                .Replace("-", " ")
                .Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.Trim() + "*");

            return string.Join(" ", terms);
        }

        private static Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }

            return query;
        }
    }
}