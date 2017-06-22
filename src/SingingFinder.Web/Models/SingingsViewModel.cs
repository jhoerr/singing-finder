using System;
using System.Collections.Generic;
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
    }
}