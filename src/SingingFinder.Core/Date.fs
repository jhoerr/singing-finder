namespace SingingFinder.Core

open System

type Month =
    | January=1
    | February=2
    | March=3
    | April=4
    | May=5
    | June=6
    | July=7
    | August=8
    | September=9
    | October=10
    | November=11
    | December=12

type Singing = 
  { Month:Month; 
    Day:string; 
    Name:string; 
    Location: string; 
    Latitude: float; 
    Longitude: float; 
    Info: string; 
    LocationUrl: string }

type Days = 
  { Start: DateTime; 
    End: DateTime }

type Event =
  { Singing: Singing;
    Days: Days }

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

        firstSundayIn' (DateTime(year, int month, 1))

    // Find the date of the Nth (e.g. first, second, etc) Sunday in a given month and year.
    // Ex 1: The first (n=1) Sunday in January 2017 is 1/1/2017
    // Ex 2: The second (n=2) Sunday in January 2017 is 1/8/2017
    // Account for the fact that months can sometimes have an e.g. "fifth" Sunday, and sometimes not.
    let dateOfNthSunday year month weekend =
        let firstSunday = findFirstSundayInMonth year month
        let weekOffset = weekend - 1
        let nthSunday' = firstSunday.AddDays(7.0 * (float)(weekOffset))
        if nthSunday'.Month = int month then Some(nthSunday') else None

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
        let date = (sprintf "%d/%d/%d" (int month) day year) |> DateTime.Parse
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
        let nthSunday = 
            str 
            |> whichWeekend 
            |> dateOfNthSunday year month

        match nthSunday with
        | Some(x) ->    calculateRelativeDates x str |> Some
        | _ ->          None

    // determine the singing days from its secription
    let parseSingingDates year singing =
        let mutable specificDay = 0;
        if Int32.TryParse(singing.Day, &specificDay) 
            then parseFromSpecificDate year singing.Month specificDay
            else parseFromRelativeDates year singing.Month singing.Day
    
    let determineSingingDates year singing =
        let days = parseSingingDates year singing
        match days with
        | Some(x) -> Some({ Singing = singing; Days = x })
        | None    -> None

    // determine if two date ranges overlap
    let dateRangesOverlap range1 range2 =
        range1.Start <= range2.End && range2.Start <= range1.End

    // take a list of singings and determine which, if any, fall within a supplied date range.
    let singingsWithinDateRange (days:Days) (singings:Singing list) = 
        // get a list of singings in each of the starting/ending years chosen by the user.
        (singings |> List.map (fun s -> determineSingingDates days.Start.Year s))
        @ (singings |> List.map (fun s -> determineSingingDates days.End.Year s))
        |> List.filter (fun e -> e.IsSome)
        |> List.map (fun e -> e.Value)
        |> List.filter (fun e -> dateRangesOverlap days e.Days)
        |> List.sortBy (fun e -> e.Days.Start)
        |> List.distinct
