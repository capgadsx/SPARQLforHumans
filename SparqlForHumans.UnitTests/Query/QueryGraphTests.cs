﻿using SparqlForHumans.Models.Query;
using Xunit;

namespace SparqlForHumans.UnitTests.Query
{
    public class QueryGraphTests
    {
        [Fact]
        public void TestInferScenario1_1Node0Edge()
        {
            var graph = new RDFExplorerGraph()
            {
                nodes = new[]
                {
                    new Node()
                    {
                        id = 0,
                        name = "?var0",
                        uris = new string[0]
                    },
                    
                },
            };
            var queryGraph = new QueryGraph(graph);
            Assert.Equal(GraphQueryType.QueryTopEntities, queryGraph.Nodes[0].QueryType);
        }

        [Fact]
        public void TestInferScenario1_2Nodes0Edge()
        {
            var graph = new RDFExplorerGraph()
            {
                nodes = new[]
                {
                    new Node()
                    {
                        id = 0,
                        name = "?var0",
                        uris = new string[0]
                    },
                    new Node()
                    {
                        id = 1,
                        name = "?var1",
                        uris = new string[0]
                    },
                },
            };
            var queryGraph = new QueryGraph(graph);
            Assert.Equal(GraphQueryType.QueryTopEntities, queryGraph.Nodes[0].QueryType);
            Assert.Equal(GraphQueryType.QueryTopEntities, queryGraph.Nodes[1].QueryType);
        }

        [Fact]
        public void TestInferScenario1_2Nodes1Edge()
        {
            var graph = new RDFExplorerGraph()
            {
                nodes = new[]
                {
                    new Node()
                    {
                        id = 0,
                        name = "?var0",
                        uris = new string[0]
                    },
                    new Node()
                    {
                        id = 1,
                        name = "?var1",
                        uris = new string[0]
                    },
                },
                edges = new[]
                {
                    new Edge()
                    {
                        id = 0,
                        name = "?prop0",
                        sourceId = 0,
                        targetId = 1,
                        uris = new string[0]
                    }
                },
            };
            var queryGraph = new QueryGraph(graph);
            Assert.Equal(GraphQueryType.QueryTopEntities, queryGraph.Nodes[0].QueryType);
            Assert.Equal(GraphQueryType.QueryTopEntities, queryGraph.Nodes[1].QueryType);
            Assert.Equal(GraphQueryType.QueryTopProperties, queryGraph.Edges[0].QueryType);
        }

        [Fact]
        public void TestInferScenario2_3Nodes2Edge()
        {
            var graph = new RDFExplorerGraph()
            {
                nodes = new[]
                {
                    new Node()
                    {
                        id = 0,
                        name = "?var0",
                        uris = new string[0]
                    },
                    new Node()
                    {
                        id = 1,
                        name = "?var1",
                        uris = new string[0]
                    },
                    new Node()
                    {
                        id = 2,
                        name = "?var2",
                        uris = new string[]{"http://www.wikidata.org/entity/Q5"}
                    },
                },
                edges = new[]
                {
                    new Edge()
                    {
                        id = 0,
                        name = "?prop0",
                        sourceId = 0,
                        targetId = 1,
                        uris = new string[0]
                    },
                    new Edge()
                    {
                        id = 1,
                        name = "?prop1",
                        sourceId = 0,
                        targetId = 2,
                        uris = new string[]{"http://www.wikidata.org/prop/direct/P31"}
                    }
                },
            };
            var queryGraph = new QueryGraph(graph);

            // Node 0 is type Q5. 
            // Results should be something like: I know the type of this guy, should return items of type Q5 (Use Wikidata)
            Assert.Equal(GraphQueryType.KnownNodeTypeQueryInstanceEntities, queryGraph.Nodes[0].QueryType);

            // Q1 should be something like: I don't know anything about this type.
            // TODO: But actually, I do know somwthing about this node: I know that I have properties in the graph that come from Q5. This node is in the range of Q5.
            // Not implemented yet.
            Assert.Equal(GraphQueryType.QueryTopEntities, queryGraph.Nodes[1].QueryType);

            // Constant, should not have results.
            Assert.Equal(GraphQueryType.ConstantTypeDoNotQuery, queryGraph.Nodes[2].QueryType);

            // Edge source is Known. Results should be Domain of the node type (Use Endpoint)
            Assert.Equal(GraphQueryType.KnownSubjectTypeOnlyQueryDomainProperties, queryGraph.Edges[0].QueryType);

            // Constant, should not have results.
            Assert.Equal(GraphQueryType.ConstantTypeDoNotQuery, queryGraph.Edges[1].QueryType);
        }

