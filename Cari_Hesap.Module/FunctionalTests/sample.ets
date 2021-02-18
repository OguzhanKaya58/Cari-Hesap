;#DropDB Cari HesapEasyTest

#Application Cari HesapWin
#Application Cari HesapWeb

*FillForm
 User Name = Admin
*Action Log In

#IfDef Cari HesapWin

*Action About...
 
*Action OK

#EndIf

#IfDef Cari HesapWeb
*Action Navigation(My Details)
#EndIf
