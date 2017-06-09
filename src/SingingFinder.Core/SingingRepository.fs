namespace SingingFinder.Core

open System

module SingingRepository =

    // to do: replace this with sql query
    let singings = 
        [
            {Name = "Ohio State Convention"; Day = "First Saturday and Sunday Before"; Month = 3}
            {Name = "Missouri State Convention"; Day = "Second Saturday and Sunday Before"; Month = 3}
            {Name = "Bloomington All-Day"; Day = "Saturday Before the Fourth Sunday"; Month = 10}
            {Name = "Higher Ground"; Day = "Saturday Before the Fourth Sunday"; Month = 4}
        ]

    // to do: replace this with sql query
    let singingsInRange (lattitude:float,longitude:float) rangeInMiles = 
        [
            {Name = "Bloomington All-Day"; Day = "Saturday Before the Fourth Sunday"; Month = 10}
            {Name = "Higher Ground"; Day = "Saturday Before the Fourth Sunday"; Month = 4}
        ]


