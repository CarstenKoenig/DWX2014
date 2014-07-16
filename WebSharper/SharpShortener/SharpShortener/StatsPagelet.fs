namespace SharpShortener

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Google
open IntelliFactory.WebSharper.Google.Visualization
open IntelliFactory.WebSharper.Google.Visualization.Base

[<JavaScript>]
module StatsPagelet =

    let getStatsData (drawTo : ColumnChart, options : ColumnChartOptions) =
        async {
            let stats = SharpShortener.Remoting.getStats()
            let data = new Base.DataTable()
            data.addColumn(ColumnType.StringType, "url") |> ignore
            data.addColumn(ColumnType.NumberType, "#hits") |> ignore
            stats
            |> Array.iter (fun (url, anz) ->
                let i = data.addRow()
                data.setValue(i, 0, Url.nameOnly url)
                data.setValue(i, 1, anz)
            )
            drawTo.draw (data, options)
        } |> Async.Start

    let StatsChart () =
        Div []
        |>! OnAfterRender (fun container ->
            let visualization = new ColumnChart(container.Dom)
            let options =
                ColumnChartOptions(
                    width = 600,
                    height = 300,
                    legend = Legend(position = LegendPosition.Bottom),
                    title = "Hitcounter")
            getStatsData (visualization, options))    

    let Main () =
        Div [
            H2 [Text "Besucherzähler der registrierten Seiten"]
            StatsChart()
        ] -< [ Attr.Class "statistik" ]