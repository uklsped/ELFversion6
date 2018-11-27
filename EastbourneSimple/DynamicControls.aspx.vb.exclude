
Partial Class DynamicControls
    Inherits System.Web.UI.Page
    Const ALPHABET_SELECTION As String = "ALPHABET"
    Const NUMBER_SELECTION As String = "NUMBERS"
    Const VIEWSTATEKEY_DYNCONTROL As String = "DynamicControlSelection"

    'store property value in viewstate so that it will survive postbacks
    Private Property DynamicControlSelection() As String
        Get
            Dim result As String = ViewState.Item(VIEWSTATEKEY_DYNCONTROL)
            If result Is Nothing Then
                'doing things like this lets us access this property without
                'worrying about this property returning null/Nothing
                Return String.Empty
            Else
                Return result
            End If
        End Get
        Set(ByVal value As String)
            ViewState.Item(VIEWSTATEKEY_DYNCONTROL) = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) _
      Handles Me.Load
        'running this code on every page_load - even when it's a postback
        'check our page property (that we stored in viewstate) to see 
        'if we need to load a specific set of dynamic controls
        Select Case Me.DynamicControlSelection
            Case ALPHABET_SELECTION
                CreateDynamicAlphabetLinks()

            Case NUMBER_SELECTION
                CreateDynamicNumberButtons()

            Case Else
                'no dynamic controls need to be loaded...yet
        End Select

    End Sub

    Private Sub onClick(ByVal sender As Object, ByVal e As EventArgs)
        'all of the dynamic linkbuttons/buttons will trigger this event handler
        'since we used both linkbuttons and regular buttons for our dynamic controls, 
        'we will cast the sender control to an interface that is common to both
        'of those button controls - the IButtonControl interface
        Dim btn As IButtonControl = DirectCast(sender, IButtonControl)
        Me.lblClickResult.Text =
           String.Format("You clicked - CommandName: {0}  CommandArgument: {1}",
             btn.CommandName, btn.CommandArgument)
    End Sub

    Protected Sub cmdAlphabet_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
      Handles cmdAlphabet.Click
        'user is selecting to show the dynamic Alphabet buttons
        Me.CreateDynamicAlphabetLinks()
    End Sub

    Protected Sub cmdNumbers_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
      Handles cmdNumbers.Click
        'user is selecting to show the dynamic Number buttons
        Me.CreateDynamicNumberButtons()
    End Sub

    Private Sub CreateDynamicAlphabetLinks()

        'clear the placeholder first - in case something else was dynamically loaded
        Me.PlaceHolder1.Controls.Clear()

        'dynamically create a series of linkbuttons
        For keycode As Integer = 65 To 90 'one for each letter in the alphabet
            Dim lnk As New LinkButton

            'assign the ID ourself to make sure it is consistent
            'if you let the framework assign it, the dynamic control may
            'not behave correctly
            lnk.ID = "alpha_" & keycode.ToString
            lnk.Text = Chr(keycode)

            'we'll add a CommandName and a CommandArgument
            'so we can determine what was clicked when the event is raised
            lnk.CommandName = "ALPHABET"
            lnk.CommandArgument = Chr(keycode)

            'have them all use the same event handler
            AddHandler lnk.Click, AddressOf onClick

            'add these dynamic controls to our strategically place placeholder control
            'the position of the placeholder determines 
            'where on the page the dynamic controls will appear
            Me.PlaceHolder1.Controls.Add(lnk)
            Me.PlaceHolder1.Controls.Add(New LiteralControl(" ")) 'space them out
        Next

        'VERY IMPORTANT -> remember that we created these controls for the next postback
        Me.DynamicControlSelection = ALPHABET_SELECTION

    End Sub

    Private Sub CreateDynamicNumberButtons()

        'clear the placeholder first - in case something else was dynamically loaded
        Me.PlaceHolder1.Controls.Clear()

        'dynamically create a series of button controls
        For number As Integer = 0 To 25
            Dim btn As New Button
            'assign the ID ourself to make sure it is consistent
            'if you let the framework assign it, the dynamic control may
            'not behave correctly
            btn.ID = "number_" & number.ToString
            btn.Text = number.ToString

            'we'll add a CommandName and a CommandArgument
            'so we can determine what was clicked when the event is raised
            btn.CommandName = "NUMBER"
            btn.CommandArgument = number.ToString

            'have them all use the same event handler
            AddHandler btn.Click, AddressOf onClick

            'add these dynamic controls to our strategically place placeholder control
            'the position of the placeholder determines 
            'where on the page the dynamic controls will appear
            Me.PlaceHolder1.Controls.Add(btn)
            Me.PlaceHolder1.Controls.Add(New LiteralControl(" ")) 'space them out
        Next

        'VERY IMPORTANT -> remember that we created these controls for the next postback
        Me.DynamicControlSelection = NUMBER_SELECTION

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) _
      Handles Me.PreRender
        Me.lblViewStateValue.Text = Me.DynamicControlSelection
    End Sub
End Class
