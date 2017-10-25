<a name='contents'></a>
# Contents [#](#contents 'Go To Here')

- [ChessHorsePathGraph](#T-Abacaxi-Practice-Graphs-ChessHorsePathGraph 'Abacaxi.Practice.Graphs.ChessHorsePathGraph')
  - [#ctor()](#M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-#ctor-System-Int32,System-Int32- 'Abacaxi.Practice.Graphs.ChessHorsePathGraph.#ctor(System.Int32,System.Int32)')
  - [IsDirected](#P-Abacaxi-Practice-Graphs-ChessHorsePathGraph-IsDirected 'Abacaxi.Practice.Graphs.ChessHorsePathGraph.IsDirected')
  - [IsReadOnly](#P-Abacaxi-Practice-Graphs-ChessHorsePathGraph-IsReadOnly 'Abacaxi.Practice.Graphs.ChessHorsePathGraph.IsReadOnly')
  - [SupportsPotentialWeightEvaluation](#P-Abacaxi-Practice-Graphs-ChessHorsePathGraph-SupportsPotentialWeightEvaluation 'Abacaxi.Practice.Graphs.ChessHorsePathGraph.SupportsPotentialWeightEvaluation')
  - [FindChessHorsePathBetweenTwoPoints(startCell,endCell)](#M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-FindChessHorsePathBetweenTwoPoints-Abacaxi-Graphs-Cell,Abacaxi-Graphs-Cell- 'Abacaxi.Practice.Graphs.ChessHorsePathGraph.FindChessHorsePathBetweenTwoPoints(Abacaxi.Graphs.Cell,Abacaxi.Graphs.Cell)')
  - [GetEdges(vertex)](#M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-GetEdges-Abacaxi-Graphs-Cell- 'Abacaxi.Practice.Graphs.ChessHorsePathGraph.GetEdges(Abacaxi.Graphs.Cell)')
  - [GetEnumerator()](#M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-GetEnumerator 'Abacaxi.Practice.Graphs.ChessHorsePathGraph.GetEnumerator')
  - [GetPotentialWeight(fromVertex,toVertex)](#M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-GetPotentialWeight-Abacaxi-Graphs-Cell,Abacaxi-Graphs-Cell- 'Abacaxi.Practice.Graphs.ChessHorsePathGraph.GetPotentialWeight(Abacaxi.Graphs.Cell,Abacaxi.Graphs.Cell)')
- [Integer](#T-Abacaxi-Practice-Integer 'Abacaxi.Practice.Integer')
  - [Divide(number,divisor)](#M-Abacaxi-Practice-Integer-Divide-System-Int32,System-Int32- 'Abacaxi.Practice.Integer.Divide(System.Int32,System.Int32)')
- [StringNeighborhoodGraph](#T-Abacaxi-Practice-Graphs-StringNeighborhoodGraph 'Abacaxi.Practice.Graphs.StringNeighborhoodGraph')
  - [#ctor(sequence)](#M-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-#ctor-System-Collections-Generic-IEnumerable{System-String}- 'Abacaxi.Practice.Graphs.StringNeighborhoodGraph.#ctor(System.Collections.Generic.IEnumerable{System.String})')
  - [IsDirected](#P-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-IsDirected 'Abacaxi.Practice.Graphs.StringNeighborhoodGraph.IsDirected')
  - [IsReadOnly](#P-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-IsReadOnly 'Abacaxi.Practice.Graphs.StringNeighborhoodGraph.IsReadOnly')
  - [SupportsPotentialWeightEvaluation](#P-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-SupportsPotentialWeightEvaluation 'Abacaxi.Practice.Graphs.StringNeighborhoodGraph.SupportsPotentialWeightEvaluation')
  - [GetEdges(vertex)](#M-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-GetEdges-System-String- 'Abacaxi.Practice.Graphs.StringNeighborhoodGraph.GetEdges(System.String)')
  - [GetEnumerator()](#M-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-GetEnumerator 'Abacaxi.Practice.Graphs.StringNeighborhoodGraph.GetEnumerator')
  - [GetPotentialWeight(fromVertex,toVertex)](#M-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-GetPotentialWeight-System-String,System-String- 'Abacaxi.Practice.Graphs.StringNeighborhoodGraph.GetPotentialWeight(System.String,System.String)')

<a name='assembly'></a>
# Abacaxi.Practice [#](#assembly 'Go To Here') [=](#contents 'Back To Contents')

<a name='T-Abacaxi-Practice-Graphs-ChessHorsePathGraph'></a>
## ChessHorsePathGraph [#](#T-Abacaxi-Practice-Graphs-ChessHorsePathGraph 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Abacaxi.Practice.Graphs

##### Summary

A chess-horse virtual graph. Each cell is connected to the cells that are reachable by a chess horse (L-shaped movements).

<a name='M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-#ctor-System-Int32,System-Int32-'></a>
### #ctor() `constructor` [#](#M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-#ctor-System-Int32,System-Int32- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Initializes a new instance of the [ChessHorsePathGraph](#T-Abacaxi-Practice-Graphs-ChessHorsePathGraph 'Abacaxi.Practice.Graphs.ChessHorsePathGraph') class.

##### Parameters

This constructor has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentOutOfRangeException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentOutOfRangeException 'System.ArgumentOutOfRangeException') | Thrown if `boardWidth` or `boardHeight` are less than `1`. |

<a name='P-Abacaxi-Practice-Graphs-ChessHorsePathGraph-IsDirected'></a>
### IsDirected `property` [#](#P-Abacaxi-Practice-Graphs-ChessHorsePathGraph-IsDirected 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets a value indicating whether this graph's edges are directed.

<a name='P-Abacaxi-Practice-Graphs-ChessHorsePathGraph-IsReadOnly'></a>
### IsReadOnly `property` [#](#P-Abacaxi-Practice-Graphs-ChessHorsePathGraph-IsReadOnly 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets a value indicating whether this instance is read only.

<a name='P-Abacaxi-Practice-Graphs-ChessHorsePathGraph-SupportsPotentialWeightEvaluation'></a>
### SupportsPotentialWeightEvaluation `property` [#](#P-Abacaxi-Practice-Graphs-ChessHorsePathGraph-SupportsPotentialWeightEvaluation 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets a value indicating whether this graph supports potential weight evaluation (heuristics).

##### Remarks

This implementation always returns `false`.

<a name='M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-FindChessHorsePathBetweenTwoPoints-Abacaxi-Graphs-Cell,Abacaxi-Graphs-Cell-'></a>
### FindChessHorsePathBetweenTwoPoints(startCell,endCell) `method` [#](#M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-FindChessHorsePathBetweenTwoPoints-Abacaxi-Graphs-Cell,Abacaxi-Graphs-Cell- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Finds the shortest path between any two arbitrary cells on an infinite chess board.

##### Returns

The shortest path between any two arbitrary cells in space.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| startCell | [Abacaxi.Graphs.Cell](#T-Abacaxi-Graphs-Cell 'Abacaxi.Graphs.Cell') | The start cell. |
| endCell | [Abacaxi.Graphs.Cell](#T-Abacaxi-Graphs-Cell 'Abacaxi.Graphs.Cell') | The end cell. |

<a name='M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-GetEdges-Abacaxi-Graphs-Cell-'></a>
### GetEdges(vertex) `method` [#](#M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-GetEdges-Abacaxi-Graphs-Cell- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets the edges for a given .

##### Returns

A sequence of edges connected to the given

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vertex | [Abacaxi.Graphs.Cell](#T-Abacaxi-Graphs-Cell 'Abacaxi.Graphs.Cell') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') | Thrown if the `vertex` is not part of the graph. |

<a name='M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-GetEnumerator'></a>
### GetEnumerator() `method` [#](#M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-GetEnumerator 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Returns an enumerator that iterates all vertices in the graph.

##### Returns

A [IEnumerator\`1](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerator`1 'System.Collections.Generic.IEnumerator`1') that can be used to iterate through the collection.

##### Parameters

This method has no parameters.

<a name='M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-GetPotentialWeight-Abacaxi-Graphs-Cell,Abacaxi-Graphs-Cell-'></a>
### GetPotentialWeight(fromVertex,toVertex) `method` [#](#M-Abacaxi-Practice-Graphs-ChessHorsePathGraph-GetPotentialWeight-Abacaxi-Graphs-Cell,Abacaxi-Graphs-Cell- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets the potential total weight connecting `fromVertex` and `toVertex` vertices.

##### Returns

The potential total cost.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fromVertex | [Abacaxi.Graphs.Cell](#T-Abacaxi-Graphs-Cell 'Abacaxi.Graphs.Cell') | The first vertex. |
| toVertex | [Abacaxi.Graphs.Cell](#T-Abacaxi-Graphs-Cell 'Abacaxi.Graphs.Cell') | The destination vertex. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.NotImplementedException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.NotImplementedException 'System.NotImplementedException') | Always thrown. |

##### Remarks

This graph does not support potential weight evaluation.

<a name='T-Abacaxi-Practice-Integer'></a>
## Integer [#](#T-Abacaxi-Practice-Integer 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Abacaxi.Practice

##### Summary

Class that only contains practice algorithms related to integers.

<a name='M-Abacaxi-Practice-Integer-Divide-System-Int32,System-Int32-'></a>
### Divide(number,divisor) `method` [#](#M-Abacaxi-Practice-Integer-Divide-System-Int32,System-Int32- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Divides `number` by `divisor`.

##### Returns

The result of division.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| number | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The number to divide. |
| divisor | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The divisor. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentOutOfRangeException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentOutOfRangeException 'System.ArgumentOutOfRangeException') | Thrown if `divisor` is `0`. |

<a name='T-Abacaxi-Practice-Graphs-StringNeighborhoodGraph'></a>
## StringNeighborhoodGraph [#](#T-Abacaxi-Practice-Graphs-StringNeighborhoodGraph 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Abacaxi.Practice.Graphs

##### Summary

A graph composed by a number of strings (representing vertices) and connected by edges signifying potential one-letter transformations.

<a name='M-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-#ctor-System-Collections-Generic-IEnumerable{System-String}-'></a>
### #ctor(sequence) `constructor` [#](#M-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-#ctor-System-Collections-Generic-IEnumerable{System-String}- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Initializes a new instance of the [StringNeighborhoodGraph](#T-Abacaxi-Practice-Graphs-StringNeighborhoodGraph 'Abacaxi.Practice.Graphs.StringNeighborhoodGraph') class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| sequence | [System.Collections.Generic.IEnumerable{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.String}') | The sequence of strings to build the graph upon. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | Thrown if `sequence` is `null`. |

<a name='P-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-IsDirected'></a>
### IsDirected `property` [#](#P-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-IsDirected 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets a value indicating whether this graph's edges are directed.

<a name='P-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-IsReadOnly'></a>
### IsReadOnly `property` [#](#P-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-IsReadOnly 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets a value indicating whether this instance is read only.

<a name='P-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-SupportsPotentialWeightEvaluation'></a>
### SupportsPotentialWeightEvaluation `property` [#](#P-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-SupportsPotentialWeightEvaluation 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets a value indicating whether this graph supports potential weight evaluation (heuristics).

##### Remarks

This implementation always returns `false`.

<a name='M-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-GetEdges-System-String-'></a>
### GetEdges(vertex) `method` [#](#M-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-GetEdges-System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets the edges for a given .

##### Returns

A sequence of edges connected to the given

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| vertex | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') | Thrown if the `vertex` is not part of the graph. |

<a name='M-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-GetEnumerator'></a>
### GetEnumerator() `method` [#](#M-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-GetEnumerator 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Returns an enumerator that iterates all vertices in the graph.

##### Returns

A [IEnumerator\`1](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerator`1 'System.Collections.Generic.IEnumerator`1') that can be used to iterate through the collection.

##### Parameters

This method has no parameters.

<a name='M-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-GetPotentialWeight-System-String,System-String-'></a>
### GetPotentialWeight(fromVertex,toVertex) `method` [#](#M-Abacaxi-Practice-Graphs-StringNeighborhoodGraph-GetPotentialWeight-System-String,System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets the potential total weight connecting `fromVertex` and `toVertex` vertices.

##### Returns

The potential total cost.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fromVertex | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The first vertex. |
| toVertex | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The destination vertex. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.NotImplementedException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.NotImplementedException 'System.NotImplementedException') | Always thrown. |

##### Remarks

This graph does not support potential weight evaluation.
