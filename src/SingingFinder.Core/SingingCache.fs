namespace SingingFinder.Core

module SingingCache=

    open System
    open FSharp.Data.Runtime.Caching
    open Database

    let [<Literal>]cacheKey = "singing-data"
    let cache = createInMemoryCache (TimeSpan.FromMinutes(3.0))

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
        
    let fetchRows =
        let query="""select 
	s.name, s.url, s.month, s.day, s.book, s.singing_type as type, s.time, s.info,
	l.id, l.name, l.url, l.address, l.city, l.county, l.state_province as stateProvince, l.postal_code as postalCode, l.country, l.gps_lat as latitude, l.gps_long as longitude
from singings s
inner join locations l on s.location_id = l.id"""

        let rows = 
            dapperComplexQuery query applyLocation
            |> Seq.toList
        cache.Set(cacheKey, rows)
        rows
    
    // get singing records from the cache, or fetch/cache them from the data source
    let getRecords() = 
        match cache.TryRetrieve cacheKey with 
        | Some(rows)    -> rows
        | _             -> fetchRows