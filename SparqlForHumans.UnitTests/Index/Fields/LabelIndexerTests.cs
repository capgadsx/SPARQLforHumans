﻿using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Documents;
using SparqlForHumans.Lucene.Indexing.Fields;
using SparqlForHumans.Models.LuceneIndex;
using SparqlForHumans.RDF.Extensions;
using Xunit;

namespace SparqlForHumans.UnitTests.Index.Fields
{
    public class LabelIndexerTests
    {
        [Fact]
        public void TestGetField()
        {
            //Arrange
            var lines = new List<string>
            {
                "<http://www.wikidata.org/entity/Q27> <http://www.w3.org/2000/01/rdf-schema#label> \"Ireland\"@en .",
            };
            var subjectGroup = lines.GroupBySubject().FirstOrDefault();
            var expected = new TextField(Labels.Label.ToString(), "Ireland", Field.Store.YES);

            //Act
            var actual = new LabelIndexer().GetField(subjectGroup);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.FieldType, actual.FieldType);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.GetStringValue(), actual.GetStringValue());
        }
    }
}
