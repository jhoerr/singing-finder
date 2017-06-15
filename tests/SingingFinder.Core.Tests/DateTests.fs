namespace SingingFinder.Core.Tests

module DateTests=

    open System
    open SingingFinder.Core
    open Swensen.Unquote
    open NUnit.Framework

    open Date

    let defaultSinging = {Month=Month.January; Day="1"; Name=""; SingingUrl=""; Location=""; Info=""; Latitude=0.0; Longitude=0.0; LocationUrl=""; Book=Book.Denson}

    // firstSunday

    [<Test>]
    let ``first sunday: year is correct`` () =
        test <@ (findFirstSundayInMonth 2017 Month.January).Year = 2017 @>

    [<Test>]
    let ``first sunday: month is correct`` () =
        test <@ (findFirstSundayInMonth 2017 Month.February).Month = 2 @>

    [<Test>]
    let ``first sunday: day is correct (Jan 2017)`` () =
        test <@ (findFirstSundayInMonth 2017 Month.January).Day = 1 @>

    [<Test>]
    let ``first sunday: day is correct (Feb 2017)`` () =
        test <@ (findFirstSundayInMonth 2017 Month.February).Day = 5 @>

    // nthSunday

    [<Test>]
    let ``first sunday in Jan 2017`` () =
        test <@ dateOfNthSunday 2017 Month.January 1 = Some(DateTime(2017, 1, 1)) @>

    [<Test>]
    let ``second sunday in Jan 2017`` () =
        test <@ dateOfNthSunday 2017 Month.January 2 = Some(DateTime(2017, 1, 8)) @>

    [<Test>]
    let ``fifth sunday in Jan 2017`` () =
        test <@ dateOfNthSunday 2017 Month.January 5 = Some(DateTime(2017, 1, 29)) @>

    let ``fifth sunday in Jan 2018`` () =
        test <@ dateOfNthSunday 2018 Month.January 5 = None @>

    // parseSingingDays

    [<Test>]
    let ``sunday`` () =
        test <@ whichSingingDays "First Sunday" = SingingDays.Sunday @>

    [<Test>]
    let ``saturday`` () =
        test <@ whichSingingDays "Saturday before First Sunday" = SingingDays.Saturday @>

    [<Test>]
    let ``friday and saturday`` () =
        test <@ whichSingingDays "Friday and Saturday before First Sunday" = SingingDays.FridayAndSaturday @>

    [<Test>]
    let ``saturday and sunday`` () =
        test <@ whichSingingDays "First Sunday and Saturday Before" = SingingDays.SaturdayAndSunday @>

    // parseSinginWeekend
    let parseSingingDatesTestData =
        [
            TestCaseData(2017, Month.February, "14").Returns(Some({Start=DateTime(2017,2,14); End=DateTime(2017,2,14)}))
            TestCaseData(2018, Month.February, "14").Returns(Some({Start=DateTime(2018,2,14); End=DateTime(2018,2,14)}))
            TestCaseData(2017, Month.December, "25").Returns(Some({Start=DateTime(2017,12,25); End=DateTime(2017,12,25)}))
            TestCaseData(2018, Month.December, "25").Returns(Some({Start=DateTime(2018,12,25); End=DateTime(2018,12,25)}))
            //
            TestCaseData(2017, Month.January, "First Sunday").Returns(Some({Start=DateTime(2017,1,1); End=DateTime(2017,1,1)}))
            TestCaseData(2018, Month.January, "First Sunday").Returns(Some({Start=DateTime(2018,1,7); End=DateTime(2018,1,7)})) 
            TestCaseData(2017, Month.February, "First Sunday").Returns(Some({Start=DateTime(2017,2,5); End=DateTime(2017,2,5)}))
            TestCaseData(2018, Month.February, "First Sunday").Returns(Some({Start=DateTime(2018,2,4); End=DateTime(2018,2,4)}))
            //
            TestCaseData(2017, Month.January, "Second Sunday").Returns(Some({Start=DateTime(2017,1,8); End=DateTime(2017,1,8)}))
            TestCaseData(2018, Month.January, "Second Sunday").Returns(Some({Start=DateTime(2018,1,14); End=DateTime(2018,1,14)}))
            TestCaseData(2017, Month.February, "Second Sunday").Returns(Some({Start=DateTime(2017,2,12); End=DateTime(2017,2,12)}))
            TestCaseData(2018, Month.February, "Second Sunday").Returns(Some({Start=DateTime(2018,2,11); End=DateTime(2018,2,11)}))
            //
            TestCaseData(2017, Month.January, "Fifth Sunday").Returns(Some({Start=DateTime(2017,1,29); End=DateTime(2017,1,29)}))
            TestCaseData(2018, Month.January, "Fifth Sunday").Returns(None)
            //
            TestCaseData(2017, Month.January, "Saturday before First Sunday").Returns(Some({Start=DateTime(2016,12,31); End=DateTime(2016,12,31)}))
            TestCaseData(2018, Month.January, "Saturday before First Sunday").Returns(Some({Start=DateTime(2018,1,6); End=DateTime(2018,1,6)}))
            TestCaseData(2017, Month.February, "Saturday before First Sunday").Returns(Some({Start=DateTime(2017,2,4); End=DateTime(2017,2,4)}))
            TestCaseData(2018, Month.February, "Saturday before First Sunday").Returns(Some({Start=DateTime(2018,2,3); End=DateTime(2018,2,3)}))
            //
            TestCaseData(2017, Month.January, "Saturday before Second Sunday").Returns(Some({Start=DateTime(2017,1,7); End=DateTime(2017,1,7)}))
            TestCaseData(2018, Month.January, "Saturday before Second Sunday").Returns(Some({Start=DateTime(2018,1,13); End=DateTime(2018,1,13)}))
            TestCaseData(2017, Month.February, "Saturday before Second Sunday").Returns(Some({Start=DateTime(2017,2,11); End=DateTime(2017,2,11)}))
            TestCaseData(2018, Month.February, "Saturday before Second Sunday").Returns(Some({Start=DateTime(2018,2,10); End=DateTime(2018,2,10)}))
            //
            TestCaseData(2017, Month.January, "Saturday before Fifth Sunday").Returns(Some({Start=DateTime(2017,1,28); End=DateTime(2017,1,28)}))
            TestCaseData(2018, Month.January, "Saturday before Fifth Sunday").Returns(None)
            //
            TestCaseData(2017, Month.January, "First Sunday and Saturday Before").Returns(Some({Start=DateTime(2016,12,31); End=DateTime(2017,1,1)}))
            TestCaseData(2018, Month.January, "First Sunday and Saturday Before").Returns(Some({Start=DateTime(2018,1,6); End=DateTime(2018,1,7)})) 
            TestCaseData(2017, Month.February, "First Sunday and Saturday Before").Returns(Some({Start=DateTime(2017,2,4); End=DateTime(2017,2,5)}))
            TestCaseData(2018, Month.February, "First Sunday and Saturday Before").Returns(Some({Start=DateTime(2018,2,3); End=DateTime(2018,2,4)}))
            //
            TestCaseData(2017, Month.January, "Second Sunday and Saturday Before").Returns(Some({Start=DateTime(2017,1,7); End=DateTime(2017,1,8)}))
            TestCaseData(2018, Month.January, "Second Sunday and Saturday Before").Returns(Some({Start=DateTime(2018,1,13); End=DateTime(2018,1,14)}))
            TestCaseData(2017, Month.February, "Second Sunday and Saturday Before").Returns(Some({Start=DateTime(2017,2,11); End=DateTime(2017,2,12)}))
            TestCaseData(2018, Month.February, "Second Sunday and Saturday Before").Returns(Some({Start=DateTime(2018,2,10); End=DateTime(2018,2,11)}))
            //
            TestCaseData(2017, Month.January, "Fifth Sunday and Saturday Before").Returns(Some({Start=DateTime(2017,1,28); End=DateTime(2017,1,29)}))
            TestCaseData(2018, Month.January, "Fifth Sunday and Saturday Before").Returns(None)
            //
            TestCaseData(2017, Month.January, "Friday and Saturday before First Sunday").Returns(Some({Start=DateTime(2016,12,30); End=DateTime(2016,12,31)}))
            TestCaseData(2018, Month.January, "Friday and Saturday before First Sunday").Returns(Some({Start=DateTime(2018,1,5); End=DateTime(2018,1,6)}))
            TestCaseData(2017, Month.February, "Friday and Saturday before First Sunday").Returns(Some({Start=DateTime(2017,2,3); End=DateTime(2017,2,4)}))
            TestCaseData(2018, Month.February, "Friday and Saturday before First Sunday").Returns(Some({Start=DateTime(2018,2,2); End=DateTime(2018,2,3)}))
            //
            TestCaseData(2017, Month.January, "Friday and Saturday before Second Sunday").Returns(Some({Start=DateTime(2017,1,6); End=DateTime(2017,1,7)}))
            TestCaseData(2018, Month.January, "Friday and Saturday before Second Sunday").Returns(Some({Start=DateTime(2018,1,12); End=DateTime(2018,1,13)}))
            TestCaseData(2017, Month.February, "Friday and Saturday before Second Sunday").Returns(Some({Start=DateTime(2017,2,10); End=DateTime(2017,2,11)}))
            TestCaseData(2018, Month.February, "Friday and Saturday before Second Sunday").Returns(Some({Start=DateTime(2018,2,9); End=DateTime(2018,2,10)}))
            //
            TestCaseData(2017, Month.January, "Friday and Saturday before Fifth Sunday").Returns(Some({Start=DateTime(2017,1,27); End=DateTime(2017,1,28)}))
            TestCaseData(2018, Month.January, "Friday and Saturday before Fifth Sunday").Returns(None)
        ]
    
    [<TestCaseSource("parseSingingDatesTestData")>]
    let ``parse singing dates`` year month (str:String) = 
        {defaultSinging with Day=str; Month=month; Name="singing"} 
        |> parseSingingDates year 

    // dateWithinRange

    let dateWithinRangeTestData =
        [
            // these represent user-selected start/end dates for a search.
            // the test date range is 1/1/2017-1/3/2017
            TestCaseData("1/1/2017", "1/1/2017").Returns(true)
            TestCaseData("1/2/2017", "1/2/2017").Returns(true)
            TestCaseData("1/3/2017", "1/3/2017").Returns(true)
            TestCaseData("12/31/2016", "1/1/2017").Returns(true)
            TestCaseData("1/3/2017", "1/4/2017").Returns(true)
            TestCaseData("12/31/2016", "1/4/2017").Returns(true)
            // negative e
            TestCaseData("12/31/2016", "12/31/2016").Returns(false)
            TestCaseData("1/4/2017", "1/4/2017").Returns(false)
            // far past
            TestCaseData("1/1/2016", "1/1/2016").Returns(false)
            TestCaseData("1/2/2016", "1/2/2016").Returns(false)
            TestCaseData("1/3/2016", "1/3/2016").Returns(false)
            TestCaseData("12/31/2015", "1/1/2016").Returns(false)
            TestCaseData("1/3/2016", "1/4/2016").Returns(false)
            TestCaseData("12/31/2015", "1/4/2016").Returns(false)
            // far future
            TestCaseData("1/1/2018", "1/1/2018").Returns(false)
            TestCaseData("1/2/2018", "1/2/2018").Returns(false)
            TestCaseData("1/3/2018", "1/3/2018").Returns(false)
            TestCaseData("12/31/2017","1/1/2018").Returns(false)
            TestCaseData("1/3/2018", "1/4/2018").Returns(false)
            TestCaseData("12/31/2017", "1/4/2018").Returns(false)
        ]

    [<TestCaseSource("dateWithinRangeTestData")>]
    let ``date in range`` (candidateStart:String) (candidateEnd:String) = 
        let range1 = {Start=DateTime(2017,1,1); End=DateTime(2017,1,3)}
        let range2 = {Start=DateTime.Parse(candidateStart); End=DateTime.Parse(candidateEnd)}
        dateRangesOverlap range1 range2

    // singingsWithinDateRange
    let newYearsDay =   {defaultSinging with Month=Month.January;   Day="1";            Name="new years day"}
    let christmasDay =  {defaultSinging with Month=Month.December;  Day="25";           Name="christmas day"}
    let firstSunInJan = {defaultSinging with Month=Month.January;   Day="First Sunday"; Name="first sunday in january"}
    let ohioConvention = {defaultSinging with Month=Month.March;   Day="First Sunday and Saturday Before"; Name="first sunday and saturday before in march"}
    let fifthSundayInJan = {defaultSinging with Month=Month.January;   Day="Fifth Sunday"; Name="fifthy sunday in january"}

    let singings = 
        [
            newYearsDay; 
            christmasDay; 
            firstSunInJan;
            fifthSundayInJan;
            ohioConvention
        ]
    
    let generalTests =
        [
            TestCaseData("1/1/2017", "1/1/2017").Returns(
                [
                    {Singing=newYearsDay; Days={Start=DateTime(2017,1,1); End=DateTime(2017,1,1)}}
                    {Singing=firstSunInJan; Days={Start=DateTime(2017,1,1); End=DateTime(2017,1,1)}} 
                ])
            TestCaseData("1/1/2018", "1/1/2018").Returns(
                [
                    {Singing=newYearsDay; Days={Start=DateTime(2018,1,1); End=DateTime(2018,1,1)}}
                ])
            TestCaseData("1/1/2018", "1/15/2018").Returns(
                [
                    {Singing=newYearsDay; Days={Start=DateTime(2018,1,1); End=DateTime(2018,1,1)}}
                    {Singing=firstSunInJan; Days={Start=DateTime(2018,1,7); End=DateTime(2018,1,7)}}
                ])
            TestCaseData("12/20/2017", "12/31/2017").Returns(
                [
                    {Singing=christmasDay; Days={Start=DateTime(2017,12,25); End=DateTime(2017,12,25)}}
                ])
            TestCaseData("12/1/2017", "1/1/2018").Returns(
                [
                    {Singing=christmasDay; Days={Start=DateTime(2017,12,25); End=DateTime(2017,12,25)}}
                    {Singing=newYearsDay; Days={Start=DateTime(2018,1,1); End=DateTime(2018,1,1)}}
                ])
            TestCaseData("1/1/2017", "1/1/2018").Returns(
                [
                    {Singing=newYearsDay; Days={Start=DateTime(2017,1,1); End=DateTime(2017,1,1)}}
                    {Singing=firstSunInJan; Days={Start=DateTime(2017,1,1); End=DateTime(2017,1,1)}}
                    {Singing=fifthSundayInJan; Days={Start=DateTime(2017,1,29); End=DateTime(2017,1,29)}}
                    {Singing=ohioConvention; Days={Start=DateTime(2017,3,4); End=DateTime(2017,3,5)}}
                    {Singing=christmasDay; Days={Start=DateTime(2017,12,25); End=DateTime(2017,12,25)}}
                    {Singing=newYearsDay; Days={Start=DateTime(2018,1,1); End=DateTime(2018,1,1)}}
                ])
        ]

    let fifthSundayTests =
        [
            TestCaseData("1/1/2017", "2/1/2017").Returns(
                [
                    {Singing=newYearsDay; Days={Start=DateTime(2017,1,1); End=DateTime(2017,1,1)}}
                    {Singing=firstSunInJan; Days={Start=DateTime(2017,1,1); End=DateTime(2017,1,1)}}
                    {Singing=fifthSundayInJan; Days={Start=DateTime(2017,1,29); End=DateTime(2017,1,29)}}
                ])
            TestCaseData("1/1/2018", "2/1/2018").Returns(
                [
                    {Singing=newYearsDay; Days={Start=DateTime(2018,1,1); End=DateTime(2018,1,1)}}
                    {Singing=firstSunInJan; Days={Start=DateTime(2018,1,7); End=DateTime(2018,1,7)}}
                ])
        ]

    let multiDaySingingBoundaryTests =
        [
            TestCaseData("3/3/2017", "3/3/2017").Returns([])
            TestCaseData("3/3/2017", "3/4/2017").Returns([{Singing=ohioConvention; Days={Start=DateTime(2017,3,4); End=DateTime(2017,3,5)}}])
            TestCaseData("3/4/2017", "3/5/2017").Returns([{Singing=ohioConvention; Days={Start=DateTime(2017,3,4); End=DateTime(2017,3,5)}}])
            TestCaseData("3/5/2017", "3/6/2017").Returns([{Singing=ohioConvention; Days={Start=DateTime(2017,3,4); End=DateTime(2017,3,5)}}])
            TestCaseData("3/3/2017", "3/6/2017").Returns([{Singing=ohioConvention; Days={Start=DateTime(2017,3,4); End=DateTime(2017,3,5)}}])
            TestCaseData("3/6/2017", "3/6/2017").Returns([])
        ]

    [<TestCaseSource("generalTests")>]
    [<TestCaseSource("fifthSundayTests")>]
    [<TestCaseSource("multiDaySingingBoundaryTests")>]
    let ``singings within date range`` (startDay:String) (endDay:String) =
        singings |> singingsWithinDateRange {Start=DateTime.Parse(startDay); End=DateTime.Parse(endDay)}
