namespace SingingFinder.Core

open System

// first Sunday
// first Sunday and Saturday before
// Saturday before first Sunday
// Friday and Saturday before first Sunday
// Jan 1
// Jan 1 - 3

type Singing = {Month:int; Day:string; Name:string }
type Days = {Start: DateTime; End: DateTime}

type SingingDays = 
    | Sunday
    | Saturday
    | SaturdayAndSunday
    | FridayAndSaturday

module Date=

    // Find the date of the first Sunday in a given month and year
    // Ex 1: The first Sunday in January 2017 is 1/1/2017
    // Ex 2: The first Sunday in January 2018 is 1/7/2017
    let findFirstSundayInMonth year month = 
        let rec firstSundayIn' (date:DateTime) = 
            match date.DayOfWeek with
            | DayOfWeek.Sunday  -> date
            | _                 -> firstSundayIn' (date.AddDays(1.0))

        firstSundayIn' (DateTime(year, month, 1))

    // Find the date of the Nth (e.g. first, second, etc) Sunday in a given month and year.
    // Ex 1: The first (n=1) Sunday in January 2017 is 1/1/2017
    // Ex 2: The second (n=2) Sunday in January 2017 is 1/8/2017
    // Account for the fact that months can sometimes have an e.g. "fifth" Sunday, and sometimes not.
    let dateOfNthSunday year month weekend =
        let firstSunday = findFirstSundayInMonth year month
        let weekOffset = weekend - 1
        let nthSunday' = firstSunday.AddDays(7.0 * (float)(weekOffset))
        if nthSunday'.Month = month then Some(nthSunday') else None

    // parse the singing description to determine which days of the week the singing will be held on
    let whichSingingDays (str:String) = 
        match str.ToLower() with
        | x when x.StartsWith("friday and saturday before") -> SingingDays.FridayAndSaturday        
        | x when x.StartsWith("saturday before")            -> SingingDays.Saturday        
        | x when x.EndsWith("sunday")                       -> SingingDays.Sunday
        | x when x.EndsWith("and saturday before")          -> SingingDays.SaturdayAndSunday
        | _ -> failwith ("could not determine days from: " + str)
    
    // parse the singing description to determine which weekend of the month the singing will be held on
    let whichWeekend (str:String) = 
        match str.ToLower() with
        | x when x.Contains("first")    -> 1   
        | x when x.Contains("second")   -> 2   
        | x when x.Contains("third")    -> 3   
        | x when x.Contains("fourth")   -> 4   
        | x when x.Contains("fifth")    -> 5   
        | _ -> failwith ("could not determine weekend from: " + str)

    // sometimes singings are on a specific date (January 1st, December 26th, etc.)
    let parseFromSpecificDate year month day =
        let date = (sprintf "%d/%d/%d" month day year) |> DateTime.Parse
        Some({Start=date; End=date})
    
    // figure the start/end dates of a singing based on the days of the week it falls on.
    let calculateRelativeDates sunday (str:String) =
        match whichSingingDays str with
        | SingingDays.Sunday            -> {Start=sunday; End=sunday}
        | SingingDays.Saturday          -> {Start=sunday.AddDays(-1.0); End=sunday.AddDays(-1.0)}
        | SingingDays.SaturdayAndSunday -> {Start=sunday.AddDays(-1.0); End=sunday}
        | SingingDays.FridayAndSaturday -> {Start=sunday.AddDays(-2.0); End=sunday.AddDays(-1.0)}

    // determine if this singing will be held based on its scheduled weekend, and if so on what specific days
    let parseFromRelativeDates year month (str:String) = 
        let nthSunday = whichWeekend str |> dateOfNthSunday year month
        match nthSunday with
        | Some(x) ->    Some(calculateRelativeDates x str)
        | _ ->          None

    // determine the singing days from its secription
    let parseSingingDates year month (str:String) =
        let mutable day = 0;
        if Int32.TryParse(str, &day) 
            then parseFromSpecificDate year month day
            else parseFromRelativeDates year month str  

    // determine if two date ranges overlap
    let dateRangesOverlap range1 range2 =
        match (range1, range2) with
        | (None, _) -> false
        | (_, None) -> false
        | (Some(x), Some(y)) -> x.Start <= y.End && y.Start <= x.End

    // take a list of singings and determine which, if any, fall within a supplied date range.
    let singingsWithinDateRange (days:Days) (singings:Singing list) = 
        
        let singingsWithinDateRangeDuringYear year singings = 
            singings 
            |> List.map (fun s -> (s, (parseSingingDates year s.Month s.Day)))
            |> List.filter (fun (s,d) -> dateRangesOverlap (Some(days)) d)

        // get a list of singings in each of the starting/ending years chosen by the user.
        // this accounts for scenarios where the dates span the new year (e.g. 12/1/2017-1/1/2018)
        let startYearSingings = singings |> singingsWithinDateRangeDuringYear days.Start.Year
        let endYearSingings = singings |> singingsWithinDateRangeDuringYear days.End.Year

        // combine the results, sort them by date, and remove duplicates.
        startYearSingings @ endYearSingings 
        |> List.sortBy (fun (s,Some(d)) -> d.Start)
        |> List.map (fun (s,d) -> s)
        |> List.distinct
