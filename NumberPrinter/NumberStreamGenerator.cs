using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberPrinter
{
    /// <summary>
    /// Encapsulates the process of producing a list of strings based on a provided key/value rule set.
    /// </summary>
    public class NumberStreamGenerator
    {
        private IDictionary<int, string> mappingRules;
        private IOrderedEnumerable<int> keyValues;

        /// <summary>
        /// Creates an instance for a provided set of mapping rules.
        /// </summary>
        /// <remarks>
        /// The output mappings will be applied with precedent given to the highest key value.
        /// </remarks>
        /// <param name="outputMappingRules">The key value pairs that define</param>
        public NumberStreamGenerator(IDictionary<int, string> outputMappingRules)
        {
            this.mappingRules = outputMappingRules;
            this.keyValues = (this.mappingRules != null) ? 
                this.mappingRules.Keys.Where(x => x != 0).OrderByDescending(x => x) : 
                Enumerable.Empty<int>().OrderByDescending(x => x);
        }

        /// <summary>
        /// Produces a stream of string values from 1 to the provided upper bound based on the rule mappings provided to the object.
        /// </summary>
        /// <param name="upperBound">The highest value to generate a return value for.</param>
        /// <returns>An enumerable of strings based on the rule mappings.</returns>
        public IEnumerable<string> GenerateOutputStream(int upperBound)
        {
            for (int i = 1; i <= upperBound; i++)
            {
                yield return PrintNumber(i);
            }
            yield break;
        }

        private string PrintNumber(int number)
        {
            foreach (var modKey in keyValues)
            {
                if (number % modKey == 0) return this.mappingRules[modKey];
            }

            return number.ToString();
        }
    }
}
