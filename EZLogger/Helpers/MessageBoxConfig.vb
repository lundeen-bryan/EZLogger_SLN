'''<summary>
'''Defines the configuration options for displaying custom message boxes
'''in EZLogger. This class is used by MsgBoxHelper to control which
'''buttons (Yes, No, Ok) should be shown and what message should be
'''displayed. This file is located in the Helpers folder because it
'''supports helper functionality and is not an Enum or standalone UI
'''component.
'''</summary>
Public Class MessageBoxConfig
    Public Property Message As String
    Public Property ShowYes As Boolean = False
    Public Property ShowNo As Boolean = False
    Public Property ShowOk As Boolean = True
End Class
