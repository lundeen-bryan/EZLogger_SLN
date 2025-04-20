Namespace Models

    ''' <summary>
    ''' Represents version information loaded from the global_config.json file.
    ''' Used by the AboutViewHandler to pass metadata to the AboutView.
    ''' </summary>
    Public Class AboutInfoResult

        ''' <summary>
        ''' The name of the person who created this version of the application.
        ''' </summary>
        Public Property CreatedBy As String

        ''' <summary>
        ''' The support contact email address.
        ''' </summary>
        Public Property SupportEmail As String

        ''' <summary>
        ''' The date of the most recent update.
        ''' </summary>
        Public Property LastUpdate As String

        ''' <summary>
        ''' The version number of the application.
        ''' </summary>
        Public Property VersionNumber As String

        ''' <summary>
        ''' A short description of the latest change or instruction.
        ''' </summary>
        Public Property LatestChange As String

        ''' <summary>
        ''' If loading fails, this contains the error message to be displayed.
        ''' </summary>
        Public Property ErrorMessage As String

        ''' <summary>
        ''' Returns True if an error occurred while loading version info.
        ''' </summary>
        Public ReadOnly Property HasError As Boolean
            Get
                Return Not String.IsNullOrEmpty(ErrorMessage)
            End Get
        End Property

    End Class

End Namespace
