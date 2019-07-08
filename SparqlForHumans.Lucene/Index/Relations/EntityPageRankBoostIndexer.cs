﻿using System.Collections.Generic;
using Lucene.Net.Documents;
using SparqlForHumans.Lucene.Index.Base;
using SparqlForHumans.Models.LuceneIndex;
using SparqlForHumans.RDF.Models;
using SparqlForHumans.Utilities;

namespace SparqlForHumans.Lucene.Index.Relations
{
    public class EntityPageRankBoostIndexer : BaseOneToOneRelationMapper<int, double>, IFieldIndexer<DoubleField>
    {
        public EntityPageRankBoostIndexer(string inputFilename) : base(inputFilename)
        {
        }
        public override string NotifyMessage { get; } = "Building <EntityId, Boost> Dictionary";

        internal override void ParseTripleGroup(Dictionary<int, double> dictionary, SubjectGroup subjectGroup)
        {
        }

        public override Dictionary<int, double> BuildIndex(IEnumerable<SubjectGroup> subjectGroups)
        {
            LogProgress(0);
            return EntityPageRank.BuildPageRank(subjectGroups);
        }

        public double Boost { get; set; }

        public string FieldName => Labels.Rank.ToString();

        public IReadOnlyList<DoubleField> GetField(SubjectGroup subjectGroup)
        {
            var subjectId = subjectGroup.Id.ToNumbers();
            return RelationIndex.ContainsKey(subjectId)
                ? new List<DoubleField> {new DoubleField(FieldName, RelationIndex[subjectId], Field.Store.YES)}
                : new List<DoubleField>();
        }
    }
}