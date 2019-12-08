﻿using System;
using System.Collections.Generic;
using System.Linq;
using SparqlForHumans.Models.RDFExplorer;

namespace SparqlForHumans.Lucene.Queries.Graph
{
    public class QueryGraph
    {
        public QueryGraph(RDFExplorerGraph rdfGraph)
        {
            Nodes = rdfGraph.nodes.ToDictionary(x => x.id, x => new QueryNode(x));
            Edges = rdfGraph.edges.ToDictionary(x => x.id, x => new QueryEdge(x));

            this.CheckNodeTypes();

            Selected = rdfGraph.selected;

        }

        public void SetIndexPaths(string entitiesIndexPath, string propertiesIndexPath)
        {
            EntitiesIndexPath = entitiesIndexPath;
            PropertiesIndexPath = propertiesIndexPath;
        }

        public string EntitiesIndexPath { get; set; }
        public string PropertiesIndexPath { get; set; }

        public Dictionary<int, QueryNode> Nodes { get; set; }
        public Dictionary<int, QueryEdge> Edges { get; set; }
        public Selected Selected { get; set; }

        public List<string> Results
        {
            get
            {
                return Selected.isNode
                    ? Nodes.FirstOrDefault(x => x.Key.Equals(Selected.id)).Value.Results.Select(x => $"{x.Id}#{x.Label}").ToList()
                    : Edges.FirstOrDefault(x => x.Key.Equals(Selected.id)).Value.Results.Select(x => $"{x.Id}#{x.Label}").ToList();
            }
        }

        public override string ToString()
        {
            var nodesString = $"{{ Nodes: {{{string.Join("; ", Nodes.Select(x=>x.ToString()))}}} }}";
            var edgesString = $"{{ Edges: {{{string.Join("; ", Edges.Select(x=>x.ToString()))}}} }}";
            return $"{nodesString} {edgesString}";
        }
    }
}
