﻿using System.Collections.Generic;
using SparqlForHumans.Utilities;
namespace SparqlForHumans.Models.Query
{
    public class QueryNode : Node
    {
        public bool Traversed { get; set; } = false;
        public QueryNode(Node node)
        {
            this.id = node.id;
            this.name = node.name;
            this.uris = node.uris;
        }
        public QueryType QueryType {get;set; } = QueryType.Unkwown;
        public List<string> Results { get; set; } = new List<string>();
        public List<string> Types { get; set; } = new List<string>();
        public bool IsKnownType { get; set; } = false;
        public bool IsConnectedToKnownType { get; set; } = false;
        public override string ToString()
        {
            return $"{id}:{name} {(Types.Any() ? string.Join(";", Types.Select(x=>x.GetUriIdentifier())) : string.Empty)}: [HasP31:{IsKnownType}][edgeToP31:{IsConnectedToKnownType}] : {string.Join(",", Results)}";
        }
    }


}
