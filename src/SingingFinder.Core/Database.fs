namespace SingingFinder.Core

open Microsoft.Azure

module Database=
    open System
    open System.Data.SQLite
    open Dapper

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
    
    let dbConnectionFromPath path = 
        let cn = new SQLiteConnection("DataSource="+path)
        cn.Open()
        cn

    let dbConnection ()= 
        sqlLitePath
        |> dbConnectionFromPath
    
    let dapperQueryWithConnection<'Result> (query:string) (connection:SQLiteConnection) =
        connection.Query<'Result>(query)
    
    let dapperQuery<'Result> (query:string) =
        dbConnection()
        |> dapperQueryWithConnection query

    let dapperComplexQueryWithConnection<'First,'Second,'Return> (query:string) (map:'First->'Second->'Return) (connection:SQLiteConnection) =
        let func = Func<'First,'Second,'Return> map
        connection.Query<'First,'Second,'Return>(query, func)
    
    let dapperComplexQuery<'First,'Second,'Return> (query:string) (map:'First->'Second->'Return) =
        dbConnection()
        |> dapperComplexQueryWithConnection query map