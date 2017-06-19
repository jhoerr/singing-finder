namespace SingingFinder.Core

module SingingRepository =
    
    open System
    open SingingCache
    open Date

    // to do: replace this with sql query
    let singings() =
        getRecords()
        |> List.filter (fun s -> (s.Latitude,s.Longitude) <> (0.0,0.0))

        // to do: replace this with sql query
    let singingsInRange (startDate:DateTime) (endDate:DateTime) (book:Book) rangeInMiles = 
        singings() 
        |> List.filter (fun s -> book = Book.All || s.Book.HasFlag(book))
        |> singingsWithinDateRange {Start=startDate; End=endDate}


