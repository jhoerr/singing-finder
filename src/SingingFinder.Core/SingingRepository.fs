namespace SingingFinder.Core

open System
open FSharp.Data
open Date

type SingingRecord = CsvProvider<"./singings.csv", Schema="Month,string,,,,,Book,float,float,">

module SingingRepository =
    
    let getRecords () = 
        SingingRecord.Load("https://docs.google.com/spreadsheets/d/1fVm-niiqMko4eFa2P1sBHLlofLOsmiqkylYYbKaIuXw/export?format=csv")
        |> (fun d -> d.Rows)

    // to do: replace this with sql query
    let singings() =
        getRecords()
        |> Seq.map (fun r -> 
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
        |> Seq.filter (fun s -> (s.Latitude,s.Longitude) <> (0.0,0.0))
        |> Seq.toList

        // to do: replace this with sql query
    let singingsInRange (startDate:DateTime) (endDate:DateTime) rangeInMiles = 
        singings() 
        |> singingsWithinDateRange {Start=startDate; End=endDate}


