echo "Starting the execution of batch file: automation test execution"
if [$EmailNotification__Env -eq $null]
then
	echo "Execution Stopped"
else
	dotnet vstest FCSEETests.EomAPIEETestFramework.Web.dll --blame --logger:"nunit;LogFilePath=results.xml" --logger:trx --TestCaseFilter:Category=APITest
	dotnet FCSEETests.EomAPIEETestFramework.TrxToHtml.dll
	dotnet FCSEETests.EomAPIEETestFramework.EmailNotifier.dll
fi