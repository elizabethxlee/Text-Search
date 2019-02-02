'Name: ReadFile Project 3
'Purpose: Find the number of times a word is found in the entire file
'Programmer: Elizabeth Lee
Public Class frmMain
    'Declare variables
    Const DATA_FILE_NAME As String = "Datafile.txt"
    Dim inFile As IO.StreamReader



    '
    '
    '*******************SUBROUTINES********************
    '
    '

    'Purpose: Checks if the file exists
    Private Sub Check_If_File_Exists(ByRef inFile As IO.StreamReader)
        If IO.File.Exists(DATA_FILE_NAME) = True Then
            'if the file exists, opens the file
            inFile = IO.File.OpenText(DATA_FILE_NAME)
        Else
            'If the file doesn't exist, an error message is displayed
            MessageBox.Show("The File '" + DATA_FILE_NAME + "'" + " Does Not Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        End If
    End Sub


    'Purpose: displays all the lines that contain the search word into the textbox
    Private Sub findLinesWithSearchWord(ByRef inFile As IO.StreamReader, ByVal strLineFromFile As String, ByVal intIndex As Integer, ByVal strSearchWord As String)
        Do Until inFile.Peek = -1
            strLineFromFile = inFile.ReadLine + ControlChars.NewLine + ControlChars.NewLine
            intIndex = strLineFromFile.ToUpper.IndexOf(strSearchWord.ToUpper)
            If intIndex <> -1 Then
                txtDisplay.Text = txtDisplay.Text + strLineFromFile
            End If
        Loop
        If txtDisplay.Text = String.Empty Then
            txtDisplay.Text = "word/phrase does not appear in the text file"
        End If
    End Sub


    'Purpose: bolds and highlights the user's search word in all the displayed verses
    Private Sub boldAndHighlight(ByVal strSearchWord As String)
        txtDisplay.SuspendLayout()
        Dim PreviousPosition As Integer = txtDisplay.SelectionStart
        Dim PreviousSelection As Integer = txtDisplay.SelectionLength
        Dim SelectionIndex As Integer = -1

        Using BoldFont As New Font(txtDisplay.Font, FontStyle.Bold)
            While True
                SelectionIndex = txtDisplay.Find(strSearchWord, SelectionIndex + 1, RichTextBoxFinds.None)
                'Stops search if there is no more text
                If SelectionIndex < 0 Then Exit While
                'Indicates what portion of the text should be bolded & highlighted
                txtDisplay.SelectionStart = SelectionIndex
                txtDisplay.SelectionLength = strSearchWord.Length
                'Chooses the font style of the user's serach word
                txtDisplay.SelectionFont = BoldFont
                txtDisplay.SelectionColor = Color.Blue
            End While
        End Using
        txtDisplay.ResumeLayout()
        txtDisplay.SelectionStart = PreviousPosition
        txtDisplay.SelectionLength = PreviousSelection
    End Sub


    'Purpose: Counts the number of times the user's search word appears in the entire file
    Private Function CountOccurrences(ByRef inFile As IO.StreamReader, ByRef strEntireFile As String, ByVal strSearchWord As String, ByRef intIndex As Integer, ByRef intCounter As Integer) As Integer
        'If the word does not appear at all, Do While loop will not process
        intIndex = strEntireFile.ToUpper.IndexOf(strSearchWord.ToUpper)

        Do While intIndex <> -1
            intCounter += 1
            strEntireFile = strEntireFile.Substring(intIndex + 1)
            intIndex = strEntireFile.ToUpper.IndexOf(strSearchWord.ToUpper)
        Loop
        'Returns the number of times the word  appears in the entire file
        Return intCounter
    End Function


    '
    '
    '*****************EVENT PROCEDURES******************************8
    '
    '


    'EVENT: The program is run
    'Purpose: Calls the subroutine 'Check_If_File_Exists' as soon as program is run
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Calls subroutine to check if the file exists
        Call Check_If_File_Exists(inFile)
    End Sub


    'EVENT:     user presses 'Count' button
    'Purpose:   Counts the number of times a word is found in the text file
    Private Sub btnCount_Click(sender As Object, e As EventArgs) Handles btnCount.Click
        Dim strSearch As String = txtSearch.Text
        Dim intCounter As Integer
        Dim strEntireFile As String = ""
        Dim intIndex As Integer
        txtDisplay.Text = ""

        'prevents null reference exception
        Call Check_If_File_Exists(inFile)

        'Checks if user input is valid or if it is an empty string
        Select Case strSearch
            Case Is = String.Empty
                txtDisplay.Text = "Invalid entry"
            Case Else
                'stores all the text from the file into a string variable
                Do Until inFile.Peek = -1
                    strEntireFile = inFile.ReadLine + ControlChars.NewLine
                    'Invokes CountOccurrences function
                    intCounter = CountOccurrences(inFile, strEntireFile, strSearch, intIndex, intCounter)
                Loop
                'Displays the number of times a word appears
                txtDisplay.Text = "'" + strSearch.ToUpper + "'" + " appears " + intCounter.ToString + " times in the entire file."
                'If the file doesn't exist, a message is displayed
        End Select
        'brings focus back to the search text box
        txtSearch.Focus()
    End Sub


    'EVENT:     user presses 'Display' button
    'Purpose:   Displays each line of text that contains the user's search word
    '           Displays the user's search word in bold & in a different color
    Private Sub btnDisplay_Click(sender As Object, e As EventArgs) Handles btnDisplay.Click
        Dim strSearchWord As String = txtSearch.Text
        Dim strFileLine As String = ""
        Dim intIndex As Integer

        'Clears any previously displayed text
        txtDisplay.Text = ""

        'prevents null reference exception
        Call Check_If_File_Exists(inFile)

        'Checks if user input is valid or if it is an empty string
        Select Case strSearchWord
            Case Is = String.Empty
                txtDisplay.Text = "Invalid entry"

                'finds the lines of text that the search word appears in
            Case Else
                Call findLinesWithSearchWord(inFile, strFileLine, intIndex, strSearchWord)
                Call boldAndHighlight(strSearchWord)
        End Select

        'brings focus back to the search text box
        txtSearch.Focus()
    End Sub



    'EVENT:     user clicks the 'Exit' button
    'Purpose:   Closes the program
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub



    'EVENT:     user clicks the 'Clear' button
    'Purpose: Clears user input and output
    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        txtDisplay.Text = ""
        txtSearch.Text = ""
        txtSearch.Focus()
    End Sub



    'EVENT:     User presses any key
    'Purpose:   limits user input to only letters
    Private Sub txtSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearch.KeyPress
        If (e.KeyChar < "a" OrElse e.KeyChar > "z") AndAlso
            (e.KeyChar < "A" OrElse e.KeyChar > "Z") AndAlso
            e.KeyChar <> ControlChars.Back Then
            e.Handled = True
        End If
        'Clears the output if the user types in new input
        txtDisplay.Text = ""
    End Sub


End Class
