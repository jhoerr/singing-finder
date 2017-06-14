namespace SingingFinder.Core

open System
open FSharp.Data
open Date

type SingingRecord = CsvProvider<"./singings.csv", Schema="Month,string,,,,Book,float,float,">

module SingingRepository =

    // to do: replace this with sql query
    let singings =
        SingingRecord.Load("https://docs.google.com/spreadsheets/d/1fVm-niiqMko4eFa2P1sBHLlofLOsmiqkylYYbKaIuXw/export?format=csv")
        |> (fun d -> d.Rows)
        |> Seq.map (fun r -> 
          { Month=System.Enum.Parse(typeof<Month>,r.Month) :?> Month;
            Day=r.Day;
            Name=r.Name;
            Location=r.Location;
            Info=r.Info;
            Book=System.Enum.Parse(typeof<Book>,r.Book) :?> Book;
            Latitude=r.Latitude;
            Longitude=r.Longitude;
            LocationUrl=r.LocationUrl; })
        |> Seq.toList

        // to do: replace this with sql query
    let singingsInRange (startDate:DateTime) (endDate:DateTime) rangeInMiles = 
        singings 
        |> singingsWithinDateRange {Start=startDate; End=endDate}


