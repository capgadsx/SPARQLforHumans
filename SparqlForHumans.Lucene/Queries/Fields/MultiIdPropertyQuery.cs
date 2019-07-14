﻿using SparqlForHumans.Lucene.Queries.Base;
using SparqlForHumans.Lucene.Queries.Parsers;

namespace SparqlForHumans.Lucene.Queries
{
    public class MultiIdPropertyQuery : BasePropertyQuery
    {
        public MultiIdPropertyQuery(string luceneIndexPath, string searchString) : base(luceneIndexPath, searchString, 20) { }

        internal override IQueryParser QueryParser => new IdQueryParser();
        internal override bool IsInvalidSearchString(string inputString) => string.IsNullOrEmpty(inputString);
    }
}