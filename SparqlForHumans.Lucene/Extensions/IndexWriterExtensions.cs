﻿using System.Linq;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using SparqlForHumans.Models.LuceneIndex;
using SparqlForHumans.Models.RDFIndex;

namespace SparqlForHumans.Lucene.Extensions
{
    public static class IndexWriterExtensions
    {
        public static void AddEntityDocument(this IndexWriter writer, RDFIndexEntity entity)
        {
            var document = new Document();

            //OK
            if (entity.InstanceOf.Count > 0)
                document.Add(new StringField(Labels.InstanceOf.ToString(),
                    string.Join(" ## ", entity.InstanceOf.Distinct()), Field.Store.YES));

            //OK
            if (entity.SubClass.Count > 0)
                document.Add(new StringField(Labels.SubClass.ToString(),
                    string.Join(" ## ", entity.SubClass.Distinct()), Field.Store.YES));

            //OK
            if (entity.Properties.Count > 0)
                document.Add(new StringField(Labels.Property.ToString(),
                    string.Join(" ## ", entity.Properties.Distinct()), Field.Store.YES));

            if (entity.IsType)
                document.Add(new StringField(Labels.IsTypeEntity.ToString(), entity.IsType.ToString(),
                    Field.Store.YES));

            //OK
            if (!string.IsNullOrWhiteSpace(entity.Label))
                document.Add(new TextField(Labels.Label.ToString(), entity.Label, Field.Store.YES)
                { Boost = (float)entity.Rank });

            //OK
            if (entity.AltLabels.Count > 0)
                document.Add(
                    new TextField(Labels.AltLabel.ToString(), string.Join(" ## ", entity.AltLabels), Field.Store.YES)
                    { Boost = (float)entity.Rank });

            //OK
            if (!string.IsNullOrWhiteSpace(entity.Description))
                document.Add(new TextField(Labels.Description.ToString(), entity.Description, Field.Store.YES));

            if (entity.Rank > 0)
                document.Add(new DoubleField(Labels.Rank.ToString(), entity.Rank, Field.Store.YES));

            //OK
            document.Add(new StringField(Labels.Id.ToString(), entity.Id, Field.Store.YES));

            writer.AddDocument(document);
        }
    }
}