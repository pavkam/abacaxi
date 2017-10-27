# Abacaxi [![Build status](https://ci.appveyor.com/api/projects/status/ckq7nanjy3nms8a7?svg=true)](https://ci.appveyor.com/project/pavkam/abacaxi)

*"No code library is complete without a binary search!"*

I'm building this library as a repository of algorithms, data structures and helper methods that make one's daily programming life easier (in .NET that is).

Abacaxi is available on NuGet: https://www.nuget.org/packages/Abacaxi/ and is built against __.NET Framework 4.5.__

### The list of core data structures:
| Data structure | Description |
| :--- | :--- |
| `Heap` | Implements the *heap* data structure (also known as *priority queue*). Related material: <https://en.wikipedia.org/wiki/Heap_(data_structure)> |
| `BitSet` | Implements the standard *ISet&lt;int&gt;* data structure in an optimized form (using bit masks). Related material: <https://en.wikipedia.org/wiki/Bit_array> |
| `DisjointSet` | Also known as *union-find* or *merge-find* data structure. Related material: <https://en.wikipedia.org/wiki/Disjoint-set_data_structure> |
| `SingleLinkedNode` | Represents a node in a *singly-linked list*. All operations implemented by the node classes. Related material: <https://en.wikipedia.org/wiki/Linked_list> |
| `Trie` | A *trie* that implements the *IDictionary&lt;TKey, TValue&gt;* interface. Related material: <https://en.wikipedia.org/wiki/Trie> |
| `AvlTree` | The standard *AVL self-balancing tree*. Related material: <https://en.wikipedia.org/wiki/AVL_tree> |
| `BinarySearchTree` | The standard *binary search tree*. Related material: <https://en.wikipedia.org/wiki/Binary_search_tree> |
| `LeftLeaningRedBlackTree` | The simplified version of *Red/Black self-balancing tree*. Related material: <https://en.wikipedia.org/wiki/Left-leaning_red%E2%80%93black_tree> |
| `Graph` | A generic *(un)directional weighted graph* that implements many algorithms. Related material: <https://en.wikipedia.org/wiki/Graph> |
| `ChessHorsePathGraph` | A specialized graph implementation used to solve the *Knight's Tour* problem. Related material: <https://en.wikipedia.org/wiki/Knight%27s_tour> |
| `StringNeighborhoodGraph` | A specialized graph implementation used to solve the *Word Ladder* problem. Related material: <https://en.wikipedia.org/wiki/Word_ladder> |
| `MazeGraph` | A specialized base class for other graphs that use a two-dimensional integer board (think, a rat's maze). All standard graph algorithms can be applied to such a graph. |
| `LiteralGraph` | A specialized graph used mainly in testing the graph-related algorithms. |

### The list of helper/additional classes:
| Class | Description |
| :--- | :--- |
| `ArrayEqualityComparer` | Implements an equality comparer that is able to check two array for equality. The class is useful when using dictionaries/sets whose keys are arrays. |
| `Temporary` | A class used to store a value for a specific amount of time. The value expires and has to be reloaded. **Multi-threaded** |
| `BitWriter` | A specialized I/O class that implements the *Stream* base class. Allows for writing to a stream with bit granularity. |
| `GlobPattern` | Simple class that allows checking if a string matches a glob-like pattern (e.g. _"some*.?xt"_) |

### The list of implemented algorithms/helper methods:
| Algorithm/Method | Description |
| :--- | :--- |
| `SingleLinkedNode.FindMiddle` | Algorithm to *find the middle* node of a singly-linked list using one pass only. See related material: <https://en.wikibooks.org/wiki/Data_Structures/LinkedLists> |
| `SingleLinkedNode.VerifyIfKnotted` | Verifies if a singly-linked list *contains a knot (cycle)* using one pass only. See related material: <https://en.wikibooks.org/wiki/Data_Structures/LinkedLists>  |
| `SingleLinkedNode.Reverse` | *Reverses* a singly-linked list using the recursive algorithm. See related material: <https://en.wikibooks.org/wiki/Data_Structures/LinkedLists>  |
| `Graph.TraverseBfs` | Traverses the vertices in a graph using the *breadth-first search*. See related material: <https://en.wikipedia.org/wiki/Breadth-first_search> |
| `Graph.TraverseDfs` | Traverses the vertices in a graph using the *depth-first search*. See related material: <https://en.wikipedia.org/wiki/Depth-first_search> |
| `Graph.FillWithOneColor` | *Fills* all vertices of a graph with a given "color". See related material: <https://en.wikipedia.org/wiki/Flood_fill> |
| `Graph.FindShortestPath` | Find the *shortest path* between two graph vertices. See related material: <https://en.wikipedia.org/wiki/Shortest_path_problem>  |
| `Graph.GetComponents` | Finds all distinct *connected components* of a graph. The method returns each component represented as another graph. See related material: <https://en.wikipedia.org/wiki/Connected_component_(graph_theory)> |
| `Graph.TopologicalSort` | Implements the *topological sorting* algorithm. See related material: <https://en.wikipedia.org/wiki/Topological_sorting> |
| `Graph.FindAllArticulationVertices` | Finds all *articulation points* in a graph. See related material: <https://en.wikipedia.org/wiki/Biconnected_component> |
| `Graph.IsBipartite` | Checks if a graph is *bipartite*. See related material: <https://en.wikipedia.org/wiki/Bipartite_graph> |
| `Graph.DescribeVertices` | Returns a *description* of all vertices in a graph, including in-degree, out-degree and component index. |
| `Graph.FindCheapestPath` | Finds the *cheapest path* between two vertices in a graph. See related material: <https://en.wikipedia.org/wiki/A*_search_algorithm> |
| `FibonacciSequence.Enumerate` | Lists all *Fibonacci numbers* up to a given index in the series. See related material: <https://en.wikipedia.org/wiki/Fibonacci_number> |
| `FibonacciSequence.GetMember` | Returns the *Fibonacci number* at a given index in the series. See related material: <https://en.wikipedia.org/wiki/Fibonacci_number> |
| `Integer.DeconstructIntoPowersOfTwo` | *Deconstructs* an integer into a sum of powers of two. |
| `Integer.DeconstructIntoPrimeFactors` |  |
| `Integer.IsPrime` | Checks whether an integer is a *prime number*. |
| `Integer.Zip` |  |
| `Integer.Divide` |  |
| `IntegerPartitions.Enumerate` |  |
| `IntegerPartitions.GetCount` |  |
| `Knapsack.Fill` | The generic *0/1 knapsack* algorithm. See related material: <https://en.wikipedia.org/wiki/Knapsack_problem> |
| `Pairing.GetWithMinimumCost` |  |
| `Pairing.GetWithApproximateMinimumCost` |  |
| `ZArray.Construct` | Constructs a *Z-array* from a given input sequence. Z-arrays are useful for string pattern matching. See related materials: <http://wittawat.com/assets/talks/z_algorithm.pdf>, <https://shiv4289.wordpress.com/2013/09/17/z-algorithm-for-pattern-matching/> |
| `RandomExtensions.Sample` | A *random sampling* algorithm for a sequence of objects. See related material: <https://en.wikipedia.org/wiki/Reservoir_sampling> |
| `RandomExtensions.NextBool` | An extension method that allows retrieving a *random boolean* value. |
| `RandomExtensions.NextItem` | An extension method that allows retrieving a *random item* from a sequence of objects. |
| `SequenceExtensions. FindLongestIncreasingSequence` |  |
| `SequenceExtensions. ContainsTwoElementsThatAggregateTo` |  |
| `SequenceExtensions.FindDuplicates` | *Finds duplicates* in a sequence of objects. A specialized and optimized version for integer sequences also provided. |
| `SequenceExtensions.ExtractNestedBlocks` | An algorithm to allow *extracting nested* sub-sequences from a sequence (e.g. _"(a(b))"_ would return _"(b)"_ then _"(a(b))"_). |
| `SequenceExtensions. FindSubsequencesWithGivenAggregatedValue` |  |
| `SequenceExtensions.Interleave` | Creates a sequence which combines multiple *interleaved sequences* based on a given comparison. |
| `SequenceExtensions.Reverse` | *Reverses* a sequence in place. |
| `SequenceExtensions.Repeat` | Creates a sequence which is based on the original sequence *repeated a number of times*. |
| `SequenceExtensions.BinarySearch` | Implements the standard *binary search* algorithm. See related material: <https://en.wikipedia.org/wiki/Binary_search_algorithm> |
| `SequenceExtensions.Diff` | Implements the generic *edit distance* algorithm. See related material: <https://en.wikipedia.org/wiki/Edit_distance> |
| `SequenceExtensions. GetLongestCommonSubSequence` |  |
| `SequenceExtensions.GetItemFrequencies` |  |
| `SequenceExtensions.ToSet` | Helper method to *convert a given sequence into a set*. |
| `SequenceExtensions.AsList` | Helper method that *interprets a given sequence as a list*. If the sequence is already a list/array then the original object is returned; otherwise, the sequence is converted to an array. **This method may or may not create a new object and does not guarantee mutability of the result.** |
| `SequenceExtensions.AddOrUpdate` | Extends the dictionary classes with the ability to *add a new, or update an existing* key/pair. |
| `SequenceExtensions.Append` | A number of small utility methods used to *append items to arrays*. If the array is null, a new array is created. These methods return new arrays as their return values. |
| `SequenceExtensions.ToList` | Utility method that replaces a common _"Select(...).Tolist()"_ LINQ pattern. |
| `SequenceExtensions.Partition` | *Partitions* a given sequence into a batch of smaller partitions of a given size. |
| `SequenceExtensions.EmptyIfNull` | Method returns an *empty sequence* if the current sequence is null; otherwise it returns the sequence itself. |
| `SequenceExtensions.IsNullOrEmpty` | Mimics the **String.IsNullOrEmpty** method. |
| `SequenceExtensions.ToString` | Utility method that replaces a common _"String.Join(..., ...Select(...))"_ LINQ pattern. |
| `SequenceExtensions.Min` | Returns the element of a sequence with a given *selected minimum* (e.g. select the person object with the smallest age). |
| `SequenceExtensions.Max` | Returns the element of a sequence with a given *selected maximum* (e.g. select the person object with the greatest age). |
| `SequenceExtensions.Segment` | Returns a view of the original sequence bounded to a segment of the list. Useful when other methods do not allow specifying a start/length pair of arguments. |
| `Set.EnumerateSubsetCombinations` |  |
| `Set.SplitIntoSubsetsOfEqualValue` |  |
| `Set.GetSubsetWithNearValue` |  |
| `Set.ContainsSubsetWithExactValue` |  |
| `Set.GetSubsetWithGreatestValue` |  |
| `Sorting.BubbleSort` | Implements the standard *bubble sort* algorithm. See related material: <https://en.wikipedia.org/wiki/Bubble_sort> |
| `Sorting.CocktailShakerSort` | Implements the standard *cocktail shaker sort* algorithm. See related material: <https://en.wikipedia.org/wiki/Cocktail_shaker_sort> |
| `Sorting.CombSort` | Implements the standard *comb sort* algorithm. See related material: <https://en.wikipedia.org/wiki/Comb_sort> |
| `Sorting.GnomeSort` | Implements the standard *gnome sort* algorithm. See related material: <https://en.wikipedia.org/wiki/Gnome_sort> |
| `Sorting.HeapSort` | Implements the standard *heapsort* algorithm. See related material: <https://en.wikipedia.org/wiki/Heapsort> |
| `Sorting.InsertionSort` | Implements the standard *insertion sort* algorithm. See related material: <https://en.wikipedia.org/wiki/Insertion_sort> |
| `Sorting.MergeSort` | Implements the standard *merge sort* algorithm. See related material: <https://en.wikipedia.org/wiki/Merge_sort> |
| `Sorting.OddEvenSort` | Implements the standard *odd-even sort* algorithm. See related material: <https://en.wikipedia.org/wiki/Odd%E2%80%93even_sort> |
| `Sorting.QuickSort` | Implements the standard *quicksort* algorithm. See related material: <https://en.wikipedia.org/wiki/Quicksort> |
| `Sorting.ShellSort` | Implements the standard *shellsort* algorithm. See related material: <https://en.wikipedia.org/wiki/Shellsort> |
| `ObjectExtensions.IsAnyOf` | Helper methods that allows checking if an object is *equal to any other object* in a sequence (think of *_x IN (o1, o2, o3)*). |
| `ObjectExtensions.Inspect` | A simple helper method that allows *extracting fields/properties/methods* values from an object as a dictionary. |
| `ObjectExtensions.TryConvert` | Tries to *convert* a given object to a given type. Uses different techniques to achieve this goal. |
| `ObjectExtensions.As` | A simpler version of *TryConvert* that throws an exception if the conversion is not possible. |
| `StringExtensions.AsList` | Returns a *wrapper IList&lt;char&gt;* object. Useful when using other algorithms that expect a list. **The returned list is read-only for obvious reasons.** |
| `StringExtensions.Reverse` | *Reverses* a string and returns the reversed version. |
| `StringExtensions.Shorten` | *Shortens* a string to a given maximum length (considering Unicode surrogate-pairs, etc.). Allows for an optional "shortening indicator string" used at the end of the string (think *"This is a go..."*).  |
| `StringExtensions.Escape` | Escapes a string using the standard "C" escape sequences (e.g. _"\n"_ for new line). |
| `StringExtensions.Like` | A wrapper method on top of **GlobPattern** class. A convenient method to check if a *string matches a pattern*.  |
| `StringExtensions.FindDuplicates` | Finds *duplicate characters* in the string. Uses both a set and a small array for ASCII characters. |
| `StringExtensions.SplitIntoLines` | *Splits* a given string into its contituent lines. Treats both _"\n"_ and _"\r\n"_ as line breaks. |
| `StringExtensions.WordWrap` | *Word-wraps* a string to a given max line length. Uses white-spaces and puctuation characters as potential line breaks. |
| `StringExtensions.StripDiacritics` | *Strips the Unicode diacritics* from a text. Useful for text normalization in searches. |
