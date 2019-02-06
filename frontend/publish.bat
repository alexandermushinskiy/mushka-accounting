@echo off
@echo start frontend publishing..

ng build --prod --base-href /mushka/ &&^
xcopy /s /y dist C:\inetpub\wwwroot\mushka &&^
rd /s /q dist