namespace SingingFinder.Core

module SingingRepository =
    
    open System
    open SingingCache
    open Date

    // get all singings with a valid location
    let singings() =
        getRecords()
        |> List.filter (fun s -> (s.Latitude,s.Longitude) <> (0.0,0.0))

    // get all singings for the selected date range and book type
    let singingsInRange (startDate:DateTime) (endDate:DateTime) (book:Book) rangeInMiles = 
        singings() 
        |> List.filter (fun s -> book = Book.All || s.Book.HasFlag(book))
        |> singingsWithinDateRange {Start=startDate; End=endDate}


