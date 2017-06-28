using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Collections;
using NUnit.Framework;
using SingingFinder.Core;
using SingingFinder.Web.Models;

namespace SingingFinder.Web.Tests.Models
{
    [TestFixture]
    public class SingingsViewModelTests
    {
        static readonly DateTime Day = new DateTime(2017, 1, 1);
        static readonly Singing Singing = new Singing(Month.May, "day","","name", new Location("", "", "", "", "", 0.0, 0.0, ""), "info", "url", Book.All, SingingType.All);
        static readonly Event Event = new Event(Singing, new FSharpList<Days>(new Days(Day, Day), FSharpList<Days>.Empty));

        static readonly object[] TestCases =
        {
            new object[] { new SingingsViewModel(new Event[0], Day, Day, Book.All, SingingType.All), "There are no singings on Sun, Jan 1 2017." },
            new object[] { new SingingsViewModel(new []{Event}, Day, Day, Book.All, SingingType.All), "There is 1 singing on Sun, Jan 1 2017." },
            new object[] { new SingingsViewModel(new []{Event, Event}, Day, Day, Book.All, SingingType.All), "There are 2 singings on Sun, Jan 1 2017." },
            new object[] { new SingingsViewModel(new []{Event}, Day, Day.AddDays(1), Book.All, SingingType.All), "There is 1 singing between Sun, Jan 1 2017 and Mon, Jan 2 2017." },
            new object[] { new SingingsViewModel(new []{Event}, Day, Day, (Book)1 , SingingType.All), "There is 1 1991 Edition singing on Sun, Jan 1 2017." },
            new object[] { new SingingsViewModel(new []{Event}, Day, Day, Book.All, SingingType.Annual), "There is 1 annual singing on Sun, Jan 1 2017." },
            new object[] { new SingingsViewModel(new []{Event}, Day, Day, (Book)1, SingingType.Annual), "There is 1 annual 1991 Edition singing on Sun, Jan 1 2017." },
        };

        [TestCaseSource(nameof(TestCases))]
        public void Description(SingingsViewModel model, string expected)
            => Assert.That(model.Description(), Is.EqualTo(expected));

        private Location loc = new Location("loc", "", "", "", "", 0.0, 0.0, "");
        private Location locprime = new Location("loc", "", "", "", "", 0.0, 0.0, ""); // different object, but structurally equal to 'loc'

        [Test]
        public void StructuralEqualityRespected()
        {
            Assert.AreEqual(loc, locprime);
        }

        [Test]
        public void GroupBy()
        {
            var s1 = new Singing(Month.January, "", "", "", loc, "", "", Book.All, SingingType.Annual);
            var s2 = new Singing(Month.February, "", "", "", locprime, "", "", Book.All, SingingType.Annual);

            var result = new List<Singing>() {s1, s2}.GroupBy(s => s.Location);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Key, Is.EqualTo(loc));
            Assert.That(result.First().Count(), Is.EqualTo(2));
        }

    }
}
