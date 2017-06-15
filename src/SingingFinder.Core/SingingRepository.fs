namespace SingingFinder.Core

open FSharp.Data

type SingingRecord = CsvProvider<"./singings.csv", Schema="Month,string,,,,,Book,float,float,">

module SingingRepository =

    open System
    open FSharp.Data.Runtime.Caching
    open Date
    
    let [<Literal>]cacheKey = "singing-data"
    let cache = createInMemoryCache (TimeSpan.FromMinutes(3.0))

    let fetchRows =
        let rows = 
            SingingRecord
                .Load("https://docs.google.com/spreadsheets/d/1fVm-niiqMko4eFa2P1sBHLlofLOsmiqkylYYbKaIuXw/export?format=csv")
                .Rows
            |> Seq.toList
        cache.Set (cacheKey,rows)
        rows

    let getRecords () = 
        match cache.TryRetrieve cacheKey with 
        | Some(rows)    -> rows
        | _             -> fetchRows

    // to do: replace this with sql query
    let singings() =
        getRecords()
        |> List.map (fun r -> 
          { Month=System.Enum.Parse(typeof<Month>,r.Month) :?> Month;
            Day=r.Day;
            Name=r.Name;
            SingingUrl=r.SingingUrl;
            Location=r.Location;
            Info=r.Info;
            Book=System.Enum.Parse(typeof<Book>,r.Book) :?> Book;
            Latitude=r.Latitude;
            Longitude=r.Longitude;
            LocationUrl=r.LocationUrl; })
        |> List.filter (fun s -> (s.Latitude,s.Longitude) <> (0.0,0.0))

        // to do: replace this with sql query
    let singingsInRange (startDate:DateTime) (endDate:DateTime) rangeInMiles = 
        singings() 
        |> singingsWithinDateRange {Start=startDate; End=endDate}


