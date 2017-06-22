namespace SingingFinder.Core

module SingingRepository =
    
    open System
    open SingingCache
    open Date

    // get all singings with a valid location
    let getSingings() =
        getRecords()
        
    let filterSingingsInRange (startDate:DateTime) (endDate:DateTime) (book:Book) (singingType:SingingType) rangeInMiles singings =
        singings
        |> List.filter (fun s -> (s.Latitude,s.Longitude) <> (0.0,0.0))
        |> List.filter (fun s -> book = Book.All || int (s.Book &&& book) <> 0)
        |> List.filter (fun s -> singingType = SingingType.All || s.Type = singingType)
        |> singingsWithinDateRange {Start=startDate; End=endDate}

    // get all singings for the selected date range and book type
    let getSingingsInRange (startDate:DateTime) (endDate:DateTime) (book:Book) (singingType:SingingType) rangeInMiles = 
        getSingings()
        |> filterSingingsInRange startDate endDate book singingType rangeInMiles


