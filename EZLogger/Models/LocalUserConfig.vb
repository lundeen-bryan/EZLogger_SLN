Namespace Models

    Public Class LocalUserConfig
        Public Property _comment As String
        Public Property this_config As ThisConfigSection
        Public Property sp_filepath As SPFilePathSection
        Public Property edo_filepath As EDOFilePathSection
    End Class

    Public Class ThisConfigSection
        Public Property _comment As String
        Public Property name As String
    End Class

    Public Class SPFilePathSection
        Public Property _comment As String
        Public Property databases As String
        Public Property user_forensic_database As String
        Public Property user_forensic_library As String
        Public Property court_contact As String
        Public Property da_contact_database As String
        Public Property doctors_list As String
        Public Property global_config_file As String
        Public Property hlv_data As String
        Public Property hlv_due As String
        Public Property ods_filepath As String
        Public Property properties_list As String
        Public Property sheriff_addresses As String
        Public Property templates As String
        Public Property ezl_database As String
    End Class

    Public Class EDOFilePathSection
        Public Property _comment As String
        Public Property forensic_office As String
        Public Property processed_reports As String
        Public Property tcars_folder As String
    End Class

End Namespace

