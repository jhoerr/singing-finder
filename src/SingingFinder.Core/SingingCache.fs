namespace SingingFinder.Core

open Microsoft.Azure

module SingingCache=

    open System
    open FSharp.Data.Runtime.Caching
    open System.Data.SQLite
    open Dapper

    let [<Literal>]cacheKey = "singing-data"
    let cache = createInMemoryCache (TimeSpan.FromMinutes(3.0))

    let dbConnection path = 
        let cn = new SQLiteConnection("DataSource="+path)
        cn.Open()
        cn

    let applyLocation s l =
      { Name = s.Name;
        Month = s.Month;
        Book = s.Book;
        Day = s.Day;
        Time = s.Time;
        Type = s.Type;
        Info = s.Info;
        Url = s.Url;
        Location = l; }
        
    let fetchRows' (cn:SQLiteConnection) =
        let query="""select 
	s.name, s.url, s.month, s.day, s.book, s.singing_type as type, s.time, s.info,
	l.id, l.name, l.url, l.address, l.city, l.county, l.state_province as stateProvince, l.postal_code as postalCode, l.country, l.gps_lat as latitude, l.gps_long as longitude
from singings s
inner join locations l on s.location_id = l.id"""

        let func = Func<Singing,Location,Singing> applyLocation
        cn.Query<Singing,Location,Singing>(query, func)
        |> Seq.toList
    
    let sqlLitePath =
        let path = CloudConfigurationManager.GetSetting("SQLite.DB.Path")
        if path = "CHANGEME"
        then 
            System.Reflection.Assembly.GetExecutingAssembly().CodeBase
            |> UriBuilder
            |> (fun uri -> Uri.UnescapeDataString(uri.Path))
            |> System.IO.Path.GetDirectoryName
            |> (fun dir -> dir + "\\..\\..\\..\\db\\minutes.db")
        else path

    // get singing records from the cache, or fetch/cache them from the data source
    let getRecords() = 
        match cache.TryRetrieve cacheKey with 
        | Some(rows)    -> rows
        | _             -> 
            sqlLitePath
            |> dbConnection
            |> fetchRows'