        [Fact]
        public void TestInferScenario3_3Nodes2Edge()
        {
            var graph = new RDFExplorerGraph()
            {
                nodes = new[]
                {
                    new Node()
                    {
                        id = 0,
                        name = "?var0",
                        uris = new string[0]
                    },
                    new Node()
                    {
                        id = 1,
                        name = "?var1",
                        uris = new string[0]
                    },
                    new Node()
                    {
                        id = 2,
                        name = "?var2",
                        uris = new string[]{"http://www.wikidata.org/entity/Q5"}
                    },
                },
                edges = new[]
                {
                    new Edge()
                    {
                        id = 0,
                        name = "?prop0",
                        sourceId = 1,
                        targetId = 0,
                        uris = new string[0]
                    },
                    new Edge()
                    {
                        id = 1,
                        name = "?prop1",
                        sourceId = 0,
                        targetId = 2,
                        uris = new string[]{"http://www.wikidata.org/prop/direct/P31"}
                    }
                },
            };
            var queryGraph = new QueryGraph(graph);

            // Node 0 is type Q5. 
            // Results should be something like: I know the type of this guy, should return items of type Q5 (Use Wikidata)
            Assert.Equal(GraphQueryType.KnownNodeTypeQueryInstanceEntities, queryGraph.Nodes[0].QueryType);

            // Q1 should be something like: I don't know anything about this type.
            // TODO: But actually, I do know somwthing about this node: I know that I have properties in the graph that come from Q5. This node is in the range of Q5.
            // Not implemented yet.
            Assert.Equal(GraphQueryType.KnownDomainTypeNotUsed, queryGraph.Nodes[1].QueryType);

            // Constant, should not have results.
            Assert.Equal(GraphQueryType.ConstantTypeDoNotQuery, queryGraph.Nodes[2].QueryType);

            // Edge source is Known. Results should be Domain of the node type (Use Endpoint)
            Assert.Equal(GraphQueryType.KnownObjectTypeOnlyQueryRangeProperties, queryGraph.Edges[0].QueryType);
            
            // Constant, should not have results.
            Assert.Equal(GraphQueryType.ConstantTypeDoNotQuery, queryGraph.Edges[1].QueryType);
        }

        [Fact]
        public void TestInferScenario4_4Nodes3Edge()
        {
            var graph = new RDFExplorerGraph()
            {
                nodes = new[]
                {
                    new Node()
                    {
                        id = 0,
                        name = "?varHuman",
                        uris = new string[0]
                    },
                    new Node()
                    {
                        id = 1,
                        name = "?varCity",
                        uris = new string[0]
                    },
                    new Node()
                    {
                        id = 2,
                        name = "human",
                        uris = new string[]{"http://www.wikidata.org/entity/Q5"}
                    },
                    new Node()
                    {
                        id = 3,
                        name = "city",
                        uris = new string[]{"http://www.wikidata.org/entity/Q515"}
                    },
                },
                edges = new[]
                {
                    new Edge()
                    {
                        id = 0,
                        name = "?prop0",
                        sourceId = 0,
                        targetId = 1,
                        uris = new string[0]
                    },
                    new Edge()
                    {
                        id = 1,
                        name = "?type1",
                        sourceId = 0,
                        targetId = 2,
                        uris = new string[]{"http://www.wikidata.org/prop/direct/P31"}
                    },
                    new Edge()
                    {
                        id = 2,
                        name = "?type2",
                        sourceId = 1,
                        targetId = 3,
                        uris = new string[]{"http://www.wikidata.org/prop/direct/P31"}
                    }
                },
            };
            var queryGraph = new QueryGraph(graph);

            // Node 0 is type Q5. 
            // Results should be something like: I know the type of this guy, should return items of type Q5 (Use Wikidata)
            Assert.Equal(GraphQueryType.KnownNodeAndDomainTypesNotUsed, queryGraph.Nodes[0].QueryType);

            // Q1 should be something like: I don't know anything about this type.
            // TODO: But actually, I do know somwthing about this node: I know that I have properties in the graph that come from Q5. This node is in the range of Q5.
            // Not implemented yet.
            Assert.Equal(GraphQueryType.KnownNodeTypeQueryInstanceEntities, queryGraph.Nodes[1].QueryType);

            // Constant, should not have results.
            Assert.Equal(GraphQueryType.ConstantTypeDoNotQuery, queryGraph.Nodes[2].QueryType);
            Assert.Equal(GraphQueryType.ConstantTypeDoNotQuery, queryGraph.Nodes[3].QueryType);

            // Edge source is Known. Results should be Domain of the node type (Use Endpoint)
            Assert.Equal(GraphQueryType.KnownSubjectAndObjectTypesIntersectDomainRangeProperties, queryGraph.Edges[0].QueryType);
            
            // Constant, should not have results.
            Assert.Equal(GraphQueryType.ConstantTypeDoNotQuery, queryGraph.Edges[1].QueryType);
            Assert.Equal(GraphQueryType.ConstantTypeDoNotQuery, queryGraph.Edges[2].QueryType);
        }
    }
}
