Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json
Imports System.IO
Imports System.Net
Imports HtmlAgilityPack

Public Class Form1
    Private Declare Function SetProcessWorkingSetSize Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal dwMinimumWorkingSetSize As Int32, ByVal dwMaximumWorkingSetSize As Int32) As Int32

    'API Setup
    Public Class pkmnAPI
        Public Property count As Integer
        Public Property next1 As Object
        Public Property previous As Object
        Public Property results As Result()
    End Class
    Public Class Result
        Public Property name As String
        Public Property url As String
    End Class

    'Variables
    Dim request As HttpWebRequest
    Dim response As HttpWebResponse = Nothing
    Dim reader As StreamReader
    Dim rawJson As String
    Dim jsonURL As String = "https://pokeapi.co/api/v2/pokemon/?limit=6969"
    Dim ttlPkmn As Integer = 0

    'Json parser
    Public Sub JsonParse(ByVal url As String)
        'Take request url then take the response and save it to our global variable
        request = DirectCast(WebRequest.Create(url), HttpWebRequest)
        response = DirectCast(request.GetResponse(), HttpWebResponse)
        reader = New StreamReader(response.GetResponseStream())
        rawJson = reader.ReadToEnd

        'Close the reader
        reader.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CountPkmn()
    End Sub

    Public Sub CountPkmn()
        'Parse json url
        JsonParse(jsonURL)
        'Deserialize json
        Dim pkmn As New pkmnAPI
        pkmn = JsonConvert.DeserializeObject(Of pkmnAPI)(rawJson)
        'Count total pokemon via pokeapi json 
        Dim count As Integer = pkmn.results.Count
        Dim newname As String
        Dim i As Integer = 1

        If Not pkmn.results(i).name.EndsWith("-mega") Then
            Do
                If pkmn.results(i - 1).name.Contains("-f") Then
                    newname = pkmn.results(i - 1).name.Substring(0, pkmn.results(i - 1).name.Length - 2)
                ElseIf pkmn.results(i - 1).name.EndsWith("-m") Then
                    newname = pkmn.results(i - 1).name.Substring(0, pkmn.results(i - 1).name.Length - 2)
                Else
                    newname = pkmn.results(i - 1).name
                End If

                If pkmn.results(i).name = "farfetchd" Then newname = "farfetch%27d"
                If pkmn.results(i).name = "mr-mime" Then newname = "mr._Mime"
                If pkmn.results(i).name = "ho-oh" Then newname = "ho-Oh"
                If pkmn.results(i).name = "deoxys-normal" Then newname = "deoxys"
                If pkmn.results(i).name = "wormadam-plant" Then newname = "wormadam"
                If pkmn.results(i).name = "cherrim" Then newname = "cherrim-Overcast"
                If pkmn.results(i).name = "mime-jr" Then newname = "mime_Jr"
                If pkmn.results(i).name = "porygon-z" Then newname = "porygon-Z"
                If pkmn.results(i).name = "giratina-altered" Then newname = "giratina-Altered"
                If pkmn.results(i).name = "shaymin-land" Then newname = "shaymin-Land"
                If pkmn.results(i).name = "basculin-red-striped" Then newname = "basculin"
                If pkmn.results(i).name = "darmanitan-standard" Then newname = "darmanitan"
                If pkmn.results(i).name = "deerling" Then newname = "deerling-Spring"
                If pkmn.results(i).name = "sawsbuck" Then newname = "sawsbuck-Spring"
                If pkmn.results(i).name = "tornadus-incarnate" Then newname = "tornadus"
                If pkmn.results(i).name = "thundurus-incarnate" Then newname = "thundurus"
                If pkmn.results(i).name = "landorus-incarnate" Then newname = "landorus"
                If pkmn.results(i).name = "keldeo-ordinary" Then newname = "keldeo"
                If pkmn.results(i).name = "meloetta-aria" Then newname = "meloetta"
                If pkmn.results(i).name = "flabebe" Then newname = "flabébé"
                If pkmn.results(i).name = "meowstic-male" Then newname = "meowstic"
                If pkmn.results(i).name = "aegislash-shield" Then newname = "aegislash"
                If pkmn.results(i).name = "pumpkaboo-average" Then newname = "pumpkaboo"
                If pkmn.results(i).name = "gourgeist-average" Then newname = "gourgeist"
                If pkmn.results(i).name = "oricorio-baile" Then newname = "oricorio-Baile"
                If pkmn.results(i).name = "lycanroc-midday" Then newname = "lycanroc"
                If pkmn.results(i).name = "wishiwashi-solo" Then newname = "wishiwashi-Solo"
                If pkmn.results(i).name = "type-null" Then newname = "type-Null"
                If pkmn.results(i).name = "minior-red-meteor" Then newname = "minior"
                If pkmn.results(i).name = "mimikyu-disguised" Then newname = "mimikyu"
                If pkmn.results(i).name = "tapu-koko" Then newname = "tapu-Koko"
                If pkmn.results(i).name = "tapu-lele" Then newname = "tapu-Lele"
                If pkmn.results(i).name = "tapu-bulu" Then newname = "tapu-Bulu"
                If pkmn.results(i).name = "tapu-fini" Then newname = "tapu-Fini"

                DownloadPkmn(i.ToString("000"), newname.Substring(0, 1).ToUpper() + newname.Substring(1))
                i = i + 1
                ttlPkmn = i
                Button1.Text = "Scrape " & i - 1 & "/" & count
            Loop Until i = count - 1
        End If
    End Sub

    Public Sub DownloadPkmn(ByVal pkmnID As String, ByVal pkmnName As String)
        Try
            Dim request As WebRequest = WebRequest.Create("https://bulbapedia.bulbagarden.net/wiki/File:" & pkmnID & pkmnName & ".png")
            Using response As WebResponse = request.GetResponse
                Using reading As New StreamReader(response.GetResponseStream())
                    Dim html As String = reading.ReadToEnd
                    Dim htmldoc = New HtmlAgilityPack.HtmlDocument()
                    htmldoc.LoadHtml(html.ToString())
                    For Each node In htmldoc.DocumentNode.SelectNodes("//img[@alt='File:" & pkmnID & pkmnName & ".png']")
                        RichTextBox1.AppendText(node.Attributes("src").Value() & Environment.NewLine)
                        RichTextBox1.ScrollToCaret()
                        Console.WriteLine(node.Attributes("src").Value())
                        Dim img As Image = getimage(node.Attributes("src").Value())
                        'img.Save(Application.StartupPath & "\pokemon\" & pkmnName & ".png", Imaging.ImageFormat.Png)
                    Next
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    Function getimage(ByVal url As String)
        Dim request As WebRequest = WebRequest.Create("https:" & url)
        Dim response As WebResponse = request.GetResponse
        Dim reader As Stream = response.GetResponseStream
        Dim bmp As New Bitmap(reader)
        reader.Dispose()
        Return bmp
    End Function

End Class
