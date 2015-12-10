using System;

using NumberPrinter;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NumberPrinterTests
{
    [TestFixture]
    public class NumberPrinterTest
    {
        [Test]
        [Description("Verify a simple sequence produced by the NumberPrinter.")]
        public void NumberPrinter_Verify_Sequence()
        {
            var rules = new Dictionary<int, string>();
            rules.Add(3, "Fizz");
            rules.Add(5, "Buzz");

            var generator = new NumberStreamGenerator(rules);
            var list = generator.GenerateOutputStream(15).ToList();

            CollectionAssert.AreEqual(new List<string>() { 
                "1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz", "11", "Fizz", "13", "14", "Buzz"}, list);
        }

        [Test]
        [Description("In the case where more than 1 key meet the criteria, the highest key value will be used.")]
        public void NumberPrinter_GreaterPrecedent()
        {
            var rules = new Dictionary<int, string>();
            rules.Add(3, "Fizz");
            rules.Add(5, "Buzz");

            var generator = new NumberStreamGenerator(rules);
            var list = generator.GenerateOutputStream(15);
            Assert.AreEqual(list.Last(), "Buzz");
        }

        [Test]
        [Description("A null input can be provided to the object, in which case there are no rules to apply.")]
        public void NumberPrinter_Null_Rules()
        {
            var generator = new NumberStreamGenerator(null);
            var list = generator.GenerateOutputStream(15).ToList();

            Assert.IsNotNull(list);
            CollectionAssert.AreEqual(new List<string>() { 
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15"}, list);
        }

        [Test]
        [Description("An empty input can be provided to the object, in which case there are no rules to apply.")]
        public void NumberPrinter_Empty_Rules()
        {
            var generator = new NumberStreamGenerator(new Dictionary<int, string>());
            var list = generator.GenerateOutputStream(15).ToList();

            Assert.IsNotNull(list);
            CollectionAssert.AreEqual(new List<string>() { 
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15"}, list);
        }

        [Test]
        [Description("A rule with a key value of 0 cannot be processed because of division of 0.  Key values of 0 are ignored.")]
        public void NumberPrinter_Zero_Rule()
        {
            var rules = new Dictionary<int, string>();
            rules.Add(0, "Beep");

            var generator = new NumberStreamGenerator(rules);
            var list = generator.GenerateOutputStream(15).ToList();

            Assert.IsNotNull(list);
            CollectionAssert.AreEqual(new List<string>() { 
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15"}, list);
        }

        [Test]
        [Description("A value of null is a legitimate output, verify the output is correct for this case.")]
        public void NumberPrinter_Null_Map_Value()
        {
            var rules = new Dictionary<int, string>();
            rules.Add(3, null);

            var generator = new NumberStreamGenerator(rules);
            var list = generator.GenerateOutputStream(15).ToList();

            Assert.IsNotNull(list);
            CollectionAssert.AreEqual(new List<string>() { 
            "1", "2", null, "4", "5", null, "7", "8", null, "10", "11", null, "13", "14", null}, list);
        }

        [Test]
        [Description("A provided upper bound of 0 will produce no result.")]
        public void NumberPrinter_Zero_Upper_Bound()
        {
            var rules = new Dictionary<int, string>();
            rules.Add(3, "Fizz");
            rules.Add(5, "Buzz");

            var generator = new NumberStreamGenerator(rules);
            var list = generator.GenerateOutputStream(0).ToList();

            Assert.AreEqual(list.Count, 0);
        }

        [Test]
        [Description("A provided upper bound less than 0 will produce no result.")]
        public void NumberPrinter_Negative_Upper_Bound()
        {
            var rules = new Dictionary<int, string>();
            rules.Add(3, "Fizz");
            rules.Add(5, "Buzz");

            var generator = new NumberStreamGenerator(rules);
            var list = generator.GenerateOutputStream(-1).ToList();

            Assert.AreEqual(list.Count, 0);
        }

        [Test]
        [Description("Provided rules can technically be modified while the result is being process, verify the results behave accordingly.")]
        public void NumberPrinter_Input_Modified()
        {
            var rules = new Dictionary<int, string>();
            rules.Add(3, "Fizz");
            rules.Add(5, "Buzz");

            var generator = new NumberStreamGenerator(rules);
            var list = generator.GenerateOutputStream(15);

            rules.Add(7, "Burp");
            rules.Remove(5);

            CollectionAssert.AreEqual(new List<string>() 
            { "1", "2", "Fizz", "4", "5", "Fizz", "Burp", "8", "Fizz", "10", "11", "Fizz", "13", "Burp", "Fizz" }, 
            list.ToList());
        }
    }
}
