using System;
using System.Collections.Generic;
using System.Linq;
using SingingFinder.Core;

namespace SingingFinder.Web.Models
{
    public class SingingsViewModel
    {
        public SingingsViewModel(IEnumerable<Event> events, DateTime start, DateTime end, Book book, SingingType singingType)
        {
            Events = events;
            Start = start;
            End = end;
            Book = book;
            SingingType = singingType;
        }
            
        public IEnumerable<Event> Events { get; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public Book Book { get; }
        public SingingType SingingType { get; }

        public string Description() => $"There {IsAreClause} {CountClause} {TypeClause}{BookClause}{SingingClause} {DateClause}.";

        private int Count => Events.Count();
        private string IsAreClause => Count == 1 ? "is" : "are";
        private string SingingClause => Count == 1 ? "singing" : "singings";
        private string BookClause => Book == Book.All ? string.Empty : Book + " ";
        private string TypeClause => SingingType == SingingType.All ? string.Empty : SingingType.ToString().ToLowerInvariant() + " ";
        private string CountClause => Count == 0 ? "no" : Count.ToString();
        private string DateClause => Start == End ? $"on {Format(Start)}" : $"between {Format(Start)} and {Format(End)}";
        private string Format(DateTime date) => date.ToString("ddd, MMM d yyyy");
    }
}