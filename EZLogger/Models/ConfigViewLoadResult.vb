Namespace Models

    ''' <summary>
    ''' Contains data needed to populate the ConfigView when it is loaded.
    ''' </summary>
    Public Class ConfigViewLoadResult

        ''' <summary>
        ''' List of doctor names to populate the doctors textbox.
        ''' </summary>
        Public Property DoctorList As List(Of String)

        ''' <summary>
        ''' Full path to the local_user_config.json file or a message if not found.
        ''' </summary>
        Public Property LocalConfigPath As String

        ''' <summary>
        ''' Global config file path or a message if it is not set.
        ''' </summary>
        Public Property GlobalConfigPathMessage As String
        Public Property ForensicDatabasePath As String
        Public Property ForensicLibraryPath As String
        Public Property ForensicOfficePath As String

    End Class

End Namespace
