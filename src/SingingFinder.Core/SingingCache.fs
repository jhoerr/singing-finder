﻿namespace SingingFinder.Core

open FSharp.Data
open Microsoft.Azure

type SingingRecord = CsvProvider<"./singings.csv", Schema="SingingType,Month,string,,,,,Book,float,float,">

module SingingCache=

    open System
    open FSharp.Data.Runtime.Caching

    let [<Literal>]cacheKey = "singing-data"
    let cache = createInMemoryCache (TimeSpan.FromMinutes(3.0))

    let toDomainSinging (r:SingingRecord.Row) =
      { Month=System.Enum.Parse(typeof<Month>,r.Month) :?> Month;
        Day=r.Day;
        Name=r.Name;
        SingingUrl=r.SingingUrl;
        Location=r.Location;
        Info=r.Info;
        Book=System.Enum.Parse(typeof<Book>,r.Book) :?> Book;
        Latitude=r.Latitude;
        Longitude=r.Longitude;
        LocationUrl=r.LocationUrl;
        Type=System.Enum.Parse(typeof<SingingType>,r.SingingType) :?> SingingType}

    // fetch singing records from the data source, convert them to domain records, and cache the result.
    let fetchRows() =
        let rows = 
            SingingRecord
                .Load(CloudConfigurationManager.GetSetting("SingingData.Url"))
                .Rows
            |> Seq.map toDomainSinging
            |> Seq.toList

        cache.Set (cacheKey,rows)
        rows

    // get singing records from the cache, or fetch/cache them from the data source
    let getRecords() = 
        match cache.TryRetrieve cacheKey with 
        | Some(rows)    -> rows
        | _             -> fetchRows()