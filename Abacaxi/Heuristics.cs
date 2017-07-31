/* Copyright 2017 by Alexandru Ciobanu (alex+git@ciobanu.org)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation 
 * files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
 * is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

namespace Abacaxi
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Internal;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines a set of algorithms useful during heuristical search for solutions.
    /// </summary>
    [PublicAPI]
    internal static class Heuristics
    {
        /// <summary>
        /// Class that holds the required properties for <see cref="Heuristics.ApplySimulatedAnnealing{TInput,TSolution}"/> method.
        /// </summary>
        public class SimulatedAnnealingParams
        {
            /// <summary>
            /// Gets number the cooling steps.
            /// </summary>
            /// <value>
            /// The number of cooling steps.
            /// </value>
            public int CoolingSteps { get; }

            /// <summary>
            /// Gets the number of iterations per cooling step.
            /// </summary>
            /// <value>
            /// The number of iterations per cooling step.
            /// </value>
            public int IterationsPerCoolingStep { get; }

            /// <summary>
            /// Gets the initial temperature.
            /// </summary>
            /// <value>
            /// The initial temperature.
            /// </value>
            public double InitialTemperature { get; }

            /// <summary>
            /// Gets the cooling alpha constant.
            /// </summary>
            /// <value>
            /// The cooling alpha constant.
            /// </value>
            public double CoolingAlpha { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="SimulatedAnnealingParams"/> class.
            /// </summary>
            /// <param name="coolingSteps">The number cooling steps.</param>
            /// <param name="iterationsPerCoolingStep">The number of iterations per cooling step.</param>
            /// <param name="coolingAlpha">The cooling alpha constant.</param>
            /// <param name="initialTemperature">The initial temperature.</param>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if any of the provided values are out of allowed range.</exception>
            public SimulatedAnnealingParams(
                int coolingSteps = 100,
                int iterationsPerCoolingStep = 100,
                double coolingAlpha = 0.99,
                double initialTemperature = 1)
            {
                Validate.ArgumentGreaterThanZero(nameof(coolingSteps), coolingSteps);
                Validate.ArgumentGreaterThanZero(nameof(iterationsPerCoolingStep), iterationsPerCoolingStep);
                Validate.ArgumentGreaterThanZero(nameof(coolingAlpha), coolingAlpha);
                Validate.ArgumentLessThan(nameof(coolingAlpha), coolingAlpha, 1);
                Validate.ArgumentGreaterThanZero(nameof(initialTemperature), initialTemperature);
                Validate.ArgumentLessThanOrEqualTo(nameof(initialTemperature), initialTemperature, 1);

                CoolingSteps = coolingSteps;
                IterationsPerCoolingStep = iterationsPerCoolingStep;
                InitialTemperature = initialTemperature;
                CoolingAlpha = coolingAlpha;
            }
        }

        private sealed class Partition<T>
        {
            public T[] Items;
            public double Cost;
        }

        /// <summary>
        /// Applies the simulated annealing algorithm to a given <paramref name="problemInput" />.
        /// </summary>
        /// <typeparam name="TInput">The type of the input data.</typeparam>
        /// <typeparam name="TSolution">The type of the solution.</typeparam>
        /// <param name="problemInput">The problem input.</param>
        /// <param name="problemInputLenght">The problem input lenght.</param>
        /// <param name="evaluateInitialSolutionFunc">Function used to evaluate initial complete solution.</param>
        /// <param name="solutionTransitionFunc">Function used to transition solution.</param>
        /// <param name="evaluateSolutionCostFunc">Function used to evaluate the full cost of a solution.</param>
        /// <param name="algorithmParams">The algorithm parameters.</param>
        /// <returns>
        /// The approximated solution.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="problemInput" /> or <paramref name="evaluateInitialSolutionFunc" /> or 
        /// <paramref name="solutionTransitionFunc" /> or <paramref name="evaluateSolutionCostFunc" /> or <paramref name="algorithmParams" /> are null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="problemInputLenght"/> is less than or equal to zero.</exception>
        public static TSolution ApplySimulatedAnnealing<TInput, TSolution>(
            TInput problemInput,
            int problemInputLenght,
            Func<TInput, TSolution> evaluateInitialSolutionFunc,
            Func<TSolution, TInput, int, int, double> solutionTransitionFunc,
            Func<TSolution, double> evaluateSolutionCostFunc,
            SimulatedAnnealingParams algorithmParams)
        {
            Validate.ArgumentNotNull(nameof(problemInput), problemInput);
            Validate.ArgumentNotNull(nameof(evaluateInitialSolutionFunc), evaluateInitialSolutionFunc);
            Validate.ArgumentGreaterThanZero(nameof(problemInputLenght), problemInputLenght);
            Validate.ArgumentNotNull(nameof(solutionTransitionFunc), solutionTransitionFunc);
            Validate.ArgumentNotNull(nameof(evaluateSolutionCostFunc), evaluateSolutionCostFunc);
            Validate.ArgumentNotNull(nameof(algorithmParams), algorithmParams);

            var temperature = algorithmParams.InitialTemperature;
            var solution = evaluateInitialSolutionFunc(problemInput);
            var currentSolutionCost = evaluateSolutionCostFunc(solution);
            var random = new Random();

            const double kb = 1.3807e-16;

            for (var i = 0; i < algorithmParams.CoolingSteps; i++)
            {
                temperature *= algorithmParams.CoolingAlpha;
                var startCost = currentSolutionCost;

                for (var j = 0; j < algorithmParams.IterationsPerCoolingStep; j++)
                {
                    var i1 = random.Next(problemInputLenght);
                    var i2 = random.Next(problemInputLenght);
                    var flip = random.NextDouble();

                    var delta = solutionTransitionFunc(solution, problemInput, i1, i2);
                    var exponent = (-delta / currentSolutionCost) / (kb * temperature);
                    var merit = Math.Pow(Math.E, exponent);

                    if (delta < 0)
                    {
                        currentSolutionCost = currentSolutionCost + delta;
                    }
                    else
                    {
                        if (merit > flip)
                        {
                            currentSolutionCost = currentSolutionCost + delta;
                        }
                        else
                        {
                            solutionTransitionFunc(solution, problemInput, i1, i2);
                        }
                    }
                }

                if (currentSolutionCost - startCost < 0)
                {
                    temperature = temperature / algorithmParams.CoolingAlpha;
                }
            }

            return solution;
        }

        /// <summary>
        /// Applies the combinatorial simulated annealing algorithm to a given <paramref name="sequence" />.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <param name="partitionLength">Length of a partition.</param>
        /// <param name="evaluatePartitionCostFunc">Function used evaluate the cost of a given partition.</param>
        /// <param name="algorithmParams">The algorithm parameters.</param>
        /// <returns>A sequence of partitions whose total cost is the approximated minimum.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> or <paramref name="evaluatePartitionCostFunc"/> or <paramref name="algorithmParams"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="partitionLength"/> is less than one.</exception>
        public static T[][] ApplySimulatedAnnealing<T>(
            IList<T> sequence, 
            int partitionLength, 
            Func<T[], double> evaluatePartitionCostFunc,
            SimulatedAnnealingParams algorithmParams)
        {
            Validate.ArgumentNotNull(nameof(sequence), sequence);
            Validate.ArgumentNotNull(nameof(evaluatePartitionCostFunc), evaluatePartitionCostFunc);
            Validate.ArgumentNotNull(nameof(algorithmParams), algorithmParams);
            Validate.ArgumentGreaterThanZero(nameof(partitionLength), partitionLength);

            var result = ApplySimulatedAnnealing(sequence, sequence.Count, input =>
                {
                    Debug.Assert(input != null);

                    /* Create partitions */
                    var partitions = new List<T[]>();
                    for (var i = 0; i < input.Count / partitionLength; i++)
                    {
                        partitions.Add(new T[partitionLength]);
                    }
                    var remainder = input.Count % partitionLength;
                    if (remainder != 0)
                    {
                        partitions.Add(new T[remainder]);
                    }

                    var array = partitions.Select(s => new Partition<T>
                    {
                        Cost = evaluatePartitionCostFunc(s),
                        Items = s,
                    }).ToArray();

                    for (var x = 0; x < input.Count; x++)
                    {
                        array[x / partitionLength].Items[x % partitionLength] = input[x];
                    }

                    foreach (var t in array)
                    {
                        t.Cost = evaluatePartitionCostFunc(t.Items);
                    }

                    return array;
                },
                (solution, input, i1, i2) =>
                {
                    Debug.Assert(solution != null);
                    Debug.Assert(input != null);
                    Debug.Assert(i1 >= 0 && i1 < input.Count);
                    Debug.Assert(i2 >= 0 && i2 < input.Count);

                    var partition1 = solution[i1 / partitionLength];
                    var partition2 = solution[i2 / partitionLength];

                    var temp = partition1.Items[i1 % partitionLength];
                    partition1.Items[i1 % partitionLength] = partition2.Items[i2 % partitionLength];
                    partition2.Items[i2 % partitionLength] = temp;

                    var newCost1 = evaluatePartitionCostFunc(partition1.Items);
                    var newCost2 = evaluatePartitionCostFunc(partition2.Items);

                    var delta = newCost1 - partition1.Cost + (newCost2 - partition2.Cost);
                    partition1.Cost = newCost1;
                    partition2.Cost = newCost2;

                    return delta;
                },
                solution =>
                {
                    Debug.Assert(solution != null);
                    return solution.Sum(s => s.Cost);
                },
                algorithmParams);

            return result.Select(s => s.Items).ToArray();
        }
    }
}
