# Abacaxi [![Build status](https://ci.appveyor.com/api/projects/status/ckq7nanjy3nms8a7?svg=true)](https://ci.appveyor.com/project/pavkam/abacaxi)

*"No code library is complete without a binary search!"*

I'm building this library as a repository of algorithms, data structures and helper methods that make one's daily programming life easier (in .NET that is).

Abacaxi is available on NuGet: https://www.nuget.org/packages/Abacaxi/ and is built against __.NET Framework 4.5.__

### The list of core data structures:
| Data structure | Description |
| --- |:---:|
| **Heap** | Implements the *heap* data structure (also known as *priority queue*). Related material: <https://en.wikipedia.org/wiki/Heap_(data_structure)> |
| **BitSet** | Implements the standard *ISet&lt;int&gt;* data structure in an optimized form (using bit masks). Related material: <https://en.wikipedia.org/wiki/Bit_array> |
| **DisjointSet** | Also known as *union-find* or *merge-find* data structure. Related material: <https://en.wikipedia.org/wiki/Disjoint-set_data_structure> |
| **SingleLinkedNode** | Represents a node in a *single-linked list*. All operations implemented by the node classes. Related material: <https://en.wikipedia.org/wiki/Linked_list> |
| **Trie** | A *trie* that implements the *IDictionary&lt;TKey, TValue&gt;* interface. Related material: <https://en.wikipedia.org/wiki/Trie> |
| **AvlTree** | The standard *AVL self-balancing tree*. Related material: <https://en.wikipedia.org/wiki/AVL_tree> |
| **BinarySearchTree** | The standard *binary search tree*. Related material: <https://en.wikipedia.org/wiki/Binary_search_tree> |
| **LeftLeaningRedBlackTree** | The simplified version of *Red/Black self-balancing tree*. Related material: <https://en.wikipedia.org/wiki/Left-leaning_red%E2%80%93black_tree> |
| **Graph** | A generic *(un)directional weighted graph* that implements a number of algorithms. Related material: <https://en.wikipedia.org/wiki/Graph> |
| **ChessHorsePathGraph** | A specialized graph implementation used to solve the *Knight's Tour* problem. Related material: <https://en.wikipedia.org/wiki/Knight%27s_tour> |
| **StringNeighborhoodGraph** | A specialized graph implementation used to solve the *Word Ladder* problem. Related material: <https://en.wikipedia.org/wiki/Word_ladder> |
| **MazeGraph** | A specialized base class for other graphs that use a two-dimensional integer board (think, a rat's maze). All standard graph algorithms can be applied to such a graph. |
| **LiteralGraph** | A specialized graph used mainly in testing the graph-related algorithms. |

### The list of helper/additional classes:
| Class | Description |
| --- |:---:|
| **ArrayEqualityComparer** | Implements an equality comparer that is able to check two array for equality. The class is useful when using dictionaries/sets whose keys are arrays. |
| **Temporary** | A class used to store a value for a specific amount of time. The value expires and has to be reloaded. **Multi-threaded** |
| **BitWriter** | A specialized I/O class that implements the *Stream* base class. Allows for writing to a stream with bit granularity. |
| **GlobPattern** | Simple class that allows checking if a string matches a glob-like pattern (e.g. _"some*.?xt"_) |

### The list of implemented algorithms/helper methods:
| Algorithm/Method | Description |
| --- |:---:|
| SingleLinkedNode.FindMiddle |  |
| SingleLinkedNode.VerifyIfKnotted |  |
| SingleLinkedNode.Reverse |  |
| Graph.TraverseBfs |  |
| Graph.TraverseDfs |  |
| Graph.FillWithOneColor |  |
| Graph.FindShortestPath |  |
| Graph.GetComponents |  |
| Graph.TopologicalSort |  |
| Graph.FindAllArticulationVertices |  |
| Graph.IsBipartite |  |
| Graph.DescribeVertices |  |
| Graph.FindCheapestPath |  |
| Graph.Enumerate |  |
| FibonacciSequence.Enumerate |  |
| FibonacciSequence.GetMember |  |
| Integer.DeconstructIntoPowersOfTwo |  |
| Integer.DeconstructIntoPrimeFactors |  |
| Integer.IsPrime |  |
| Integer.Zip |  |
| Integer.Divide |  |
| IntegerPartitions.Enumerate |  |
| IntegerPartitions.GetCount |  |
| Knapsack.Fill |  |
| Pairing.GetWithMinimumCost |  |
| Pairing.GetWithApproximateMinimumCost |  |
| RandomExtensions.Sample |  |
| RandomExtensions.NextBool |  |
| RandomExtensions.NextItem |  |
| SequenceExtensions.FindLongestIncreasingSequence |  |
| SequenceExtensions.ContainsTwoElementsThatAggregateTo |  |
| SequenceExtensions.FindDuplicates |  |
| SequenceExtensions.ExtractNestedBlocks |  |
| SequenceExtensions.FindSubsequencesWithGivenAggregatedValue |  |
| SequenceExtensions.Interleave |  |
| SequenceExtensions.Reverse |  |
| SequenceExtensions.Repeat |  |
| SequenceExtensions.BinarySearch |  |
| SequenceExtensions.Diff |  |
| SequenceExtensions.GetLongestCommonSubSequence |  |
| SequenceExtensions.GetItemFrequencies |  |
| SequenceExtensions.ToSet |  |
| SequenceExtensions.AsList |  |
| SequenceExtensions.AddOrUpdate |  |
| SequenceExtensions.Append |  |
| SequenceExtensions.ToList |  |
| SequenceExtensions.Partition |  |
| SequenceExtensions.EmptyIfNull |  |
| SequenceExtensions.ToString |  |
| SequenceExtensions.Min |  |
| SequenceExtensions.Max |  |
| Set.EnumerateSubsetCombinations |  |
| Set.SplitIntoSubsetsOfEqualValue |  |
| Set.GetSubsetWithNearValue |  |
| Set.ContainsSubsetWithExactValue |  |
| Set.GetSubsetWithGreatestValue |  |
| Sorting.BubbleSort |  |
| Sorting.CocktailShakerSort |  |
| Sorting.CombSort |  |
| Sorting.GnomeSort |  |
| Sorting.HeapSort |  |
| Sorting.InsertionSort |  |
| Sorting.MergeSort |  |
| Sorting.OddEvenSort |  |
| Sorting.QuickSort |  |
| Sorting.ShellSort |  |
| ObjectExtensions.IsAnyOf |  |
| ObjectExtensions.Inspect |  |
| ObjectExtensions.TryConvert |  |
| ObjectExtensions.As |  |
| StringExtensions.AsList |  |
| StringExtensions.Reverse |  |
| StringExtensions.Shorten |  |
| StringExtensions.Escape |  |
| StringExtensions.Like |  |
| StringExtensions.FindDuplicates |  |
| StringExtensions.SplitIntoLines |  |
| StringExtensions.WordWrap |  |
| StringExtensions.StripDiacritics |  |
